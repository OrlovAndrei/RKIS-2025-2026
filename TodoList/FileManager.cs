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
        /// Сохраняет все профили в profile.csv.
        /// </summary>
        /// <param name="profiles">Список профилей</param>
        /// <param name="filePath">Путь к файлу</param>
        public static void SaveProfiles(List<Profile> profiles, string filePath)
        {
            if (profiles == null)
                throw new ArgumentNullException(nameof(profiles));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым.", nameof(filePath));

            var lines = new StringBuilder();
            lines.AppendLine("Id;Login;Password;FirstName;LastName;BirthYear");

            foreach (var profile in profiles)
            {
                string escapedLogin = EscapeCsv(profile.Login);
                string escapedPassword = EscapeCsv(profile.Password);
                string escapedFirstName = EscapeCsv(profile.FirstName);
                string escapedLastName = EscapeCsv(profile.LastName);
                lines.AppendLine($"{profile.Id};{escapedLogin};{escapedPassword};{escapedFirstName};{escapedLastName};{profile.BirthYear}");
            }

            File.WriteAllText(filePath, lines.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// Загружает все профили из profile.csv.
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Список профилей</returns>
        public static List<Profile> LoadProfiles(string filePath)
        {
            var profiles = new List<Profile>();
            
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым.", nameof(filePath));

            if (!File.Exists(filePath))
                return profiles;

            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            if (lines.Length < 2)
                return profiles; // Только заголовок или пустой файл

            // Пропускаем заголовок (первую строку)
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = ParseCsvLine(line);
                if (parts.Length < 6)
                    continue;

                if (!Guid.TryParse(parts[0], out Guid id))
                    continue;

                string login = UnescapeCsv(parts[1]);
                string password = UnescapeCsv(parts[2]);
                string firstName = UnescapeCsv(parts[3]);
                string lastName = UnescapeCsv(parts[4]);
                if (!int.TryParse(parts[5], out int birthYear))
                    continue;

                var profile = new Profile(id, login, password, firstName, lastName, birthYear);
                profiles.Add(profile);
            }

            return profiles;
        }

        /// <summary>
        /// Сохраняет задачи в CSV-файл todos_<Id>.csv.
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
            lines.AppendLine("Index;Text;Status;LastUpdate");

            for (int i = 0; i < todos.Count; i++)
            {
                var item = todos.GetItem(i);
                string escapedText = EscapeCsv(item.Text);
                string statusStr = item.Status.ToString();
                string dateStr = item.LastUpdate.ToString("yyyy-MM-ddTHH:mm:ss");

                lines.AppendLine($"{i};{escapedText};{statusStr};{dateStr}");
            }

            File.WriteAllText(filePath, lines.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// Загружает задачи из CSV-файла todos_<Id>.csv.
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
                TodoStatus status = Enum.Parse<TodoStatus>(parts[2], ignoreCase: true);
                DateTime lastUpdate = DateTime.Parse(parts[3]);

                var item = new TodoItem(text, status, lastUpdate);
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


