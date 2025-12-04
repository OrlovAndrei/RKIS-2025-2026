using System;
using System.IO;
using System.Text;
using System.Globalization;

static class FileManager
{
    // Ensure the directory exists, create if missing
    public static void EnsureDataDirectory(string dirPath)
    {
        if (string.IsNullOrWhiteSpace(dirPath))
            throw new ArgumentException("dirPath is required");

        try
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            // Файл с профилями (profile.csv) должен существовать
            string profilePath = Path.Combine(dirPath, "profile.csv");
            if (!File.Exists(profilePath))
            {
                File.WriteAllText(profilePath, string.Empty, Encoding.UTF8);
            }
        }
        catch (Exception ex)
        {
            throw new IOException($"Не удалось создать директорию данных: {ex.Message}", ex);
        }
    }

    // --- Работа с профилями ---
    // Формат строки: Id;Login;Password;FirstName;LastName;BirthYear
    public static void SaveProfiles(List<Profile> profiles, string filePath)
    {
        if (profiles == null) throw new ArgumentNullException(nameof(profiles));
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filePath is required");

        try
        {
            var sb = new StringBuilder();
            foreach (var p in profiles)
            {
                string id = p.Id.ToString();
                string login = p.Login ?? string.Empty;
                string password = p.Password ?? string.Empty;
                string firstName = p.FirstName ?? string.Empty;
                string lastName = p.LastName ?? string.Empty;
                string birthYear = p.BirthYear.ToString(CultureInfo.InvariantCulture);

                sb.Append(id);
                sb.Append(';');
                sb.Append(login);
                sb.Append(';');
                sb.Append(password);
                sb.Append(';');
                sb.Append(firstName);
                sb.Append(';');
                sb.Append(lastName);
                sb.Append(';');
                sb.AppendLine(birthYear);
            }

            string? dirPath = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dirPath) && !Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }
        catch (Exception ex)
        {
            throw new IOException($"Не удалось сохранить список профилей: {ex.Message}", ex);
        }
    }

    public static List<Profile> LoadProfiles(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filePath is required");

        var result = new List<Profile>();

        try
        {
            if (!File.Exists(filePath))
                return result;

            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Id;Login;Password;FirstName;LastName;BirthYear
                var parts = line.Split(';');
                if (parts.Length < 6)
                    continue;

                if (!Guid.TryParse(parts[0], out Guid id))
                    continue;

                string login = parts[1];
                string password = parts[2];
                string firstName = parts[3];
                string lastName = parts[4];

                if (!int.TryParse(parts[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out int birthYear))
                    birthYear = DateTime.Now.Year;

                var profile = new Profile(id, login, password, firstName, lastName, birthYear);
                result.Add(profile);
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new IOException($"Не удалось загрузить список профилей: {ex.Message}", ex);
        }
    }

    // CSV for todos: Index;"Text";Status;LastUpdate (one item per line)
    public static void SaveTodos(TodoList todos, string filePath)
    {
        if (todos == null) throw new ArgumentNullException(nameof(todos));
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filePath is required");

        try
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= todos.Count; i++)
            {
                var item = todos.GetItem(i);
                string text = item.Text ?? string.Empty;
                // Replace newlines with literal \n
                text = text.Replace("\r\n", "\\n").Replace("\n", "\\n");
                // Escape double quotes by doubling them (CSV style)
                text = text.Replace("\"", "\"\"");
                // Wrap in quotes
                string quoted = '"' + text + '"';

                string status = item.Status.ToString();
                string dt = item.LastUpdate == default ? string.Empty : item.LastUpdate.ToString("s", CultureInfo.InvariantCulture);

                sb.Append(i.ToString(CultureInfo.InvariantCulture));
                sb.Append(';');
                sb.Append(quoted);
                sb.Append(';');
                sb.Append(status);
                sb.Append(';');
                sb.AppendLine(dt);
            }

            // Убедимся, что директория существует
            string? dirPath = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dirPath) && !Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }
        catch (Exception ex)
        {
            throw new IOException($"Не удалось сохранить список задач: {ex.Message}", ex);
        }
    }

    public static TodoList LoadTodos(string filePath)
    {
        var todos = new TodoList();
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filePath is required");
        
        try
        {
            if (!File.Exists(filePath)) return todos;

            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            foreach (var raw in lines)
            {
                if (string.IsNullOrWhiteSpace(raw)) continue;
                // Parse fields: Index;"Text";Status;LastUpdate
                try
                {
                    int pos = 0;
                    // index
                    int sep = raw.IndexOf(';', pos);
                    if (sep < 0) continue;
                    string idxStr = raw.Substring(pos, sep - pos);
                    pos = sep + 1;

                    // text (may be quoted)
                    string text = string.Empty;
                    if (pos < raw.Length && raw[pos] == '"')
                    {
                        pos++; // skip opening quote
                        var sb = new StringBuilder();
                        while (pos < raw.Length)
                        {
                            if (raw[pos] == '"')
                            {
                                // If next is also a quote, it's an escaped quote
                                if (pos + 1 < raw.Length && raw[pos + 1] == '"')
                                {
                                    sb.Append('"');
                                    pos += 2;
                                    continue;
                                }
                                // otherwise end of quoted field
                                pos++;
                                break;
                            }
                            sb.Append(raw[pos]);
                            pos++;
                        }
                        text = sb.ToString();
                        // unescape \n -> newline
                        text = text.Replace("\\n", "\n");
                    }
                    // skip separator after text if present
                    if (pos < raw.Length && raw[pos] == ';') pos++;

                    // status (can be enum string or legacy boolean)
                    sep = raw.IndexOf(';', pos);
                    if (sep < 0) sep = raw.Length;
                    string statusStr = raw.Substring(pos, sep - pos);
                    pos = Math.Min(sep + 1, raw.Length);

                    // lastUpdate
                    string lastUpdateStr = pos < raw.Length ? raw.Substring(pos) : string.Empty;

                    // Build item
                    var item = new TodoItem(text);
                    
                    // Try to parse as enum first, then fall back to boolean for backward compatibility
                    if (Enum.TryParse<TodoStatus>(statusStr, true, out TodoStatus status))
                    {
                        item.Status = status;
                    }
                    else if (bool.TryParse(statusStr, out bool done))
                    {
                        // Legacy format: convert boolean to enum
                        item.Status = done ? TodoStatus.Completed : TodoStatus.NotStarted;
                    }
                    
                    if (!string.IsNullOrWhiteSpace(lastUpdateStr) && DateTime.TryParse(lastUpdateStr, null, DateTimeStyles.RoundtripKind, out DateTime dt))
                        item.LastUpdate = dt;

                    todos.Add(item);
                }
                catch (Exception lineEx)
                {
                    // Log error but continue processing other lines
                    Console.WriteLine($"Предупреждение: не удалось прочитать строку '{raw}': {lineEx.Message}");
                    continue;
                }
            }

            return todos;
        }
        catch (Exception ex)
        {
            throw new IOException($"Не удалось загрузить список задач: {ex.Message}", ex);
        }
    }
}
