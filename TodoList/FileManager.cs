using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TodoList
{
    public static class FileManager
    {
        private static readonly string ProfilesFileName = "profile.csv";
        private static readonly string TodosFolderName = "todos";

        public static void EnsureDataDirectory(string dataDir)
        {
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            string todosPath = Path.Combine(dataDir, TodosFolderName);
            if (!Directory.Exists(todosPath))
            {
                Directory.CreateDirectory(todosPath);
            }
        }

        public static string GetProfilesFilePath(string dataDir) => Path.Combine(dataDir, ProfilesFileName);

        public static string GetUserTodosFilePath(string dataDir, Guid userId)
            => Path.Combine(dataDir, TodosFolderName, $"todos_{userId}.csv");

        public static void SaveProfiles(List<Profile> profiles, string dataDir)
        {
            var lines = new List<string>();
            foreach (var profile in profiles)
            {
                lines.Add($"{profile.Id};{profile.Login};{profile.Password};{profile.FirstName};{profile.LastName};{profile.BirthYear}");
            }
            File.WriteAllLines(GetProfilesFilePath(dataDir), lines);
        }

        public static List<Profile> LoadProfiles(string dataDir)
        {
            var profiles = new List<Profile>();
            string filePath = GetProfilesFilePath(dataDir);

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var parts = line.Split(';');
                        if (parts.Length == 6 &&
                            Guid.TryParse(parts[0], out Guid id) &&
                            int.TryParse(parts[5], out int birthYear))
                        {
                            profiles.Add(new Profile(id, parts[1], parts[2], parts[3], parts[4], birthYear));
                        }
                    }
                }
            }
            return profiles;
        }

        public static void SaveTodos(Guid userId, TodoList todoList, string dataDir)
        {
            var lines = new List<string>();
            foreach (var todo in todoList)
            {
                lines.Add($"{EscapeCsvField(todo.Text)};{todo.Status};{todo.LastUpdate:yyyy-MM-dd HH:mm:ss}");
            }
            File.WriteAllLines(GetUserTodosFilePath(dataDir, userId), lines);
        }

        public static TodoList LoadTodos(Guid userId, string dataDir)
        {
            var todoList = new TodoList(new List<TodoItem>());
            string filePath = GetUserTodosFilePath(dataDir, userId);

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var parts = ParseCsvLine(line, ';');
                        if (parts.Length == 3 &&
                            DateTime.TryParse(parts[2], out DateTime lastUpdate))
                        {
                            try
                            {
                                var status = Enum.Parse<TodoStatus>(parts[1]);
                                todoList.Add(new TodoItem(UnescapeCsvField(parts[0]), status, lastUpdate));
                            }
                            catch { }
                        }
                    }
                }
            }
            return todoList;
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