using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace TodoList
{
    public static class FileManager
    {
        public static void EnsureDataDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                Console.WriteLine($"Создана папка для данных: {dirPath}");
            }
        }

        public static void SaveProfile(Profile profile, string filePath)
        {
            try
            {
                string[] profileData = {
                    $"FirstName: {profile.FirstName}",
                    $"LastName: {profile.LastName}",
                    $"BirthYear: {profile.BirthYear}"
                };
                File.WriteAllLines(filePath, profileData);
                Console.WriteLine("Профиль сохранён.");
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
                string firstName = "";
                string lastName = "";
                int birthYear = 0;

                foreach (var line in lines)
                {
                    if (line.StartsWith("FirstName: "))
                        firstName = line.Substring(11);
                    else if (line.StartsWith("LastName: "))
                        lastName = line.Substring(10);
                    else if (line.StartsWith("BirthYear: ") && int.TryParse(line.Substring(11), out int year))
                        birthYear = year;
                }

                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && birthYear > 0)
                {
                    Console.WriteLine("Профиль загружен из файла.");
                    return new Profile(firstName, lastName, birthYear);
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки профиля: {ex.Message}");
                return null;
            }
        }

        public static void SaveTodos(TodoList todos, string filePath)
        {
            try
            {
                var csvLines = new List<string> { "Index;Text;IsDone;LastUpdate" };
                
                for (int i = 1; i <= todos.Count; i++)
                {
                    var item = todos.GetItem(i);
                    string escapedText = EscapeCsv(item.Text);
                    string line = $"{i};{escapedText};{item.IsDone};{item.LastUpdate:yyyy-MM-ddTHH:mm:ss}";
                    csvLines.Add(line);
                }
                
                File.WriteAllLines(filePath, csvLines);
                Console.WriteLine("Задачи сохранены в файл.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения задач: {ex.Message}");
            }
        }

        public static TodoList LoadTodos(string filePath)
        {
            var todoList = new TodoList();
            
            if (!File.Exists(filePath))
                return todoList;

            try
            {
                var lines = File.ReadAllLines(filePath);
                
                foreach (var line in lines.Skip(1))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var parts = ParseCsvLine(line);
                    
                    if (parts.Length >= 4)
                    {
                        string text = UnescapeCsv(parts[1]);
                        var item = new TodoItem(text);
                        
                        if (bool.TryParse(parts[2], out bool isDone) && isDone)
                        {
                            item.MarkDone();
                        }
                        
                        
                        todoList.Add(item);
                    }
                }
                
                Console.WriteLine("Задачи загружены из файла.");
                return todoList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки задач: {ex.Message}");
                return todoList;
            }
        }

        private static string EscapeCsv(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "\"\"";

            string escaped = text
                .Replace("\"", "\"\"")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r");
            
            return $"\"{escaped}\"";
        }

        private static string UnescapeCsv(string text)
        {
            if (text.StartsWith("\"") && text.EndsWith("\""))
            {
                text = text.Substring(1, text.Length - 2);
            }
            
            return text
                .Replace("\"\"", "\"")
                .Replace("\\n", "\n")
                .Replace("\\r", "\r");
        }

        private static string[] ParseCsvLine(string line)
        {
            var parts = new List<string>();
            int start = 0;
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (line[i] == ';' && !inQuotes)
                {
                    parts.Add(line.Substring(start, i - start));
                    start = i + 1;
                }
            }
            
            parts.Add(line.Substring(start));
            
            return parts.ToArray();
        }
    }
}