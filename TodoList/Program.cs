using System;
using System.IO;
using TodoApp.Commands;

namespace TodoApp
{
    class Program
    {
        private static CommandParser commandParser;

        static void Main()
        {
            Console.WriteLine("выполнил работу Турищев Иван");
            InitializeApplication();
            RunCommandLoop();
        }

        static void InitializeApplication()
        {
            Console.WriteLine("=== ПРИЛОЖЕНИЕ ДЛЯ УПРАВЛЕНИЯ ЗАДАЧАМИ ===");
            
            // Проверяем существование папки
            FileManager.EnsureDataDirectory("data");
            
            // Загружаем или создаем профиль
            var profile = LoadUserProfile();
            
            // Загружаем или создаем список задач
            var todoList = LoadTodoList();
            
            // Инициализируем AppInfo
            AppInfo.CurrentProfile = profile;
            AppInfo.Todos = todoList;
            
            // Инициализируем парсер команд
            commandParser = new CommandParser();

            Console.WriteLine($"\nДобро пожаловать, {profile.FirstName}!");
            Console.WriteLine($"Загружено задач: {todoList.Count}");
            Console.WriteLine("Введите 'help' для списка команд.");
        }

        static Profile LoadUserProfile()
        {
            if (File.Exists(AppInfo.ProfileFilePath))
            {
                Console.WriteLine(" Загружаем профиль пользователя...");
                var profile = FileManager.LoadProfile(AppInfo.ProfileFilePath);
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

            Console.WriteLine(" Создаем новый профиль...");
            var newProfile = Profile.CreateFromInput();
            
            FileManager.SaveProfile(newProfile, AppInfo.ProfileFilePath);
            Console.WriteLine(" Новый профиль создан и сохранен");
            
            return newProfile;
        }

        static TodoList LoadTodoList()
        {
            if (File.Exists(AppInfo.TodosFilePath))
            {
                Console.WriteLine(" Загружаем список задач...");
                var todos = FileManager.LoadTodos(AppInfo.TodosFilePath);
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

            Console.WriteLine(" Создаем новый список задач...");
            var newTodoList = new TodoList();
            
            FileManager.SaveTodos(newTodoList, AppInfo.TodosFilePath);
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
