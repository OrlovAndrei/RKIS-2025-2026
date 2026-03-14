using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TodoList.Exceptions;

namespace TodoList
{
    public class FileManager : IDataStorage
    {
        private readonly string _dataDir;
        private readonly string _profilesFileName = "profile.dat";
        private readonly string _todosFolderName = "todos";

        private static readonly byte[] Key = Encoding.UTF8.GetBytes("0123456789abcdef0123456789abcdef"); 
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("abcdefghijklmnop");                 

        public FileManager(string dataDir)
        {
            _dataDir = dataDir;
            EnsureDirectories();
        }

        private void EnsureDirectories()
        {
            if (!Directory.Exists(_dataDir))
                Directory.CreateDirectory(_dataDir);

            string todosPath = Path.Combine(_dataDir, _todosFolderName);
            if (!Directory.Exists(todosPath))
                Directory.CreateDirectory(todosPath);
        }

        private string GetProfilesFilePath() => Path.Combine(_dataDir, _profilesFileName);
        private string GetUserTodosFilePath(Guid userId) =>
            Path.Combine(_dataDir, _todosFolderName, $"todos_{userId}.dat");

        public void SaveProfiles(IEnumerable<Profile> profiles)
        {
            string filePath = GetProfilesFilePath();

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var bufferedStream = new BufferedStream(fileStream))
            using (var aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
                {
                    foreach (var profile in profiles)
                    {
                        string line = FormatProfile(profile);
                        writer.WriteLine(line);
                    }
                }
            }
        }

        public IEnumerable<Profile> LoadProfiles()
        {
            string filePath = GetProfilesFilePath();
            if (!File.Exists(filePath))
                return new List<Profile>();

            var profiles = new List<Profile>();

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (var bufferedStream = new BufferedStream(fileStream))
                using (var aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;

                    using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (TryParseProfile(line, out Profile? profile))
                                profiles.Add(profile!);
                        }
                    }
                }
            }
            catch (CryptographicException)
            {
                throw new InvalidDataException("Файл профилей повреждён или используется неверный ключ шифрования.");
            }

            return profiles;
        }

        public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
        {
            string filePath = GetUserTodosFilePath(userId);

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var bufferedStream = new BufferedStream(fileStream))
            using (var aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
                {
                    foreach (var todo in todos)
                    {
                        string line = FormatTodo(todo);
                        writer.WriteLine(line);
                    }
                }
            }
        }

        public IEnumerable<TodoItem> LoadTodos(Guid userId)
        {
            string filePath = GetUserTodosFilePath(userId);
            if (!File.Exists(filePath))
                return new List<TodoItem>();

            var todos = new List<TodoItem>();

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (var bufferedStream = new BufferedStream(fileStream))
                using (var aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;

                    using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (TryParseTodo(line, out TodoItem? todo))
                                todos.Add(todo!);
                        }
                    }
                }
            }
            catch (CryptographicException)
            {
                throw new InvalidDataException($"Файл задач пользователя {userId} повреждён или используется неверный ключ шифрования.");
            }

            return todos;
        }

        private string FormatProfile(Profile profile)
        {
            return $"{EscapeCsvField(profile.Id.ToString())};" +
                   $"{EscapeCsvField(profile.Login)};" +
                   $"{EscapeCsvField(profile.Password)};" +
                   $"{EscapeCsvField(profile.FirstName)};" +
                   $"{EscapeCsvField(profile.LastName)};" +
                   $"{profile.BirthYear}";
        }

        private bool TryParseProfile(string line, out Profile? profile)
        {
            profile = null;
            if (string.IsNullOrWhiteSpace(line))
                return false;

            var parts = ParseCsvLine(line, ';');
            if (parts.Length == 6 &&
                Guid.TryParse(parts[0], out Guid id) &&
                int.TryParse(parts[5], out int birthYear))
            {
                profile = new Profile(id, parts[1], parts[2], parts[3], parts[4], birthYear);
                return true;
            }
            return false;
        }

        private string FormatTodo(TodoItem todo)
        {
            return $"{EscapeCsvField(todo.Text)};{todo.Status};{todo.LastUpdate:yyyy-MM-dd HH:mm:ss}";
        }

        private bool TryParseTodo(string line, out TodoItem? todo)
        {
            todo = null;
            if (string.IsNullOrWhiteSpace(line))
                return false;

            var parts = ParseCsvLine(line, ';');
            if (parts.Length == 3 &&
                DateTime.TryParse(parts[2], out DateTime lastUpdate))
            {
                if (Enum.TryParse<TodoStatus>(parts[1], out TodoStatus status))
                {
                    todo = new TodoItem(UnescapeCsvField(parts[0]), status, lastUpdate);
                    return true;
                }
            }
            return false;
        }

        private static string[] ParseCsvLine(string line, char delimiter)
        {
            var result = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == delimiter && !inQuotes)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }
            result.Add(current.ToString());
            return result.ToArray();
        }

        private static string EscapeCsvField(string field)
        {
            if (field.Contains(";") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                string temp = field.Replace("\n", "|NL|").Replace("\r", "|CR|");
                temp = temp.Replace("\"", "\"\"");
                return "\"" + temp + "\"";
            }
            return field;
        }

        private static string UnescapeCsvField(string field)
        {
            if (field.StartsWith("\"") && field.EndsWith("\""))
            {
                field = field.Substring(1, field.Length - 2);
            }
            field = field.Replace("|NL|", "\n").Replace("|CR|", "\r");
            field = field.Replace("\"\"", "\"");
            return field;
        }
    }
}