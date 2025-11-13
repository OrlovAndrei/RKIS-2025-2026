using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TodoList
{
    public static class FileManager
    {
        private const string DataDirectory = "Data";
        private const string ProfileFile = "profile.txt";
        private const string TodosFile = "todos.txt";

        public static int Count => LoadTodosCountOnly(Path.Combine(DataDirectory, TodosFile));

        private static int LoadTodosCountOnly(string filePath)
        {
            if (!File.Exists(filePath)) return 0;
            try
            {
                var lines = File.ReadAllLines(filePath);
                int count = 0;
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var parts = ParseCsvLine(line);
                        if (parts.Length == 3) count++; 
                    }
                }
                return count;
            }
            catch
            {
                return 0;
            }
        }

        public static void EnsureDataDirectory()
        {
            if (!Directory.Exists(DataDirectory))
            {
                Directory.CreateDirectory(DataDirectory);
            }
        }

        public static void SaveProfile(string name)
        {
            EnsureDataDirectory();
            File.WriteAllText(Path.Combine(DataDirectory, ProfileFile), name);
        }

        public static string LoadProfile()
        {
            string filePath = Path.Combine(DataDirectory, ProfileFile);
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath).Trim();
            }
            return null;
        }

        public static void SaveTodos(List<TodoItem> todos)
        {
            EnsureDataDirectory();
            var lines = new List<string>();
            foreach (var todo in todos)
            {
                lines.Add($"{EscapeCsvField(todo.Text)},{todo.Status.ToString()},{todo.LastUpdate:yyyy-MM-dd HH:mm:ss}");
            }
            File.WriteAllLines(Path.Combine(DataDirectory, TodosFile), lines);
        }

        public static List<TodoItem> LoadTodos()
        {
            string filePath = Path.Combine(DataDirectory, TodosFile);
            var todos = new List<TodoItem>();
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
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
                                todos.Add(new TodoItem(UnescapeCsvField(parts[0]), status, lastUpdate));
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            return todos;
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
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            }
            return field;
        }

        private static string UnescapeCsvField(string field)
        {
            if (field.StartsWith("\"") && field.EndsWith("\""))
            {
                field = field.Substring(1, field.Length - 2);
                return field.Replace("\"\"", "\"");
            }
            return field;
        }
    }
}