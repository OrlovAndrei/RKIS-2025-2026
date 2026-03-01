using System;
using System.IO;
using System.Globalization;

namespace Todolist
{
    public static class FileManager
    {
        public static void EnsureDataDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                Console.WriteLine($"Создана директория: {dirPath}");
            }
        }

        public static void SaveProfile(Profile profile, string filePath)
        {
            try
            {
                string content = $"{profile.FirstName};{profile.LastName};{profile.BirthYear}";
                File.WriteAllText(filePath, content);
                Console.WriteLine($"Профиль сохранен: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения профиля: {ex.Message}");
            }
        }

        public static Profile LoadProfile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return null;

                string content = File.ReadAllText(filePath);
                string[] parts = content.Split(';');

                if (parts.Length == 3)
                {
                    string firstName = parts[0];
                    string lastName = parts[1];
                    int birthYear = int.Parse(parts[2]);

                    return new Profile(firstName, lastName, birthYear);
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
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Index;Text;Status;LastUpdate");

                    int counter = 1;
                    foreach (var item in todos.GetItems())
                    {
                        string escapedText = EscapeCsvText(item.Text);
                        string status = item.Status.ToString();
                        string date = item.LastUpdate.ToString("yyyy-MM-ddTHH:mm:ss");

                        writer.WriteLine($"{counter};\"{escapedText}\";{status};{date}");
                        counter++;
                    }
                }
                Console.WriteLine($"Задачи сохранены: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения задач: {ex.Message}");
            }
        }

        public static TodoList LoadTodos(string filePath)
        {
            TodoList todoList = new TodoList();

            try
            {
                if (!File.Exists(filePath))
                    return todoList;

                string[] lines = File.ReadAllLines(filePath);

                for (int i = 1; i < lines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i]))
                        continue;

                    string[] parts = ParseCsvLine(lines[i]);

                    if (parts.Length >= 4)
                    {
                        string text = UnescapeCsvText(parts[1]);
                        TodoStatus status = (TodoStatus)Enum.Parse(typeof(TodoStatus), parts[2]);
                        DateTime lastUpdate = DateTime.ParseExact(parts[3], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

                        TodoItem item = new TodoItem(text);
                        item.SetStatus(status);
                        SetPrivateField(item, "_lastUpdate", lastUpdate);

                        todoList.Add(item);
                    }
                }

                Console.WriteLine($"Загружено задач: {todoList.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки задач: {ex.Message}");
            }

            return todoList;
        }

        private static string EscapeCsvText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            return text.Replace("\n", "\\n").Replace("\r", "");
        }

        private static string UnescapeCsvText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            return text.Replace("\\n", "\n");
        }

        private static string[] ParseCsvLine(string line)
        {
            var parts = new System.Collections.Generic.List<string>();
            bool inQuotes = false;
            string currentPart = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

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
            return parts.ToArray();
        }

        private static void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(obj, value);
        }
    }
}
