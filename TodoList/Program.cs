using System;
using TodoList.Commands;

namespace TodoList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Шелепов и Кузьменко");

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

            var profile = new Profile(firstName, lastName, birthYear);
            var todoList = new TodoList();

            Console.WriteLine($"Профиль создан: {profile.FirstName} {profile.LastName}, возраст {profile.GetAge()}");

            while (true)
            {
                Console.Write("> ");
                string? inputLine = Console.ReadLine();

                if (inputLine == null || inputLine.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

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