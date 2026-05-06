using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Todolist.Exceptions;

class FileManager : IDataStorage
{
    private readonly string _dataDir;
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public FileManager(string dataDir)
        : this(dataDir, StorageCryptoConfig.Key, StorageCryptoConfig.Iv)
    {
    }

    public FileManager(string dataDir, byte[] key, byte[] iv)
    {
        if (string.IsNullOrWhiteSpace(dataDir))
            throw new ArgumentException("Путь к каталогу данных обязателен.", nameof(dataDir));
        if (key == null || key.Length == 0)
            throw new ArgumentException("Ключ шифрования обязателен.", nameof(key));
        if (iv == null || iv.Length == 0)
            throw new ArgumentException("IV обязателен.", nameof(iv));

        _dataDir = dataDir;
        _key = (byte[])key.Clone();
        _iv = (byte[])iv.Clone();

        EnsureDataDirectory();
    }

    public void SaveProfiles(IEnumerable<Profile> profiles)
    {
        if (profiles == null) throw new ArgumentNullException(nameof(profiles));

        string path = GetProfilesPath();
        try
        {
            using FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            using BufferedStream buffered = new BufferedStream(file);
            using Aes aes = CreateAes();
            using ICryptoTransform encryptor = aes.CreateEncryptor();
            using CryptoStream crypto = new CryptoStream(buffered, encryptor, CryptoStreamMode.Write);
            using StreamWriter writer = new StreamWriter(crypto, Encoding.UTF8);

            foreach (Profile p in profiles)
            {
                writer.Write(p.Id.ToString());
                writer.Write(';');
                writer.Write(p.Login ?? string.Empty);
                writer.Write(';');
                writer.Write(p.Password ?? string.Empty);
                writer.Write(';');
                writer.Write(p.FirstName ?? string.Empty);
                writer.Write(';');
                writer.Write(p.LastName ?? string.Empty);
                writer.Write(';');
                writer.WriteLine(p.BirthYear.ToString(CultureInfo.InvariantCulture));
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new StorageException($"Нет доступа к файлу профилей: {path}", ex);
        }
        catch (IOException ex)
        {
            throw new StorageException($"Ошибка записи файла профилей: {path}", ex);
        }
        catch (CryptographicException ex)
        {
            throw new StorageException("Ошибка шифрования профилей.", ex);
        }
    }

    public IEnumerable<Profile> LoadProfiles()
    {
        string path = GetProfilesPath();
        var result = new List<Profile>();

        if (!File.Exists(path))
            return result;

        try
        {
            using FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using BufferedStream buffered = new BufferedStream(file);
            using Aes aes = CreateAes();
            using ICryptoTransform decryptor = aes.CreateDecryptor();
            using CryptoStream crypto = new CryptoStream(buffered, decryptor, CryptoStreamMode.Read);
            using StreamReader reader = new StreamReader(crypto, Encoding.UTF8);

            string? line;
            int lineNumber = 0;
            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(';');
                if (parts.Length < 6)
                    throw new StorageException($"Файл профилей поврежден: некорректная строка {lineNumber}.");
                if (!Guid.TryParse(parts[0], out Guid id))
                    throw new StorageException($"Файл профилей поврежден: некорректный Guid в строке {lineNumber}.");
                if (!int.TryParse(parts[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out int birthYear))
                    throw new StorageException($"Файл профилей поврежден: некорректный год рождения в строке {lineNumber}.");

                result.Add(new Profile(id, parts[1], parts[2], parts[3], parts[4], birthYear));
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new StorageException($"Нет доступа к файлу профилей: {path}", ex);
        }
        catch (IOException ex)
        {
            throw new StorageException($"Ошибка чтения файла профилей: {path}", ex);
        }
        catch (CryptographicException ex)
        {
            throw new StorageException("Не удалось расшифровать файл профилей. Проверьте ключ/IV или целостность файла.", ex);
        }

        return result;
    }

    public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
    {
        if (userId == Guid.Empty) throw new ArgumentException("Некорректный userId.", nameof(userId));
        if (todos == null) throw new ArgumentNullException(nameof(todos));

        string path = GetTodosPath(userId);
        try
        {
            using FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            using BufferedStream buffered = new BufferedStream(file);
            using Aes aes = CreateAes();
            using ICryptoTransform encryptor = aes.CreateEncryptor();
            using CryptoStream crypto = new CryptoStream(buffered, encryptor, CryptoStreamMode.Write);
            using StreamWriter writer = new StreamWriter(crypto, Encoding.UTF8);

            foreach (TodoItem item in todos)
            {
                string text = EscapeText(item.Text ?? string.Empty);
                string status = item.Status.ToString();
                string dt = item.LastUpdate == default
                    ? string.Empty
                    : item.LastUpdate.ToString("s", CultureInfo.InvariantCulture);

                writer.Write(text);
                writer.Write(';');
                writer.Write(status);
                writer.Write(';');
                writer.WriteLine(dt);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new StorageException($"Нет доступа к файлу задач пользователя {userId}.", ex);
        }
        catch (IOException ex)
        {
            throw new StorageException($"Ошибка записи задач пользователя {userId}.", ex);
        }
        catch (CryptographicException ex)
        {
            throw new StorageException("Ошибка шифрования задач.", ex);
        }
    }

    public IEnumerable<TodoItem> LoadTodos(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("Некорректный userId.", nameof(userId));

        string path = GetTodosPath(userId);
        var result = new List<TodoItem>();

        if (!File.Exists(path))
            return result;

        try
        {
            using FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using BufferedStream buffered = new BufferedStream(file);
            using Aes aes = CreateAes();
            using ICryptoTransform decryptor = aes.CreateDecryptor();
            using CryptoStream crypto = new CryptoStream(buffered, decryptor, CryptoStreamMode.Read);
            using StreamReader reader = new StreamReader(crypto, Encoding.UTF8);

            string? line;
            int lineNumber = 0;
            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                List<string> parts = SplitEscaped(line);
                if (parts.Count < 3)
                    throw new StorageException($"Файл задач поврежден: некорректная строка {lineNumber}.");

                string text = UnescapeText(parts[0]);
                string statusStr = parts[1];
                string lastUpdateStr = parts[2];

                var item = new TodoItem(text);

                if (Enum.TryParse(statusStr, true, out TodoStatus status))
                {
                    item.Status = status;
                }
                else if (bool.TryParse(statusStr, out bool done))
                {
                    item.Status = done ? TodoStatus.Completed : TodoStatus.NotStarted;
                }
                else
                {
                    throw new StorageException($"Файл задач поврежден: некорректный статус в строке {lineNumber}.");
                }

                if (!string.IsNullOrWhiteSpace(lastUpdateStr))
                {
                    if (!DateTime.TryParse(lastUpdateStr, null, DateTimeStyles.RoundtripKind, out DateTime dt))
                        throw new StorageException($"Файл задач поврежден: некорректная дата в строке {lineNumber}.");
                    item.LastUpdate = dt;
                }

                result.Add(item);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new StorageException($"Нет доступа к файлу задач пользователя {userId}.", ex);
        }
        catch (IOException ex)
        {
            throw new StorageException($"Ошибка чтения задач пользователя {userId}.", ex);
        }
        catch (CryptographicException ex)
        {
            throw new StorageException("Не удалось расшифровать файл задач. Проверьте ключ/IV или целостность файла.", ex);
        }

        return result;
    }

    private static string EscapeText(string text)
    {
        return text
            .Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("\r\n", "\\n", StringComparison.Ordinal)
            .Replace("\n", "\\n", StringComparison.Ordinal)
            .Replace(";", "\\;", StringComparison.Ordinal);
    }

    private static string UnescapeText(string text)
    {
        var sb = new StringBuilder();
        bool escaped = false;
        for (int i = 0; i < text.Length; i++)
        {
            char ch = text[i];
            if (escaped)
            {
                switch (ch)
                {
                    case 'n':
                        sb.Append('\n');
                        break;
                    case ';':
                        sb.Append(';');
                        break;
                    case '\\':
                        sb.Append('\\');
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
                escaped = false;
            }
            else if (ch == '\\')
            {
                escaped = true;
            }
            else
            {
                sb.Append(ch);
            }
        }

        if (escaped)
            sb.Append('\\');

        return sb.ToString();
    }

    private static List<string> SplitEscaped(string line)
    {
        var parts = new List<string>();
        var current = new StringBuilder();
        bool escaped = false;

        for (int i = 0; i < line.Length; i++)
        {
            char ch = line[i];
            if (escaped)
            {
                current.Append('\\');
                current.Append(ch);
                escaped = false;
                continue;
            }

            if (ch == '\\')
            {
                escaped = true;
                continue;
            }

            if (ch == ';')
            {
                parts.Add(current.ToString());
                current.Clear();
                continue;
            }

            current.Append(ch);
        }

        if (escaped)
            current.Append('\\');

        parts.Add(current.ToString());
        return parts;
    }

    private void EnsureDataDirectory()
    {
        try
        {
            if (!Directory.Exists(_dataDir))
            {
                Directory.CreateDirectory(_dataDir);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new StorageException($"Нет доступа к каталогу данных: {_dataDir}", ex);
        }
        catch (IOException ex)
        {
            throw new StorageException($"Ошибка подготовки каталога данных: {_dataDir}", ex);
        }
    }

    private string GetProfilesPath() => Path.Combine(_dataDir, "profile.dat");

    private string GetTodosPath(Guid userId) => Path.Combine(_dataDir, $"todos_{userId}.dat");

    private Aes CreateAes()
    {
        Aes aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        return aes;
    }
}
