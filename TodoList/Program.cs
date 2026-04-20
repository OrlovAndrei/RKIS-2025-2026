using System;
using System.IO;

namespace TodoList
{
    class Program
    {
        static TodoList _todoList = new TodoList();
        static Profile _userProfile;

        static string _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
        static string _profilePath = Path.Combine(_dataDir, "profile.txt");
        static string _todoPath = Path.Combine(_dataDir, "todo.csv");

        public static void Main()
        {
            Console.WriteLine("Работу выполнила Сайтамирова");
            FileManager.EnsureDataDirectory(_dataDir);
            _userProfile = FileManager.LoadProfile(_profilePath);
            
            if (_userProfile == null)
            {
                Console.WriteLine("Введите вашу фамилию:");
                string lastName = Console.ReadLine();

                Console.WriteLine("Введите ваше имя:");
                string firstName = Console.ReadLine();

                Console.WriteLine("Введите ваш год рождения:");
                string yearOfBirthInput = Console.ReadLine();

                int yearOfBirth = int.Parse(yearOfBirthInput);
                _userProfile = new Profile(firstName, lastName, yearOfBirth);
                FileManager.SaveProfile(_userProfile, _profilePath);
            }
            _todoList = FileManager.LoadTodos(_todoPath);

            while (true)
            {
                Console.Write("Введите команду: ");
                string input = Console.ReadLine();

                if (input == "exit")
                    return;

                ICommand command = CommandParser.Parse(input, _todoList, _userProfile);
                command.Execute();

                if (input.StartsWith("add") || input.StartsWith("done") || input.StartsWith("update") || input.StartsWith("delete"))
                {
                    FileManager.SaveTodos(_todoList, _todoPath);
                }

                if (input.StartsWith("profile"))
                {
                    FileManager.SaveProfile(_userProfile, _profilePath);
                }
            }
        }
    }
}