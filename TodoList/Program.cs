using System;
using TodoApp.Commands;

namespace TodoApp
{
    class Program
    {
        private static Profile userProfile;
        private static TodoList todoList;
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
            
            // Создание профиля пользователя
            userProfile = Profile.CreateFromInput();
            todoList = new TodoList();
            commandParser = new CommandParser(todoList, userProfile);

            Console.WriteLine($"\nДобро пожаловать, {userProfile.FirstName}!");
            Console.WriteLine("Введите 'help' для списка команд.");
        }

        static void RunCommandLoop()
        {
            while (true)
            {
                Console.Write("\n> ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                    continue;

                // Главный цикл программы согласно требованиям
                ICommand command = commandParser.Parse(input);
                
                if (command != null)
                {
                    try
                    {
                        command.Execute();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Ошибка при выполнении команды: {ex.Message}");
                    }
                }
            }
        }
    }
}
