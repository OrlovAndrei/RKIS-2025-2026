using System;
using System.IO;

namespace TodoList
{
    class Program
    {
        static bool isRunning = true;

        public static void Main()
        {
            Console.WriteLine("Работу выполнили Турчин Крошняк");

            string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            string profilePath = Path.Combine(dataDir, "profile.txt");
            string todoPath = Path.Combine(dataDir, "todo.csv");

            FileManager.EnsureDataDirectory(dataDir);

            Profile profile;
            if (File.Exists(profilePath))
            {
                try
                {
                    profile = FileManager.LoadProfile(profilePath);
                    Console.WriteLine($"Загружен профиль: {profile.GetInfo()}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка загрузки профиля: {ex.Message}. Будет создан новый профиль.");
                    profile = CreateNewProfile();
                    FileManager.SaveProfile(profile, profilePath);
                }
            }
            else
            {
                profile = CreateNewProfile();
                FileManager.SaveProfile(profile, profilePath);
            }

            TodoList todoList;
            if (File.Exists(todoPath))
            {
                try
                {
                    todoList = FileManager.LoadTodos(todoPath);
                    Console.WriteLine($"Загружено задач: {todoList.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка загрузки задач: {ex.Message}. Создан новый список.");
                    todoList = new TodoList();
                    FileManager.SaveTodos(todoList, todoPath);
                }
            }
            else
            {
                todoList = new TodoList();
                FileManager.SaveTodos(todoList, todoPath);
            }

            Action exitAction = () => isRunning = false;

            while (isRunning)
            {
                Console.Write("\nВведите команду: ");
                string input = Console.ReadLine();

                ICommand? command = CommandParser.Parse(input, todoList, profile, exitAction, todoPath, profilePath);
                if (command != null)
                {
                    command.Execute();
                }
            }
        }

        private static Profile CreateNewProfile()
        {
            Console.Write("Введите ваше имя: ");
            var name = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            var surname = Console.ReadLine();
            Console.Write("Введите ваш год рождения: ");
            int year = int.Parse(Console.ReadLine());
            var profile = new Profile(name, surname, year);
            Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");
            return profile;
        }
    }
}