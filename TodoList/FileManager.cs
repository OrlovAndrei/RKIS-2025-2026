using System;
using System.Globalization;

namespace TodoList
{
    public static class FileManager
    {
        public static void EnsureDataDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
        }

        public static void SaveProfile(Profile profile, string filePath)
        {
            if (profile == null) return;

            string[] lines =
            {
                profile.FirstName,
                profile.LastName,
                profile.BirthYear.ToString()
            };

            File.WriteAllLines(filePath, lines);
        }

        public static Profile? LoadProfile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < 3)
                return null;

            string firstName = lines[0];
            string lastName = lines[1];
            if (!int.TryParse(lines[2], out int birthYear))
                return null;

            return new Profile(firstName, lastName, birthYear);
        }

        public static void SaveTodos(TodoList todos, string filePath)
        {
            if (todos == null) return;

            using var writer = new StreamWriter(filePath);
            var tasks = todos.GetAllTasks();

            writer.WriteLine("Index;Text;IsDone;LastUpdate");

            for (int i = 0; i < tasks.Count; i++)
            {
                var t = tasks[i];
                string textEscaped = t.Text.Replace("\n", "\\n").Replace("\"", "\"\"");
                writer.WriteLine($"{i};\"{textEscaped}\";{t.IsDone.ToString().ToLowerInvariant()};{t.LastUpdate:O}");
            }
        }

        public static TodoList LoadTodos(string filePath)
        {
            var todoList = new TodoList();

            if (!File.Exists(filePath))
                return todoList;

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length <= 1)
                return todoList;

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = new List<string>();
                bool inQuotes = false;
                string currentPart = "";

                foreach (char c in line)
                {
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

                if (parts.Count < 4)
                    continue;

                string textRaw = parts[1].Trim('"').Replace("\\n", "\n").Replace("\"\"", "\"");
                bool isDone = bool.TryParse(parts[2], out bool done) && done;
                DateTime.TryParse(parts[3], null, DateTimeStyles.RoundtripKind, out DateTime date);

                var item = new TodoItem(textRaw, isDone, date);
                todoList.tasks.Add(item);
            }

            return todoList;
        }
    }
}