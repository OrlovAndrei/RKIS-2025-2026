using System;
using System.IO;
using System.Collections.Generic;
using TodoApp.Commands;

namespace TodoApp
{
    public static class FileManager
    {
        // Проверяет существование папки, если её нет - создаёт
        public static void EnsureDataDirectory(string dirPath)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                    Console.WriteLine($" Создана директория: {dirPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка при создании директории: {ex.Message}");
            }
        }

        // Сохраняет данные пользователя в profile.txt
        public static void SaveProfile(Profile profile, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(profile.FirstName);
                    writer.WriteLine(profile.LastName);
                    writer.WriteLine(profile.BirthYear);
                }
                Console.WriteLine($" Профиль сохранен в: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка при сохранении профиля: {ex.Message}");
            }
        }

        // Загружает данные пользователя из profile.txt
        public static Profile LoadProfile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine(" Файл профиля не найден, будет создан новый профиль");
                    return null;
                }

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string firstName = reader.ReadLine()?.Trim();
                    string lastName = reader.ReadLine()?.Trim();
                    
                    if (int.TryParse(reader.ReadLine()?.Trim(), out int birthYear))
                    {
                        return new Profile(firstName, lastName, birthYear);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка при загрузке профиля: {ex.Message}");
            }

            return null;
        }

        // Сохраняет задачи в CSV-файл todo.csv
        public static void SaveTodos(TodoList todos, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Заголовок CSV
                    writer.WriteLine("Text,IsDone,LastUpdate");
                    
                    // Данные задач
                    for (int i = 0; i < todos.Count; i++)
                    {
                        var task = todos.GetItem(i);
                        // Экранируем запятые и кавычки в тексте
                        string escapedText = EscapeCsvField(task.Text);
                        string isDone = task.IsDone ? "true" : "false";
                        string lastUpdate = task.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss");
                        
                        writer.WriteLine($"{escapedText},{isDone},{lastUpdate}");
                    }
                }
                Console.WriteLine($" Задачи сохранены в: {filePath} (всего: {todos.Count})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка при сохранении задач: {ex.Message}");
            }
        }

        // Загружает задачи из CSV-файла
        public static TodoList LoadTodos(string filePath)
        {
            TodoList todoList = new TodoList();

            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine(" Файл задач не найден, будет создан новый список");
                    return todoList;
                }

                using (StreamReader reader = new StreamReader(filePath))
                {
                    // Пропускаем заголовок
                    string header = reader.ReadLine();
                    
                    string line;
                    int loadedCount = 0;
                    
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var task = ParseCsvLine(line);
                        if (task != null)
                        {
                            todoList.Add(task);
                            loadedCount++;
                        }
                    }
                    
                    Console.WriteLine($" Задачи загружены из: {filePath} (всего: {loadedCount})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка при загрузке задач: {ex.Message}");
            }

            return todoList;
        }

        // Вспомогательный метод для экранирования полей CSV
        private static string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "\"\"";

            // Если поле содержит запятые, кавычки или переносы строк - заключаем в кавычки
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                // Экранируем кавычки удвоением
                field = field.Replace("\"", "\"\"");
                return $"\"{field}\"";
            }

            return field;
        }

        // Вспомогательный метод для парсинга строки CSV
        private static TodoItem ParseCsvLine(string line)
        {
            try
            {
                List<string> fields = new List<string>();
                bool inQuotes = false;
                string currentField = "";
                
                for (int i = 0; i < line.Length; i++)
                {
                    char c = line[i];
                    
                    if (c == '"')
                    {
                        if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                        {
                            // Удвоенная кавычка внутри кавычек
                            currentField += '"';
                            i++; // Пропускаем следующую кавычку
                        }
                        else
                        {
                            inQuotes = !inQuotes;
                        }
                    }
                    else if (c == ',' && !inQuotes)
                    {
                        fields.Add(currentField);
                        currentField = "";
                    }
                    else
                    {
                        currentField += c;
                    }
                }
                
                // Добавляем последнее поле
                fields.Add(currentField);
                
                if (fields.Count >= 3)
                {
                    string text = fields[0];
                    bool isDone = fields[1].ToLower() == "true";
                    DateTime lastUpdate;
                    
                    if (DateTime.TryParse(fields[2], out lastUpdate))
                    {
                        var task = new TodoItem(text);
                        if (isDone)
                        {
                            task.MarkDone();
                            // Восстанавливаем оригинальную дату
                            var field = typeof(TodoItem).GetField("lastUpdate", 
                                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                            field?.SetValue(task, lastUpdate);
                        }
                        return task;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка парсинга строки CSV: {ex.Message}");
            }
            
            return null;
        }

        // Метод для получения стандартных путей файлов
        public static class Paths
        {
            public static string DataDirectory => "data";
            public static string ProfileFile => Path.Combine(DataDirectory, "profile.txt");
            public static string TodosFile => Path.Combine(DataDirectory, "todo.csv");
        }
    }
}