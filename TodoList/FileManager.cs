using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TodoList
{
    public static class FileManager
    {
        public static void EnsureDataDirectory(string dataDir)
        {
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
        }

        public static void SaveProfile(Profile profile, string path)
        {
            string data = $"{profile.FirstName},{profile.LastName},{profile.BirthYear}";
            File.WriteAllText(path, data);
        }

        public static Profile LoadProfile(string path)
        {
            if (File.Exists(path))
            {
                string data = File.ReadAllText(path).Trim();
                var parts = data.Split(',');
                if (parts.Length == 3 &&
                    int.TryParse(parts[2], out int birthYear))
                {
                    return new Profile(parts[0], parts[1], birthYear);
                }
            }
            return null;
        }

        public static void SaveTodos(TodoList todoList, string path)
        {
            var lines = new List<string>();
            foreach (var todo in todoList)  
            {
                lines.Add($"{EscapeCsvField(todo.Text)},{todo.Status.ToString()},{todo.LastUpdate:yyyy-MM-dd HH:mm:ss}");
            }
            File.WriteAllLines(path, lines);
        }

        public static TodoList LoadTodos(string path)
        {
            var todoList = new TodoList();
            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var parts = ParseCsvLine(line);
                        if (parts.Length == 3 &&
                            DateTime.TryParse(parts[2], out DateTime lastUpdate))
                        {
                            try
                            {
                                var status = Enum.Parse<TodoStatus>(parts[1]);
                                todoList.Add(new TodoItem(UnescapeCsvField(parts[0]), status, lastUpdate));
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            return todoList;
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
                else if (c == ',' && !inQuotes)
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
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                string temp = field.Replace("\n", "|NL|").Replace("\r", "|CR|");
                temp = temp.Replace("\"", "\"\"");
                return "\"" + temp + "\"";
            }
            return field;
        }

        private static string UnescapeCsvField(string field)
        {
            field = field.Replace("|NL|", "\n").Replace("|CR|", "\r");
            field = field.Replace("\"\"", "\"");
            return field;
        }
    }
}