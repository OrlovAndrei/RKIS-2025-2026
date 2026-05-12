namespace TodoList
{
    public static class FileManager
    {
        public static void EnsureDataDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        public static void SaveProfile(Profile profile, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(profile.FirstName);
                writer.WriteLine(profile.LastName);
                writer.WriteLine(profile.BirthYear);
            }
        }

        public static Profile LoadProfile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < 3)
                return null;

            Profile profile = new Profile();
            profile.FirstName = lines[0];
            profile.LastName = lines[1];
            
            int birthYear;
            if (int.TryParse(lines[2], out birthYear))
            {
                profile.BirthYear = birthYear;
            }

            return profile;
        }

        public static void SaveTodos(TodoList todos, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Index;Text;IsDone;LastUpdate");
                
                for (int i = 0; i < todos.GetItemCount(); i++)
                {
                    TodoItem item = todos.GetItem(i);
                    if (item != null)
                    {
                        string text = item.Text.Replace("\\", "\\\\").Replace("\"", "\"\"").Replace("\n", "\\n").Replace("\r", "\\r");
                        writer.WriteLine($"{i};\"{text}\";{item.IsDone};{item.LastUpdate:yyyy-MM-ddTHH:mm:ss}");
                    }
                }
            }
        }

        public static TodoList LoadTodos(string filePath)
        {
            TodoList todoList = new TodoList();

            if (!File.Exists(filePath))
                return todoList;

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                string[] parts = lines[i].Split(';');
                if (parts.Length < 4)
                    continue;

                try
                {
                    int index = int.Parse(parts[0]);
                    string text = parts[1].Trim('"').Replace("\"\"", "\"").Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\\\", "\\");
                    bool isDone = bool.Parse(parts[2]);
                    DateTime lastUpdate = DateTime.Parse(parts[3]);

                    TodoItem item = new TodoItem(text);
                    if (isDone)
                    {
                        item.MarkDone();
                    }
                    
                    todoList.Add(item);
                }
                catch
                {
                    continue;
                }
            }

            return todoList;
        }
    }
}