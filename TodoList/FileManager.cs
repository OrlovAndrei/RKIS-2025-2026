using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Todolist
{
    public static class FileManager
    {
        private static readonly string DataDirPath = Path.Combine(Directory.GetCurrentDirectory(), "data");

        public static void EnsureDataDirectory()
        {
            if (!Directory.Exists(DataDirPath)) 
                Directory.CreateDirectory(DataDirPath);
        }

        public static void SaveProfiles(List<Profile> profiles)
        {
            EnsureDataDirectory();
            string filePath = Path.Combine(DataDirPath, "profiles.csv");
            List<string> csvLines = new List<string>();

            foreach (var profile in profiles)
            {
                string line = $"{profile.Id};{profile.Login};{profile.Password};{profile.FirstName};{profile.LastName};{profile.BirthYear}";
                csvLines.Add(line);
            }

            File.WriteAllLines(filePath, csvLines);
        }

        public static List<Profile> LoadProfiles()
        {
            EnsureDataDirectory();
            string filePath = Path.Combine(DataDirPath, "profiles.csv");
            List<Profile> profiles = new List<Profile>();

            if (!File.Exists(filePath))
                return profiles;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts.Length == 6)
                {
                    Guid id = Guid.Parse(parts[0]);
                    string login = parts[1];
                    string password = parts[2];
                    string firstName = parts[3];
                    string lastName = parts[4];
                    int birthYear = int.Parse(parts[5]);

                    profiles.Add(new Profile(id, login, password, firstName, lastName, birthYear));
                }
            }

            return profiles;
        }

        public static void SaveTodos(Todolist todos, Guid userId)
        {
            EnsureDataDirectory();
            string filePath = Path.Combine(DataDirPath, $"todos_{userId}.csv");
            List<string> csvLines = new List<string>();
            int index = 1;

            foreach (var item in todos)
            {
                string escapedText = EscapeCsv(item.Text);
                string line = $"{index};{item.Status};{item.LastUpdate:yyyy-MM-ddTHH:mm:ss};{escapedText}";
                csvLines.Add(line);
                index++;
            }

            File.WriteAllLines(filePath, csvLines);
        }

        public static Todolist LoadTodos(Guid userId)
        {
            EnsureDataDirectory();
            Todolist todos = new Todolist();
            string filePath = Path.Combine(DataDirPath, $"todos_{userId}.csv");

            if (!File.Exists(filePath))
                return todos;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts.Length >= 4)
                {
                    TodoStatus status = Enum.Parse<TodoStatus>(parts[1]);
                    DateTime lastUpdate = DateTime.Parse(parts[2]);

                    string text = string.Join(";", parts, 3, parts.Length - 3);
                    text = UnescapeCsv(text);

                    TodoItem item = new TodoItem(text, status, lastUpdate);
                    todos.Add(item);
                }
            }

            return todos;
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