using System;
using System.IO;
using System.Text;

namespace TodoList
{
    /// <summary>
    /// Статический класс для работы с файлами и папками.
    /// </summary>
    internal static class FileManager
    {
        /// <summary>
        /// Проверяет существование папки, если её нет — создаёт её.
        /// </summary>
        /// <param name="dirPath">Путь к папке</param>
        public static void EnsureDataDirectory(string dirPath)
        {
            if (string.IsNullOrWhiteSpace(dirPath))
                throw new ArgumentException("Путь к папке не может быть пустым.", nameof(dirPath));

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        /// <summary>
        /// Сохраняет данные пользователя в profile.txt.
        /// </summary>
        /// <param name="profile">Профиль пользователя</param>
        /// <param name="filePath">Путь к файлу</param>
        public static void SaveProfile(Profile profile, string filePath)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым.", nameof(filePath));

            var lines = new StringBuilder();
            lines.AppendLine(profile.FirstName);
            lines.AppendLine(profile.LastName);
            lines.AppendLine(profile.BirthYear.ToString());

            File.WriteAllText(filePath, lines.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// Загружает данные пользователя из profile.txt.
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Объект Profile</returns>
        public static Profile LoadProfile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым.", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл профиля не найден.", filePath);

            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            if (lines.Length < 3)
                throw new InvalidDataException("Неверный формат файла профиля.");

            string firstName = lines[0].Trim();
            string lastName = lines[1].Trim();
            if (!int.TryParse(lines[2].Trim(), out int birthYear))
                throw new InvalidDataException("Неверный формат года рождения в файле профиля.");

            return new Profile(firstName, lastName, birthYear);
        }

        /// <summary>
        /// Сохраняет задачи в CSV-файл todo.csv.
        /// </summary>
        /// <param name="todos">Список задач</param>
        /// <param name="filePath">Путь к файлу</param>
        public static void SaveTodos(TodoList todos, string filePath)
        {
            if (todos == null)
                throw new ArgumentNullException(nameof(todos));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым.", nameof(filePath));

            var lines = new StringBuilder();
            lines.AppendLine("Index;Text;IsDone;LastUpdate");

            for (int i = 0; i < todos.Count; i++)
            {
                var item = todos.GetItem(i);
                string escapedText = EscapeCsv(item.Text);
                string isDoneStr = item.IsDone ? "true" : "false";
                string dateStr = item.LastUpdate.ToString("yyyy-MM-ddTHH:mm:ss");

                lines.AppendLine($"{i};{escapedText};{isDoneStr};{dateStr}");
            }

            File.WriteAllText(filePath, lines.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// Загружает задачи из CSV-файла.
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Объект TodoList</returns>
        public static TodoList LoadTodos(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым.", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл задач не найден.", filePath);

            var todoList = new TodoList();
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

            if (lines.Length < 2)
                return todoList; // Только заголовок или пустой файл

            // Пропускаем заголовок (первую строку)
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = ParseCsvLine(line);
                if (parts.Length < 4)
                    continue;

                int index = int.Parse(parts[0]);
                string text = UnescapeCsv(parts[1]);
                bool isDone = parts[2].ToLowerInvariant() == "true";
                DateTime lastUpdate = DateTime.Parse(parts[3]);

                var item = new TodoItem(text, isDone, lastUpdate);
                todoList.Add(item);
            }

            return todoList;
        }

        /// <summary>
        /// Экранирует текст для CSV формата.
        /// </summary>
        private static string EscapeCsv(string text)
        {
            if (text == null)
                return "\"\"";

            string escaped = text.Replace("\"", "\"\"").Replace("\n", "\\n");
            return "\"" + escaped + "\"";
        }

        /// <summary>
        /// Раскодирует текст из CSV формата.
        /// </summary>
        private static string UnescapeCsv(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            string unescaped = text.Trim('"');
            unescaped = unescaped.Replace("\\n", "\n").Replace("\"\"", "\"");
            return unescaped;
        }

        /// <summary>
        /// Парсит строку CSV, учитывая кавычки.
        /// </summary>
        private static string[] ParseCsvLine(string line)
        {
            var parts = new System.Collections.Generic.List<string>();
            bool inQuotes = false;
            var current = new StringBuilder();

            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ';' && !inQuotes)
                {
                    parts.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            if (current.Length > 0 || line.EndsWith(";"))
            {
                parts.Add(current.ToString());
            }

            return parts.ToArray();
        }
    }
}

