using System;
using TodoList.Commands;

namespace TodoList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Шелепов и Кузьменко");

            string dataDir = "data";
            string profilePath = Path.Combine(dataDir, "profile.txt");
            string todoPath = Path.Combine(dataDir, "todo.csv");

            FileManager.EnsureDataDirectory(dataDir);

            if (!File.Exists(profilePath))
                File.WriteAllText(profilePath, "");
            if (!File.Exists(todoPath))
                File.WriteAllText(todoPath, "");

            Profile? profile = FileManager.LoadProfile(profilePath);
            TodoList todoList;

            if (profile == null)
            {
                Console.Write("Введите имя: ");
                string firstName = Console.ReadLine() ?? "";

                Console.Write("Введите фамилию: ");
                string lastName = Console.ReadLine() ?? "";

                Console.Write("Введите год рождения: ");
                string input = Console.ReadLine() ?? "";

                if (!int.TryParse(input, out int birthYear))
                {
                    Console.WriteLine("Ошибка: введите корректный год рождения");
                    return;
                }

                profile = new Profile(firstName, lastName, birthYear);
                todoList = new TodoList();
            }
            else
            {
                todoList = FileManager.LoadTodos(todoPath);
                Console.WriteLine($"Профиль загружен: {profile.FirstName} {profile.LastName}, возраст {profile.GetAge()}");
            }

            while (true)
            {
                Console.Write("> ");
                string? inputLine = Console.ReadLine();

                if (inputLine == null || inputLine.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    FileManager.SaveProfile(profile, profilePath);
                    FileManager.SaveTodos(todoList, todoPath);
                    break;
                }

                ICommand? command = CommandParser.Parse(inputLine, todoList, profile);
                if (command != null)
                {
                    command.Execute();
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                }
            }
        }
    }
}