using System;
using System.IO;
using ToDoList;

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
            string data = $"{profile.FirstName};{profile.LastName};{profile.BirthYear}";
            File.WriteAllText(filePath, data);
        }

        public static Profile LoadProfile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            string content = File.ReadAllText(filePath);

            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            string[] parts = content.Split(';');

            if (parts.Length < 3)
            {
                return null;
            }

            string firstName = parts[0];
            string lastName = parts[1];
            int birthYear = int.Parse(parts[2]);
            return new Profile(firstName, lastName, birthYear);
        }


        public static void SaveTodos(TodoList todos, string filePath)
        {
            string[] lines = new string[todos.Count];

            for (int i = 0; i < todos.Count; i++)
            {
				var todo = todos[i];
				string text = EscapeCsv(todo.Text);
				string line = $"{i};{text};{todo.Status};{todo.LastUpdate:yyyy-MM-ddTHH:mm:ss}";
				lines[i] = line;
            }
            File.WriteAllLines(filePath, lines);
        }

        public static TodoList LoadTodos(string filePath)
        {
            var todoList = new TodoList();

            if (!File.Exists(filePath))
            {
                return todoList;
            }

            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] parts = line.Split(';');

                string text = UnescapeCsv(parts[1]);

				if (string.IsNullOrEmpty(text))
				{
					continue;
				}
				TodoStatus status = (TodoStatus)Enum.Parse(typeof(TodoStatus), parts[2]);
				DateTime lastUpdate = DateTime.Parse(parts[3]);


				var todo = new TodoItem(text);
				todo.SetStatus(status);
				todoList.AddTodoFromFile(todo);
			}
            return todoList;
        }

        private static string EscapeCsv(string text)
        {
            if (text == null) return "\"\"";
            return "\"" + text.Replace("\"", "\"\"").Replace("\n", "\\n") + "\"";
        }

        private static string UnescapeCsv(string text)
        {
            if (text == null) return "";
            return text.Trim('"').Replace("\\n", "\n").Replace("\"\"", "\"");
        }
    }
}