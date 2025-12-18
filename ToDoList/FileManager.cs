using System;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MyTodoApp
{
    public static class StorageHelper
    {
        private const char Delimiter = ';';
        private const string NewLineToken = "\\n";

        public static void EnsureFolderExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }

        public static void SaveUserProfile(UserProfile user, string path)
        {
            try
            {
                string json = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
                Console.WriteLine($"Профиль сохранён в файле: {path}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка при сохранении профиля: {e.Message}");
            }
        }

        public static UserProfile LoadUserProfile(string path)
        {
            if (!File.Exists(path)) return null;

            try
            {
                string json = File.ReadAllText(path);
                var user = JsonSerializer.Deserialize<UserProfile>(json);
                Console.WriteLine($"Профиль загружен из файла: {path}");
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка при загрузке профиля, файл пропущен: {e.Message}");
                return null;
            }
        }

        public static void SaveTaskList(TaskCollection tasks, string path)
        {
            try
            {
                var lines = tasks.GetAllTasks()
                    .Select(t =>
                    {
                        string txt = EscapeForCsv(t.Description);
                        string done = t.IsCompleted.ToString();
                        string updated = t.LastModified.ToString("yyyy-MM-dd HH:mm:ss");
                        return $"\"{txt}\"{Delimiter}{done}{Delimiter}{updated}";
                    })
                    .ToList();

                lines.Insert(0, $"Description{Delimiter}IsCompleted{Delimiter}LastModified");

                File.WriteAllLines(path, lines);
                Console.WriteLine($"Список задач сохранён: {path}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка при сохранении задач: {e.Message}");
            }
        }

        public static TaskCollection LoadTaskList(string path)
        {
            var taskCollection = new TaskCollection();

            if (!File.Exists(path))
                return taskCollection;

            try
            {
                var lines = File.ReadAllLines(path);

                foreach (var line in lines.Skip(1))
                {
                    string[] parts = ParseCsvLine(line);

                    if (parts.Length != 3)
                    {
                        Console.WriteLine($"Пропущена некорректная строка CSV: {line}");
                        continue;
                    }

                    string desc = UnescapeFromCsv(parts[0]);
                    bool completed = bool.TryParse(parts[1], out bool val) && val;
                    DateTime lastMod = DateTime.TryParseExact(parts[2], "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime dt)
                        ? dt
                        : DateTime.Now;

                    var newTask = new TaskItem(desc) { LastModified = lastMod };
                    if (completed)
                        newTask.MarkComplete();

                    taskCollection.AddTask(newTask);
                }

                Console.WriteLine($"Задачи загружены из файла: {path}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка при загрузке задач, список пуст: {e.Message}");
                return new TaskCollection();
            }

            return taskCollection;
        }

        private static string EscapeForCsv(string input)
        {
            if (input == null) return string.Empty;

            string escaped = input.Replace("\n", NewLineToken);
            escaped = escaped.Replace("\"", "\"\"");

            return escaped;
        }

        private static string UnescapeFromCsv(string input)
        {
            if (input == null) return string.Empty;

            if (input.StartsWith("\"") && input.EndsWith("\""))
                input = input.Substring(1, input.Length - 2);

            string unescaped = input.Replace("\"\"", "\"");
            unescaped = unescaped.Replace(NewLineToken, "\n");

            return unescaped;
        }

        private static string[] ParseCsvLine(string line)
        {
            var segments = new List<string>();
            var buffer = new StringBuilder();
            bool insideQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];

                if (ch == '"' && (i == 0 || !insideQuotes))
                {
                    insideQuotes = !insideQuotes;
                    continue;
                }

                if (ch == '"' && insideQuotes)
                {
                    if (i + 1 < line.Length && line[i + 1] == '"')
                    {
                        buffer.Append('"');
                        i++;
                        continue;
                    }
                    insideQuotes = false;
                    continue;
                }

                if (ch == Delimiter && !insideQuotes)
                {
                    segments.Add(buffer.ToString());
                    buffer.Clear();
                    continue;
                }

                buffer.Append(ch);
            }

            segments.Add(buffer.ToString());

            return segments.ToArray();
        }
    }

    // Пример пользовательских классов для профиля и задач:

    public class UserProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }
    }

    public class TaskCollection
    {
        private readonly List<TaskItem> tasks = new();

        public void AddTask(TaskItem task) => tasks.Add(task);

        public IEnumerable<TaskItem> GetAllTasks() => tasks;
    }

    public class TaskItem
    {
        public string Description { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime LastModified { get; set; }

        public TaskItem(string description)
        {
            Description = description;
            IsCompleted = false;
            LastModified = DateTime.Now;
        }

        public void MarkComplete()
        {
            IsCompleted = true;
            LastModified = DateTime.Now;
        }
    }
}