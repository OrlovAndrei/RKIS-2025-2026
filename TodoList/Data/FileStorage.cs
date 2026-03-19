using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using TodoList.Interfaces;
using TodoList.Security;
using TodoList.Exceptions;

namespace TodoList.Data
{
    public class FileStorage : IDataStorage
    {
        private readonly string _baseDirectory;
        private readonly IFileSystem _fileSystem;

        public FileStorage(string baseDirectory) : this(baseDirectory, new RealFileSystem())
        {
        }

        public FileStorage(string baseDirectory, IFileSystem fileSystem)
        {
            _baseDirectory = baseDirectory;
            _fileSystem = fileSystem;
            _fileSystem.CreateDirectory(_baseDirectory);
        }

        private string GetProfilesPath() => Path.Combine(_baseDirectory, "profiles.dat");

        private string GetTodosPath(Guid userId) => Path.Combine(_baseDirectory, $"todos_{userId}.dat");

        public void SaveProfiles(IEnumerable<Profile> profiles)
        {
            var filePath = GetProfilesPath();
            
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = EncryptionHelper.CreateEncryptStream(memoryStream))
            using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
            {
                writer.WriteLine("Id;Login;Password;FirstName;LastName;BirthYear");
                
                foreach (var profile in profiles)
                {
                    writer.WriteLine($"{profile.Id};{profile.Login};{profile.Password};{profile.FirstName};{profile.LastName};{profile.BirthYear}");
                }
                
                writer.Flush();
                cryptoStream.Flush();
                
                byte[] encryptedData = memoryStream.ToArray();
                _fileSystem.WriteAllBytes(filePath, encryptedData);
            }
        }

        public IEnumerable<Profile> LoadProfiles()
        {
            var filePath = GetProfilesPath();
            var profiles = new List<Profile>();

            if (!_fileSystem.FileExists(filePath))
            {
                return profiles;
            }

            try
            {
                byte[] encryptedData = _fileSystem.ReadAllBytes(filePath);
                
                using (var memoryStream = new MemoryStream(encryptedData))
                using (var cryptoStream = EncryptionHelper.CreateDecryptStream(memoryStream))
                using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
                {
                    var header = reader.ReadLine();
                    if (header == null) return profiles;

                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var parts = line.Split(';');
                        if (parts.Length < 6) continue;

                        try
                        {
                            var profile = new Profile(
                                Guid.Parse(parts[0]),
                                parts[1],
                                parts[2],
                                parts[3],
                                parts[4],
                                int.Parse(parts[5])
                            );
                            profiles.Add(profile);
                        }
                        catch (FormatException)
                        {
                            continue;
                        }
                    }
                }
            }
            catch (CryptographicException)
            {
                throw new InvalidDataException("Файл профилей поврежден или имеет неверный формат.");
            }
            catch (IOException ex)
            {
                throw new InvalidDataException($"Ошибка доступа к файлу профилей: {ex.Message}");
            }

            return profiles;
        }

        public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
        {
            var filePath = GetTodosPath(userId);
            
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = EncryptionHelper.CreateEncryptStream(memoryStream))
            using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
            {
                writer.WriteLine("Index;Text;Status;LastUpdate");

                int index = 0;
                foreach (var todo in todos)
                {
                    string textEscaped = todo.Text.Replace("\n", "\\n").Replace("\"", "\"\"");
                    writer.WriteLine($"{index};\"{textEscaped}\";{todo.Status};{todo.LastUpdate:O}");
                    index++;
                }
                
                writer.Flush();
                cryptoStream.Flush();
                
                byte[] encryptedData = memoryStream.ToArray();
                _fileSystem.WriteAllBytes(filePath, encryptedData);
            }
        }

        public IEnumerable<TodoItem> LoadTodos(Guid userId)
        {
            var filePath = GetTodosPath(userId);
            var todos = new List<TodoItem>();

            if (!_fileSystem.FileExists(filePath))
            {
                return todos;
            }

            try
            {
                byte[] encryptedData = _fileSystem.ReadAllBytes(filePath);
                
                using (var memoryStream = new MemoryStream(encryptedData))
                using (var cryptoStream = EncryptionHelper.CreateDecryptStream(memoryStream))
                using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
                {
                    var header = reader.ReadLine();
                    if (header == null) return todos;

                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var parts = new List<string>();
                        bool inQuotes = false;
                        string currentPart = "";

                        foreach (char c in line)
                        {
                            if (c == '"')
                            {
                                inQuotes = !inQuotes;
                            }
                            else if (c == ';' && !inQuotes)
                            {
                                parts.Add(currentPart);
                                currentPart = "";
                            }
                            else
                            {
                                currentPart += c;
                            }
                        }
                        parts.Add(currentPart);

                        if (parts.Count < 4) continue;

                        string textRaw = parts[1].Trim('"').Replace("\\n", "\n").Replace("\"\"", "\"");
                        
                        if (Enum.TryParse<TodoStatus>(parts[2], out TodoStatus status))
                        {
                            if (DateTime.TryParse(parts[3], out DateTime date))
                            {
                                var item = new TodoItem(textRaw, status, date);
                                todos.Add(item);
                            }
                        }
                    }
                }
            }
            catch (CryptographicException)
            {
                throw new InvalidDataException($"Файл задач для пользователя {userId} поврежден.");
            }
            catch (IOException ex)
            {
                throw new InvalidDataException($"Ошибка доступа к файлу задач: {ex.Message}");
            }

            return todos;
        }
    }
}