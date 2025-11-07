using System;

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
            var lines = new string[]
            {
                profile.FirstName,
                profile.LastName,
                profile.BirthYear.ToString()
            };
            File.WriteAllLines(filePath, lines);
        }

        public static Profile LoadProfile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            var lines = File.ReadAllLines(filePath);
            if (lines.Length < 3)
                return null;

            string firstName = lines[0];
            string lastName = lines[1];
            int birthYear = int.TryParse(lines[2], out int y) ? y : 2000;

            return new Profile(firstName, lastName, birthYear);
        }

        public static void SaveTodos(TodoList todoList, string filePath)
        {
            using var writer = new StreamWriter(filePath);
            foreach (var task in todoList.GetAllTasks())
            {
                writer.WriteLine($"{task.Text.Replace("\n", "\\n")};{task.IsDone};{task.LastUpdate:O}");
            }
        }

        public static TodoList LoadTodos(string filePath)
        {
            var list = new TodoList();

            if (!File.Exists(filePath))
                return list;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(';');
                if (parts.Length < 3) continue;

                string text = parts[0].Replace("\\n", "\n");
                bool done = bool.TryParse(parts[1], out bool d) && d;
                DateTime.TryParse(parts[2], out DateTime dt);

                var item = new TodoItem(text);
                if (done)
                    item.MarkDone();
                list.AddExistingTask(item, dt);
            }

            return list;
        }
    }
}
