using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TodoList;

public class FileManager : IDataStorage
{
    private readonly string _dataDirPath;
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public FileManager(string dataDirPath, byte[] key, byte[] iv)
    {
        _dataDirPath = dataDirPath;
        _key = key;
        _iv = iv;
        EnsureDataDirectory();
    }

    private void EnsureDataDirectory()
    {
        if (!Directory.Exists(_dataDirPath))
            Directory.CreateDirectory(_dataDirPath);
    }

    private string GetProfilesFilePath() => Path.Combine(_dataDirPath, "profiles.dat");
    private string GetTodoFilePath(Guid userId) => Path.Combine(_dataDirPath, $"todos_{userId}.dat");

    public void SaveProfiles(IEnumerable<Profile> profiles)
    {
        var filePath = GetProfilesFilePath();
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        using (var bufferedStream = new BufferedStream(fileStream))
        using (var aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;
            using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
            {
                foreach (var profile in profiles)
                {
                    writer.WriteLine($"{profile.Id};{profile.Login};{profile.Password};{profile.FirstName};{profile.LastName};{profile.BirthYear}");
                }
            }
        }
    }

    public IEnumerable<Profile> LoadProfiles()
    {
        var filePath = GetProfilesFilePath();
        if (!File.Exists(filePath))
            return new List<Profile>();

        var profiles = new List<Profile>();
        try
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var bufferedStream = new BufferedStream(fileStream))
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;
                using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;
                        var parts = line.Split(';');
                        if (parts.Length != 6)
                            continue;
                        var id = Guid.Parse(parts[0]);
                        var login = parts[1];
                        var password = parts[2];
                        var firstName = parts[3];
                        var lastName = parts[4];
                        var birthYear = int.Parse(parts[5]);
                        profiles.Add(new Profile(id, login, password, firstName, lastName, birthYear));
                    }
                }
            }
        }
        catch (CryptographicException ex)
        {
            throw new InvalidOperationException("Ошибка расшифровки файла профилей. Возможно, данные повреждены.", ex);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException("Ошибка доступа к файлу профилей.", ex);
        }
        return profiles;
    }

    public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
    {
        var filePath = GetTodoFilePath(userId);
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        using (var bufferedStream = new BufferedStream(fileStream))
        using (var aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;
            using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
            {
                int index = 0;
                foreach (var item in todos)
                {
                    var text = EscapeCsv(item.Text);
                    writer.WriteLine($"{index};{text};{item.Status};{item.LastUpdate:O}");
                    index++;
                }
            }
        }
    }

    public IEnumerable<TodoItem> LoadTodos(Guid userId)
    {
        var filePath = GetTodoFilePath(userId);
        if (!File.Exists(filePath))
            return new List<TodoItem>();

        var list = new List<TodoItem>();
        try
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var bufferedStream = new BufferedStream(fileStream))
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;
                using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;
                        var fields = ParseCsvLine(line);
                        if (fields.Length < 4)
                            continue;
                        var text = UnescapeCsv(fields[1]);
                        if (!Enum.TryParse<TodoStatus>(fields[2], true, out var status))
                            continue;
                        if (!DateTime.TryParse(fields[3], out var lastUpdate))
                            continue;
                        list.Add(new TodoItem(text, status, lastUpdate));
                    }
                }
            }
        }
        catch (CryptographicException ex)
        {
            throw new InvalidOperationException($"Ошибка расшифровки файла задач пользователя {userId}.", ex);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Ошибка доступа к файлу задач пользователя {userId}.", ex);
        }
        return list;
    }

    private static string EscapeCsv(string text)
        => "\"" + text.Replace("\"", "\"\"").Replace("\n", "\\n") + "\"";

    private static string UnescapeCsv(string text)
    {
        if (text.Length >= 2 && text[0] == '"' && text[text.Length - 1] == '"')
            text = text.Substring(1, text.Length - 2);
        return text.Replace("\\n", "\n").Replace("\"\"", "\"");
    }

    private static string[] ParseCsvLine(string line)
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
            else if (c == ';' && !inQuotes)
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
}