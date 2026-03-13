using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Todolist
{
    public class FileManager : IDataStorage
    {
        private readonly string _dataDirPath;
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public FileManager(string dataDirPath = null)
        {
            _dataDirPath = dataDirPath ?? Path.Combine(Directory.GetCurrentDirectory(), "data");
            
            _key = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF");
            _iv = Encoding.UTF8.GetBytes("0123456789ABCDEF");
            
            EnsureDataDirectory();
        }

        private void EnsureDataDirectory()
        {
            if (!Directory.Exists(_dataDirPath))
                Directory.CreateDirectory(_dataDirPath);
        }

        private string GetProfilesFilePath()
        {
            return Path.Combine(_dataDirPath, "profiles.dat");
        }

        private string GetTodosFilePath(Guid userId)
        {
            return Path.Combine(_dataDirPath, $"todos_{userId}.dat");
        }

        private Stream CreateEncryptStream(Stream destination)
        {
            var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            return new CryptoStream(destination, aes.CreateEncryptor(), CryptoStreamMode.Write);
        }

        private Stream CreateDecryptStream(Stream source)
        {
            var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            return new CryptoStream(source, aes.CreateDecryptor(), CryptoStreamMode.Read);
        }

        public void SaveProfiles(IEnumerable<Profile> profiles)
        {
            try
            {
                using (var fileStream = new FileStream(GetProfilesFilePath(), FileMode.Create, FileAccess.Write))
                using (var bufferedStream = new BufferedStream(fileStream, 8192))
                using (var cryptoStream = CreateEncryptStream(bufferedStream))
                using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
                {
                    foreach (var profile in profiles)
                    {
                        string line = $"{profile.Id}|{Escape(profile.Login)}|{Escape(profile.Password)}|{Escape(profile.FirstName)}|{Escape(profile.LastName)}|{profile.BirthYear}";
                        writer.WriteLine(line);
                    }
                }
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"Ошибка доступа к файлу профилей: {ex.Message}", ex);
            }
            catch (CryptographicException ex)
            {
                throw new InvalidOperationException($"Ошибка шифрования данных профилей: {ex.Message}", ex);
            }
        }

        public IEnumerable<Profile> LoadProfiles()
        {
            var profiles = new List<Profile>();
            string filePath = GetProfilesFilePath();

            if (!File.Exists(filePath))
                return profiles;

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (var bufferedStream = new BufferedStream(fileStream, 8192))
                using (var cryptoStream = CreateDecryptStream(bufferedStream))
                using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length == 6)
                        {
                            try
                            {
                                Guid id = Guid.Parse(parts[0]);
                                string login = Unescape(parts[1]);
                                string password = Unescape(parts[2]);
                                string firstName = Unescape(parts[3]);
                                string lastName = Unescape(parts[4]);
                                int birthYear = int.Parse(parts[5]);

                                profiles.Add(new Profile(id, login, password, firstName, lastName, birthYear));
                            }
                            catch (FormatException ex)
                            {
                                throw new InvalidDataException($"Поврежденные данные профиля: {ex.Message}", ex);
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"Ошибка доступа к файлу профилей: {ex.Message}", ex);
            }
            catch (CryptographicException ex)
            {
                throw new InvalidOperationException($"Ошибка расшифровки данных профилей: {ex.Message}", ex);
            }

            return profiles;
        }

        public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("ID пользователя не может быть пустым", nameof(userId));

            try
            {
                using (var fileStream = new FileStream(GetTodosFilePath(userId), FileMode.Create, FileAccess.Write))
                using (var bufferedStream = new BufferedStream(fileStream, 8192))
                using (var cryptoStream = CreateEncryptStream(bufferedStream))
                using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
                {
                    foreach (var item in todos)
                    {
                        string escapedText = Escape(item.Text);
                        string line = $"{item.Status}|{item.LastUpdate:yyyy-MM-ddTHH:mm:ss}|{escapedText}";
                        writer.WriteLine(line);
                    }
                }
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"Ошибка доступа к файлу задач пользователя {userId}: {ex.Message}", ex);
            }
            catch (CryptographicException ex)
            {
                throw new InvalidOperationException($"Ошибка шифрования данных задач пользователя {userId}: {ex.Message}", ex);
            }
        }

        public IEnumerable<TodoItem> LoadTodos(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("ID пользователя не может быть пустым", nameof(userId));

            var todos = new List<TodoItem>();
            string filePath = GetTodosFilePath(userId);

            if (!File.Exists(filePath))
                return todos;

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (var bufferedStream = new BufferedStream(fileStream, 8192))
                using (var cryptoStream = CreateDecryptStream(bufferedStream))
                using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length >= 3)
                        {
                            try
                            {
                                TodoStatus status = Enum.Parse<TodoStatus>(parts[0]);
                                DateTime lastUpdate = DateTime.Parse(parts[1]);

                                string text = Unescape(string.Join("|", parts, 2, parts.Length - 2));

                                todos.Add(new TodoItem(text, status, lastUpdate));
                            }
                            catch (FormatException ex)
                            {
                                throw new InvalidDataException($"Поврежденные данные задачи: {ex.Message}", ex);
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"Ошибка доступа к файлу задач пользователя {userId}: {ex.Message}", ex);
            }
            catch (CryptographicException ex)
            {
                throw new InvalidOperationException($"Ошибка расшифровки данных задач пользователя {userId}: {ex.Message}", ex);
            }

            return todos;
        }

        private string Escape(string text)
        {
            if (text == null) return "";
            return text.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("|", "\\|");
        }

        private string Unescape(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Replace("\\|", "|").Replace("\\n", "\n").Replace("\\\\", "\\");
        }
    }
}