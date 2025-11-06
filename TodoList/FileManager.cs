using System.Collections.Generic;
using System.IO;

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
            try
            {
                var lines = new string[]
                {
                    profile.FirstName,
                    profile.LastName,
                    profile.BirthYear.ToString()
                };
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения профиля: {ex.Message}");
            }
        }

        public static Profile LoadProfile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            try
            {
                var lines = File.ReadAllLines(filePath);
                if (lines.Length == 3)
                {
                    string firstName = lines[0];
                    string lastName = lines[1];
                    if (int.TryParse(lines[2], out int birthYear))
                    {
                        return new Profile(firstName, lastName, birthYear);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки профиля: {ex.Message}");
            }

            return null;
        }

        public static void SaveTodos(TodoList todos, string filePath)
        {
            try
            {
                var lines = new List<string>();
                
                for (int i = 1; i <= GetTodoCount(todos); i++)
                {
                    TodoItem task = todos.GetItem(i);
                    if (task != null)
                    {
                        string escapedText = EscapeCsv(task.Text);
                        string line = $"{i};{escapedText};{task.IsDone};{task.LastUpdate:O}";
                        lines.Add(line);
                    }
                }

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения задач: {ex.Message}");
            }
        }

        public static TodoList LoadTodos(string filePath)
        {
            TodoList todoList = new TodoList();

            if (!File.Exists(filePath))
                return todoList;

            try
            {
                var lines = File.ReadAllLines(filePath);
                int loadedCount = 0;

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var parts = line.Split(';');
                    if (parts.Length == 4)
                    {
                        try
                        {
                            string text = UnescapeCsv(parts[1]);
                            bool isDone = bool.Parse(parts[2]);
                            DateTime lastUpdate = DateTime.Parse(parts[3]);

                            TodoItem todoItem = new TodoItem(text);

                            if (isDone)
                            {
                                todoItem.MarkDone();
                            }

                            todoList.Add(todoItem);
                            loadedCount++;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }

                if (loadedCount > 0)
                {
                    Console.WriteLine($"Загружено задач: {loadedCount}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки задач: {ex.Message}");
            }

            return todoList;
        }
          
        private static int GetTodoCount(TodoList todos)
        {
            int count = 0;
            while (true)
            {
                TodoItem item = todos.GetItem(count + 1);
                if (item == null)
                    break;
                count++;
            }
            return count;
        }

        private static string EscapeCsv(string text)
        {
            return "\"" + text.Replace("\"", "\"\"").Replace("\n", "\\n") + "\"";
        }

        private static string UnescapeCsv(string text)
        {
            return text.Trim('"').Replace("\\n", "\n").Replace("\"\"", "\"");
        }
    }
}