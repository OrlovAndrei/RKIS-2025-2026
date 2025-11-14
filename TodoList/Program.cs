using System;
using System.IO;

namespace TodoList
{
    class Program
    {
        private static Profile user;
        private static TodoList todoList = new TodoList();
        private static string dataDirPath;
        private static string profileFilePath;
        private static string todoFilePath;

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Морозов и Прокопенко");
            
            InitializePaths();
            
            LoadAllData();
            
            if (user == null)
            {
                SetUserData();
            }

            while (true)
            {
                Console.Write("Введите команду: ");
                string fullInput = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(fullInput))
                    continue;

                if (fullInput.ToLower() == "exit")
                {
                    SaveAllData();
                    Console.WriteLine("Данные сохранены. До свидания!");
                    return;
                }

                if (fullInput.ToLower().StartsWith("add ") && fullInput.EndsWith("\""))
                {
                    string text = fullInput.Substring(4).Trim();
                    if (text.StartsWith("\"") && text.EndsWith("\""))
                    {
                        ICommand command = CommandParser.Parse(fullInput, todoList, user);
                        if (command != null)
                        {
                            command.Execute();
                            SaveAllData();
                        }
                    }
                    else
                    {
                        HandleMultiLineAdd(text);
                    }
                }
                else
                {
                    ICommand command = CommandParser.Parse(fullInput, todoList, user);
                    if (command != null)
                    {
                        command.Execute();
                        
                        if (IsStateChangingCommand(fullInput))
                        {
                            SaveAllData();
                        }
                    }
                }
            }
        }

        private static void HandleMultiLineAdd(string initialText)
        {
            Console.WriteLine("Введите текст задачи (для завершения введите пустую строку):");
            
            var lines = new System.Collections.Generic.List<string>();
            
            if (!string.IsNullOrWhiteSpace(initialText))
            {
                lines.Add(initialText);
            }
            
            while (true)
            {
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }
                lines.Add(line);
            }
            
            if (lines.Count > 0)
            {
                string multiLineText = string.Join("\n", lines);
                
                string commandText = $"add \"{multiLineText}\"";
                ICommand command = CommandParser.Parse(commandText, todoList, user);
                
                if (command != null)
                {
                    command.Execute();
                    SaveAllData();
                    Console.WriteLine("Многострочная задача добавлена!");
                }
            }
            else
            {
                Console.WriteLine("Текст задачи не может быть пустым");
            }
        }

        private static void InitializePaths()
        {
            dataDirPath = Path.Combine(Directory.GetCurrentDirectory(), "data");
            profileFilePath = Path.Combine(dataDirPath, "profile.txt");
            todoFilePath = Path.Combine(dataDirPath, "todo.csv");
        }

        private static void LoadAllData()
        {
            FileManager.EnsureDataDirectory(dataDirPath);
            
            user = FileManager.LoadProfile(profileFilePath);
            if (user != null)
            {
                Console.WriteLine($"Загружен профиль: {user.GetInfo()}");
            }
            
            todoList = FileManager.LoadTodos(todoFilePath);
        }

        private static void SaveAllData()
        {
            if (user != null)
            {
                FileManager.SaveProfile(user, profileFilePath);
            }
            FileManager.SaveTodos(todoList, todoFilePath);
        }

        private static bool IsStateChangingCommand(string command)
        {
            var lowerCommand = command.ToLower();
            return lowerCommand.StartsWith("add ") ||
                   lowerCommand.StartsWith("done ") ||
                   lowerCommand.StartsWith("delete ") ||
                   lowerCommand.StartsWith("update ") ||
                   lowerCommand == "profile";
        }

        static void SetUserData()
        {
            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите год вашего рождения: ");
            if (int.TryParse(Console.ReadLine(), out int birthYear))
            {
                user = new Profile(name, lastName, birthYear);
                Console.WriteLine($"Добавлен пользователь: {user.GetInfo()}");
                
                FileManager.SaveProfile(user, profileFilePath);
            }
            else
            {
                Console.WriteLine("Ошибка: год рождения должен быть числом");
                SetUserData();
            }
            Console.WriteLine();
        }
    }
}