using System;
using System.IO;
using System.Collections.Generic;
using Todolist.Exceptions;

namespace Todolist
{
    public static class FileManager
    {
        public static void EnsureDataDirectory(string dirPath)
        {
            try
            {
                if (!Directory.Exists(dirPath)) 
                    Directory.CreateDirectory(dirPath);
            }
            catch (UnauthorizedAccessException)
            {
                throw new BusinessLogicException($"Нет прав на создание директории: {dirPath}");
            }
            catch (PathTooLongException)
            {
                throw new BusinessLogicException("Слишком длинный путь к директории");
            }
            catch (IOException ex)
            {
                throw new BusinessLogicException($"Ошибка ввода-вывода при создании директории: {ex.Message}");
            }
        }

        public static void SaveProfile(Profile profile, string filePath)
        {
            if (profile == null) 
                throw new InvalidArgumentException("Профиль не может быть null");
                
            try
            {
                string profileData = $"{profile.FirstName};{profile.LastName};{profile.BirthYear}";
                File.WriteAllText(filePath, profileData);
            }
            catch (UnauthorizedAccessException)
            {
                throw new BusinessLogicException($"Нет прав на запись в файл: {filePath}");
            }
            catch (DirectoryNotFoundException)
            {
                throw new BusinessLogicException($"Директория для файла профиля не найдена: {filePath}");
            }
            catch (IOException ex)
            {
                throw new BusinessLogicException($"Ошибка при сохранении профиля: {ex.Message}");
            }
        }

        public static Profile LoadProfile(string filePath)
        {
            if (!File.Exists(filePath)) 
                return null;

            try
            {
                string content = File.ReadAllText(filePath);
                string[] parts = content.Split(';');

                if (parts.Length == 3)
                {
                    string firstName = parts[0];
                    string lastName = parts[1];
                    
                    if (!int.TryParse(parts[2], out int birthYear))
                        throw new BusinessLogicException("Неверный формат года рождения в файле профиля");
                    
                    return new Profile(firstName, lastName, birthYear);
                }
                
                throw new BusinessLogicException("Неверный формат файла профиля");
            }
            catch (UnauthorizedAccessException)
            {
                throw new BusinessLogicException($"Нет прав на чтение файла: {filePath}");
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (IOException ex)
            {
                throw new BusinessLogicException($"Ошибка при загрузке профиля: {ex.Message}");
            }
        }

        public static void SaveTodos(Todolist todos, string filePath)
        {
            if (todos == null) 
                throw new InvalidArgumentException("Список задач не может быть null");

            try
            {
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
            catch (UnauthorizedAccessException)
            {
                throw new BusinessLogicException($"Нет прав на запись в файл: {filePath}");
            }
            catch (DirectoryNotFoundException)
            {
                throw new BusinessLogicException($"Директория для файла задач не найдена: {filePath}");
            }
            catch (IOException ex)
            {
                throw new BusinessLogicException($"Ошибка при сохранении задач: {ex.Message}");
            }
        }

        public static Todolist LoadTodos(string filePath)
        {
            Todolist todos = new Todolist();
            
            if (!File.Exists(filePath)) 
                return todos;

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                        
                    string[] parts = line.Split(';');
                    
                    if (parts.Length >= 4)
                    {
                        if (!Enum.TryParse<TodoStatus>(parts[1], out TodoStatus status))
                            throw new BusinessLogicException($"Неверный статус в файле: {parts[1]}");
                        
                        if (!DateTime.TryParse(parts[2], out DateTime lastUpdate))
                            throw new BusinessLogicException($"Неверный формат даты в файле: {parts[2]}");
                        
                        string text = string.Join(";", parts, 3, parts.Length - 3);
                        text = UnescapeCsv(text);
                        
                        TodoItem item = new TodoItem(text, status, lastUpdate);
                        todos.Add(item);
                    }
                }
                
                return todos;
            }
            catch (UnauthorizedAccessException)
            {
                throw new BusinessLogicException($"Нет прав на чтение файла: {filePath}");
            }
            catch (FileNotFoundException)
            {
                return todos;
            }
            catch (IOException ex)
            {
                throw new BusinessLogicException($"Ошибка при загрузке задач: {ex.Message}");
            }
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