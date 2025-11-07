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

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }

    // Profile format: three lines: FirstName, LastName, BirthYear
    public static void SaveProfile(Profile profile, string filePath)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile));
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filePath is required");

        var sb = new StringBuilder();
        sb.AppendLine(profile.FirstName ?? string.Empty);
        sb.AppendLine(profile.LastName ?? string.Empty);
        sb.AppendLine(profile.BirthYear.ToString(CultureInfo.InvariantCulture));

        File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
    }

    public static Profile LoadProfile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filePath is required");
        if (!File.Exists(filePath)) return null;

        var lines = File.ReadAllLines(filePath, Encoding.UTF8);
        if (lines.Length < 3) return null;

        string first = lines[0] ?? string.Empty;
        string last = lines[1] ?? string.Empty;
        if (!int.TryParse(lines[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out int birthYear))
            birthYear = DateTime.Now.Year;

        return new Profile(first, last, birthYear);
    }

    // CSV for todos: Index;"Text";IsDone;LastUpdate (one item per line)
    public static void SaveTodos(TodoList todos, string filePath)
    {
        if (todos == null) throw new ArgumentNullException(nameof(todos));
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filePath is required");

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

            string isDone = item.IsDone ? "true" : "false";
            string dt = item.LastUpdate == default ? string.Empty : item.LastUpdate.ToString("s", CultureInfo.InvariantCulture);

            sb.Append(i.ToString(CultureInfo.InvariantCulture));
            sb.Append(';');
            sb.Append(quoted);
            sb.Append(';');
            sb.Append(isDone);
            sb.Append(';');
            sb.AppendLine(dt);
        }

        File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
    }

    public static TodoList LoadTodos(string filePath)
    {
        var todos = new TodoList();
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filePath is required");
        if (!File.Exists(filePath)) return todos;

        var lines = File.ReadAllLines(filePath, Encoding.UTF8);
        foreach (var raw in lines)
        {
            if (string.IsNullOrWhiteSpace(raw)) continue;
            // Parse fields: Index;"Text";IsDone;LastUpdate
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

                // isDone
                sep = raw.IndexOf(';', pos);
                if (sep < 0) sep = raw.Length;
                string isDoneStr = raw.Substring(pos, sep - pos);
                pos = Math.Min(sep + 1, raw.Length);

                // lastUpdate
                string lastUpdateStr = pos < raw.Length ? raw.Substring(pos) : string.Empty;

                // Build item
                var item = new TodoItem(text);
                if (bool.TryParse(isDoneStr, out bool done))
                    item.IsDone = done;
                if (!string.IsNullOrWhiteSpace(lastUpdateStr) && DateTime.TryParse(lastUpdateStr, null, DateTimeStyles.RoundtripKind, out DateTime dt))
                    item.LastUpdate = dt;

                todos.Add(item);
            }
            catch
            {
                // ignore malformed lines
                continue;
            }
        }

        return todos;
    }
}
