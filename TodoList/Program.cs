using System;
using System.IO;
using TodoApp.Commands;

namespace TodoApp
{
    class Program
    {
        private static Profile userProfile;
        private static TodoList todoList;
        private static CommandParser commandParser;

        // Пути к файлам с использованием Path.Combine
        private static readonly string DataDirectory = "data";
        private static readonly string ProfileFilePath = Path.Combine(DataDirectory, "profile.txt");
        private static readonly string TodosFilePath = Path.Combine(DataDirectory, "todo.csv");

        static void Main()
        {
            Console.WriteLine("выполнил работу Турищев Иван");
            InitializeApplication();
            RunCommandLoop();
        }

        static void InitializeApplication()
        {
            Console.WriteLine("=== ПРИЛОЖЕНИЕ ДЛЯ УПРАВЛЕНИЯ ЗАДАЧАМИ ===");
            
            // Проверяем существование папки с помощью FileManager.EnsureDataDirectory
            FileManager.EnsureDataDirectory(DataDirectory);
            
            // Загружаем или создаем профиль
            userProfile = LoadUserProfile();
            
            // Загружаем или создаем список задач
            todoList = LoadTodoList();
            
            commandParser = new CommandParser(todoList, userProfile);

            Console.WriteLine($"\nДобро пожаловать, {userProfile.FirstName}!");
            Console.WriteLine($"Загружено задач: {todoList.Count}");
            Console.WriteLine("Введите 'help' для списка команд.");
        }

        static Profile LoadUserProfile()
        {
            // Используем File.Exists для проверки существования файла
            if (File.Exists(ProfileFilePath))
            {
                Console.WriteLine(" Загружаем профиль пользователя...");
                var profile = FileManager.LoadProfile(ProfileFilePath);
                if (profile != null)
                {
                    Console.WriteLine($" Профиль загружен: {profile.GetInfo()}");
                    return profile;
                }
                else
                {
                    Console.WriteLine(" Не удалось загрузить профиль, создаем новый...");
                }
            }

            // Если файлов нет - создаем новый объект Profile
            Console.WriteLine(" Создаем новый профиль...");
            var newProfile = Profile.CreateFromInput();
            
            // Создаем файл profile.txt
            FileManager.SaveProfile(newProfile, ProfileFilePath);
            Console.WriteLine(" Новый профиль создан и сохранен");
            
            return newProfile;
        }

        static TodoList LoadTodoList()
        {
            // Используем File.Exists для проверки существования файла
            if (File.Exists(TodosFilePath))
            {
                Console.WriteLine(" Загружаем список задач...");
                var todos = FileManager.LoadTodos(TodosFilePath);
                if (todos != null)
                {
                    Console.WriteLine($" Задачи загружены: {todos.Count} задач");
                    return todos;
                }
                else
                {
                    Console.WriteLine(" Не удалось загрузить задачи, создаем новый список...");
                }
            }

            // Если файлов нет - создаем новый объект TodoList
            Console.WriteLine(" Создаем новый список задач...");
            var newTodoList = new TodoList();
            
            // Создаем файл todo.csv
            FileManager.SaveTodos(newTodoList, TodosFilePath);
            Console.WriteLine(" Новый список задач создан и сохранен");
            
            return newTodoList;
        }

        static void RunCommandLoop()
        {
            while (true)
            {
                Console.Write("\n> ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                    continue;

                ICommand command = commandParser.Parse(input);
                
                if (command != null)
                {
                    try
                    {
                        command.Execute();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Ошибка при выполнении команды: {ex.Message}");
                    }
                }
            }
        }
    }
}
