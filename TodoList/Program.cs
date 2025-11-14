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
            
            // Создаем директорию для данных
            FileManager.EnsureDataDirectory(FileManager.Paths.DataDirectory);
            
            // Пытаемся загрузить профиль
            userProfile = FileManager.LoadProfile(FileManager.Paths.ProfileFile);
            if (userProfile == null)
            {
                // Если профиль не загружен, создаем новый
                userProfile = Profile.CreateFromInput();
                FileManager.SaveProfile(userProfile, FileManager.Paths.ProfileFile);
            }
            
            // Пытаемся загрузить задачи
            todoList = FileManager.LoadTodos(FileManager.Paths.TodosFile);
            
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
                        bool success = command.Execute();
                        
                        // Автосохранение после успешного выполнения команд, изменяющих данные
                        if (success && IsDataModifyingCommand(command))
                        {
                            SaveAllData();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($" Ошибка при выполнении команды: {ex.Message}");
                    }
                }
            }
        }

        static bool IsDataModifyingCommand(ICommand command)
        {
            return command is AddCommand || 
                   command is DoneCommand || 
                   command is UpdateCommand || 
                   command is RemoveCommand;
        }

        static void SaveAllData()
        {
            FileManager.SaveProfile(userProfile, FileManager.Paths.ProfileFile);
            FileManager.SaveTodos(todoList, FileManager.Paths.TodosFile);
        }
    }
}
