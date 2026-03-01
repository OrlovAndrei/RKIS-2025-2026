using System;
using System.IO;

namespace Todolist
{
    class Program
    {
        private static string dataDirectory;
        private static string profileFilePath;
        private static string todoFilePath;

        static void Main()
        {
            InitializeFilePaths();
            LoadData();
            RunTodoList();
        }

        static void InitializeFilePaths()
        {
            dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "data");
            profileFilePath = Path.Combine(dataDirectory, "profile.txt");
            todoFilePath = Path.Combine(dataDirectory, "todo.csv");
        }

        static void LoadData()
        {
            Console.WriteLine("Работу выполнили Гергель и Дайнеко 3832");
            Console.WriteLine();

            FileManager.EnsureDataDirectory(dataDirectory);

            AppInfo.CurrentProfile = FileManager.LoadProfile(profileFilePath);
            if (AppInfo.CurrentProfile == null)
            {
                CreateNewProfile();
            }
            else
            {
                Console.WriteLine($"Загружен профиль: {AppInfo.CurrentProfile.GetInfo()}");
            }

            AppInfo.Todos = FileManager.LoadTodos(todoFilePath);

            Console.WriteLine();
        }

        static void CreateNewProfile()
        {
            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            int birthYear = int.Parse(Console.ReadLine());

            AppInfo.CurrentProfile = new Profile(firstName, lastName, birthYear);
            FileManager.SaveProfile(AppInfo.CurrentProfile, profileFilePath);
            Console.WriteLine($"Создан новый профиль: {AppInfo.CurrentProfile.GetInfo()}");
        }

        static void RunTodoList()
        {
            while (true)
            {
                Console.Write("Введите команду (help - список команд): ");
                string input = Console.ReadLine();

                ICommand command = CommandParser.Parse(input, AppInfo.Todos, AppInfo.CurrentProfile);

                if (command is AddCommand || command is DeleteCommand ||
                    command is UpdateCommand || command is StatusCommand)
                {
                    AppInfo.UndoStack.Push(command);
                    AppInfo.RedoStack.Clear();
                }

                command.Execute();

                Console.WriteLine();
            }
        }
    }
}
