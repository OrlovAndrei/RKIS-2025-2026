using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
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
                // Используем Directory.Exists и Directory.CreateDirectory
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
                // Используем Path.GetDirectoryName для получения директории
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Используем File operations
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
                // Используем File.Exists для проверки существования файла
                if (!File.Exists(filePath))
                {
                    Console.WriteLine(" Файл профиля не найден");
                    return null;
                }

                // Проверяем, не пустой ли файл с помощью FileInfo
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Length == 0)
                {
                    Console.WriteLine(" Файл профиля пуст");
                    return null;
                }

                // Используем File operations для чтения
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string firstName = reader.ReadLine()?.Trim();
                    string lastName = reader.ReadLine()?.Trim();
                    
                    if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                    {
                        Console.WriteLine("Файл профиля содержит неполные данные");
                        return null;
                    }
                    
                    if (int.TryParse(reader.ReadLine()?.Trim(), out int birthYear))
                    {
                        Console.WriteLine($"Профиль загружен из: {filePath}");
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
                // Используем Path.GetDirectoryName для получения директории
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Используем File operations
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Заголовок CSV с разделителем ;
                    writer.WriteLine("Index;Text;IsDone;LastUpdate");
                    
                    // Данные задач
                    for (int i = 0; i < todos.Count; i++)
                    {
                        var task = todos.GetItem(i);
                        
                        // Экранируем текст: заменяем переносы на \n и обрамляем кавычками
                        string escapedText = EscapeCsvText(task.Text);
                        string isDone = task.IsDone ? "true" : "false";
                        string lastUpdate = task.LastUpdate.ToString("yyyy-MM-ddTHH:mm:ss");
                        
                        writer.WriteLine($"{i};{escapedText};{isDone};{lastUpdate}");
                    }
                }
                Console.WriteLine($"Задачи сохранены в: {filePath} (всего: {todos.Count})");
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
                // Используем File.Exists для проверки существования файла
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Файл задач не найден");
                    return todoList;
                }

                // Проверяем, не пустой ли файл с помощью FileInfo
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Length == 0)
                {
                    Console.WriteLine("Файл задач пуст");
                    return todoList;
                }

                // Используем File operations для чтения
                using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
                {
                    // Пропускаем заголовок
                    string header = reader.ReadLine();
                    if (header == null)
                    {
                        Console.WriteLine("Файл задач пуст");
                        return todoList;
                    }
                    
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

        // Вспомогательный метод для экранирования текста в CSV
        private static string EscapeCsvText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "\"\"";

            // Заменяем переносы строк на \n
            string processedText = text.Replace("\r\n", "\\n")
                                      .Replace("\n", "\\n")
                                      .Replace("\r", "\\n");

            // Если текст содержит ; или кавычки - обрамляем в кавычки
            if (processedText.Contains(";") || processedText.Contains("\"") || processedText.Contains("\\n"))
            {
                // Экранируем кавычки удвоением
                processedText = processedText.Replace("\"", "\"\"");
                return $"\"{processedText}\"";
            }

            return processedText;
        }

        // Вспомогательный метод для парсинга строки CSV
        private static TodoItem ParseCsvLine(string line)
        {
            try
            {
                List<string> fields = ParseCsvFields(line);
                
                if (fields.Count >= 4)
                {
                    // Парсим индекс (не используем, но проверяем)
                    if (!int.TryParse(fields[0], out int index))
                    {
                        Console.WriteLine($" Ошибка парсинга индекса: {fields[0]}");
                        return null;
                    }

                    string text = fields[1];
                    bool isDone = fields[2].ToLower() == "true";
                    
                    if (DateTime.TryParse(fields[3], out DateTime lastUpdate))
                    {
                        // Восстанавливаем переносы строк из \n
                        text = text.Replace("\\n", "\n");
                        
                        var task = new TodoItem(text);
                        
                        if (isDone)
                        {
                            task.MarkDone();
                            // Восстанавливаем оригинальную дату
                            var lastUpdateField = typeof(TodoItem).GetField("lastUpdate", 
                                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                            lastUpdateField?.SetValue(task, lastUpdate);
                        }
                        else
                        {
                            // Для невыполненных задач тоже восстанавливаем дату
                            var lastUpdateField = typeof(TodoItem).GetField("lastUpdate", 
                                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                            lastUpdateField?.SetValue(task, lastUpdate);
                        }
                        
                        return task;
                    }
                    else
                    {
                        Console.WriteLine($" Ошибка парсинга даты: {fields[3]}");
                    }
                }
                else
                {
                    Console.WriteLine($" Неверное количество полей в строке: {fields.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка парсинга строки CSV: {ex.Message}");
            }
            
            return null;
        }

        // Вспомогательный метод для парсинга полей CSV с разделителем ;
        private static List<string> ParseCsvFields(string line)
        {
            List<string> fields = new List<string>();
            bool inQuotes = false;
            StringBuilder currentField = new StringBuilder();
            
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        // Удвоенная кавычка внутри кавычек
                        currentField.Append('"');
                        i++; // Пропускаем следующую кавычку
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ';' && !inQuotes)
                {
                    fields.Add(currentField.ToString());
                    currentField.Clear();
                }
                else
                {
                    currentField.Append(c);
                }
            }
            
            // Добавляем последнее поле
            fields.Add(currentField.ToString());
            
            return fields;
        }
    }
}
