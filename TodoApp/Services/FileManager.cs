using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Services
{
    [Obsolete("Use DatabaseDataStorage with Entity Framework Core instead.")]
    public class FileManager : IDataStorage
    {
        private const string ProfilesFileName = "profiles.dat";
        private const string TodoFilePrefix = "todos_";
        private const string TodoFileExtension = ".dat";

        private static readonly byte[] Key =
        {
            0x25, 0x7A, 0x2D, 0xA8, 0x31, 0x6C, 0x99, 0xE0,
            0x4B, 0x12, 0xF4, 0x70, 0xC8, 0x55, 0x91, 0x3D,
            0xA1, 0x0E, 0xB7, 0x62, 0x19, 0xDC, 0x43, 0x8A,
            0xF0, 0x36, 0x5E, 0xC1, 0x77, 0x2B, 0x94, 0x09
        };

        private static readonly byte[] IV =
        {
            0xA4, 0x19, 0xC3, 0x5D, 0x6E, 0x80, 0x2F, 0xB1,
            0x47, 0xD9, 0x03, 0xEA, 0x7C, 0x58, 0x92, 0x10
        };

        private readonly string _dataDirectory;

        public FileManager(string dataDirectory = "data")
        {
            _dataDirectory = dataDirectory;
            try
            {
                EnsureDataDirectory();
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new DataStorageException("Нет доступа для создания каталога данных.", ex);
            }
            catch (IOException ex)
            {
                throw new DataStorageException("Ошибка файловой системы при создании каталога данных.", ex);
            }
        }

        public void SaveProfiles(IEnumerable<Profile> profiles)
        {
            try
            {
                using var writer = CreateEncryptedWriter(GetProfilesPath());

                foreach (var profile in profiles)
                {
                    writer.WriteLine(SerializeProfile(profile));
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new DataStorageException("Нет доступа для сохранения профилей.", ex);
            }
            catch (IOException ex)
            {
                throw new DataStorageException("Ошибка файловой системы при сохранении профилей.", ex);
            }
            catch (CryptographicException ex)
            {
                throw new DataStorageException("Ошибка шифрования профилей.", ex);
            }
        }

        public IEnumerable<Profile> LoadProfiles()
        {
            string path = GetProfilesPath();
            if (!File.Exists(path))
            {
                return new List<Profile>();
            }

            try
            {
                var profiles = new List<Profile>();
                using var reader = CreateEncryptedReader(path);

                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        profiles.Add(ParseProfile(line));
                    }
                }

                return profiles;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new DataStorageException("Нет доступа для чтения профилей.", ex);
            }
            catch (IOException ex)
            {
                throw new DataStorageException("Ошибка файловой системы при чтении профилей.", ex);
            }
            catch (CryptographicException ex)
            {
                throw new DataStorageException("Не удалось расшифровать профили. Данные повреждены или ключ неверный.", ex);
            }
            catch (FormatException ex)
            {
                throw new DataStorageException("Файл профилей содержит повреждённые данные.", ex);
            }
        }

        public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
        {
            try
            {
                using var writer = CreateEncryptedWriter(GetTodoPath(userId));
                int index = 0;

                foreach (var todo in todos)
                {
                    writer.WriteLine(SerializeTodo(index, todo));
                    index++;
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new DataStorageException("Нет доступа для сохранения задач.", ex);
            }
            catch (IOException ex)
            {
                throw new DataStorageException("Ошибка файловой системы при сохранении задач.", ex);
            }
            catch (CryptographicException ex)
            {
                throw new DataStorageException("Ошибка шифрования задач.", ex);
            }
        }

        public IEnumerable<TodoItem> LoadTodos(Guid userId)
        {
            string path = GetTodoPath(userId);
            if (!File.Exists(path))
            {
                return new List<TodoItem>();
            }

            try
            {
                var todos = new List<TodoItem>();
                using var reader = CreateEncryptedReader(path);

                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        todos.Add(ParseTodo(line));
                    }
                }

                return todos;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new DataStorageException("Нет доступа для чтения задач.", ex);
            }
            catch (IOException ex)
            {
                throw new DataStorageException("Ошибка файловой системы при чтении задач.", ex);
            }
            catch (CryptographicException ex)
            {
                throw new DataStorageException("Не удалось расшифровать задачи. Данные повреждены или ключ неверный.", ex);
            }
            catch (FormatException ex)
            {
                throw new DataStorageException("Файл задач содержит повреждённые данные.", ex);
            }
        }

        private void EnsureDataDirectory()
        {
            Directory.CreateDirectory(_dataDirectory);
        }

        private StreamWriter CreateEncryptedWriter(string path)
        {
            var aes = CreateAes();
            var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            var bufferedStream = new BufferedStream(fileStream);
            var cryptoStream = new CryptoStream(bufferedStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

            return new StreamWriter(cryptoStream, Encoding.UTF8);
        }

        private StreamReader CreateEncryptedReader(string path)
        {
            var aes = CreateAes();
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var bufferedStream = new BufferedStream(fileStream);
            var cryptoStream = new CryptoStream(bufferedStream, aes.CreateDecryptor(), CryptoStreamMode.Read);

            return new StreamReader(cryptoStream, Encoding.UTF8);
        }

        private Aes CreateAes()
        {
            var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            return aes;
        }

        private string GetProfilesPath()
        {
            return Path.Combine(_dataDirectory, ProfilesFileName);
        }

        private string GetTodoPath(Guid userId)
        {
            return Path.Combine(_dataDirectory, $"{TodoFilePrefix}{userId}{TodoFileExtension}");
        }

        private string SerializeProfile(Profile profile)
        {
            return string.Join(";",
                profile.Id,
                Escape(profile.Login),
                Escape(profile.Password),
                Escape(profile.FirstName),
                Escape(profile.LastName),
                profile.BirthYear.ToString(CultureInfo.InvariantCulture));
        }

        private Profile ParseProfile(string line)
        {
            var parts = ParseLine(line);
            if (parts.Count != 6)
            {
                throw new FormatException("Некорректная строка профиля.");
            }

            return new Profile
            {
                Id = Guid.Parse(parts[0]),
                Login = Unescape(parts[1]),
                Password = Unescape(parts[2]),
                FirstName = Unescape(parts[3]),
                LastName = Unescape(parts[4]),
                BirthYear = int.Parse(parts[5], CultureInfo.InvariantCulture)
            };
        }

        private string SerializeTodo(int index, TodoItem todo)
        {
            return string.Join(";",
                index.ToString(CultureInfo.InvariantCulture),
                Escape(todo.Text),
                todo.Status,
                todo.LastUpdate.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
        }

        private TodoItem ParseTodo(string line)
        {
            var parts = ParseLine(line);
            if (parts.Count < 4)
            {
                throw new FormatException("Некорректная строка задачи.");
            }

            var status = Enum.Parse<TodoStatus>(parts[2]);
            var lastUpdate = DateTime.Parse(parts[3], CultureInfo.InvariantCulture);

            return new TodoItem(Unescape(parts[1]))
            {
                Status = status,
                LastUpdate = lastUpdate
            };
        }

        private string Escape(string text)
        {
            return "\"" + text.Replace("\"", "\"\"").Replace("\r", "").Replace("\n", "\\n") + "\"";
        }

        private string Unescape(string text)
        {
            return text.Trim('"').Replace("\\n", "\n").Replace("\"\"", "\"");
        }

        private List<string> ParseLine(string line)
        {
            var parts = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    current.Append(c);
                }
                else if (c == ';' && !inQuotes)
                {
                    parts.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            if (inQuotes)
            {
                throw new FormatException("Незакрытая кавычка в строке данных.");
            }

            parts.Add(current.ToString());
            return parts;
        }
    }
}
