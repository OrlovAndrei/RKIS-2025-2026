using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class FileManager : IDataStorage
{
    private readonly string _dataDirectory;
    private readonly byte[] _aesKey;
    private readonly byte[] _aesIV;

    public FileManager(string dataDirectory)
    {
        _dataDirectory = dataDirectory;

        _aesKey = Encoding.UTF8.GetBytes("12345678901234567890123456789012"); 
        _aesIV = Encoding.UTF8.GetBytes("1234567890123456"); 

        EnsureDataDirectory();
    }

    private void EnsureDataDirectory()
    {
        if (!Directory.Exists(_dataDirectory))
        {
            Directory.CreateDirectory(_dataDirectory);
        }
    }

    private string GetProfilesFilePath()
    {
        return Path.Combine(_dataDirectory, "profiles.dat");
    }

    public string GetUserTodoFilePath(Guid userId)
    {
        return Path.Combine(_dataDirectory, $"todos_{userId}.dat");
    }

    public void SaveProfiles(IEnumerable<Profile> profiles)
    {
        string filePath = GetProfilesFilePath();

        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        using (var bufferedStream = new BufferedStream(fileStream, 8192))
        using (var aes = Aes.Create())
        using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateEncryptor(_aesKey, _aesIV), CryptoStreamMode.Write))
        using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
        {
            writer.WriteLine("Id;Login;Password;FirstName;LastName;BirthYear");

            foreach (var profile in profiles)
            {
                string line = $"{profile.Id};{profile.Login};{profile.Password};{profile.FirstName};{profile.LastName};{profile.BirthYear}";
                writer.WriteLine(line);
            }

            writer.Flush();
            cryptoStream.FlushFinalBlock();
        }
    }

    public IEnumerable<Profile> LoadProfiles()
    {
        string filePath = GetProfilesFilePath();
        var profiles = new List<Profile>();

        if (!File.Exists(filePath))
        {
            return profiles;
        }

        try
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var bufferedStream = new BufferedStream(fileStream, 8192))
            using (var aes = Aes.Create())
            using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateDecryptor(_aesKey, _aesIV), CryptoStreamMode.Read))
            using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
            {
                string? line;
                bool isFirstLine = true;

                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue; 
                    }

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] parts = line.Split(';');

                    if (parts.Length == 6)
                    {
                        try
                        {
                            Guid id = Guid.Parse(parts[0]);
                            string login = parts[1];
                            string password = parts[2];
                            string firstName = parts[3];
                            string lastName = parts[4];
                            int birthYear = int.Parse(parts[5]);

                            profiles.Add(new Profile(id, login, password, firstName, lastName, birthYear));
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidDataException($"Ошибка при парсинге профиля: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (CryptographicException)
        {
            throw new InvalidDataException("Файл профилей поврежден или не может быть расшифрован");
        }
        catch (IOException ex)
        {
            throw new IOException($"Ошибка доступа к файлу профилей: {ex.Message}");
        }

        return profiles;
    }

    public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
    {
        string filePath = GetUserTodoFilePath(userId);

        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        using (var bufferedStream = new BufferedStream(fileStream, 8192))
        using (var aes = Aes.Create())
        using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateEncryptor(_aesKey, _aesIV), CryptoStreamMode.Write))
        using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
        {
            writer.WriteLine("Index;Text;Status;LastUpdate");

            int index = 0;
            foreach (var item in todos)
            {
                string escapedText = EscapeCsvText(item.Text);
                string line = $"{index};\"{escapedText}\";{item.Status};{item.LastUpdate:yyyy-MM-ddTHH:mm:ss}";
                writer.WriteLine(line);
                index++;
            }

            writer.Flush();
            cryptoStream.FlushFinalBlock();
        }
    }

    public IEnumerable<TodoItem> LoadTodos(Guid userId)
    {
        string filePath = GetUserTodoFilePath(userId);
        var todoList = new List<TodoItem>();

        if (!File.Exists(filePath))
        {
            return todoList;
        }

        try
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var bufferedStream = new BufferedStream(fileStream, 8192))
            using (var aes = Aes.Create())
            using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateDecryptor(_aesKey, _aesIV), CryptoStreamMode.Read))
            using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
            {
                string? line;
                bool isFirstLine = true;

                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue; 
                    }

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] parts = ParseCsvLine(line);

                    if (parts.Length == 4)
                    {
                        try
                        {
                            string text = UnescapeCsvText(parts[1]);
                            TodoStatus status = (TodoStatus)Enum.Parse(typeof(TodoStatus), parts[2]);
                            DateTime lastUpdate = DateTime.Parse(parts[3]);

                            TodoItem item = new TodoItem(text);
                            item.SetStatus(status);
                            item.SetLastUpdate(lastUpdate);

                            todoList.Add(item);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidDataException($"Ошибка при парсинге задачи: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (CryptographicException)
        {
            throw new InvalidDataException($"Файл задач пользователя {userId} поврежден или не может быть расшифрован");
        }
        catch (IOException ex)
        {
            throw new IOException($"Ошибка доступа к файлу задач: {ex.Message}");
        }

        return todoList;
    }

    private static string EscapeCsvText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        return text.Replace("\n", "\\n").Replace("\r", "");
    }

    private static string UnescapeCsvText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        return text.Replace("\\n", "\n");
    }

    private static string[] ParseCsvLine(string line)
    {
        var parts = new List<string>();
        bool inQuotes = false;
        string currentPart = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

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
        return parts.ToArray();
    }
}