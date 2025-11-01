using System;

namespace TodoList
{
    class Program
    {
        private static Profile profile;
        private static TodoList todos = new();

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Бурнашов и Хазиев");
            profile = CommandParser.CreateUser();

            Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");
            Console.WriteLine("Введите 'help' для списка команд");

            while (true)
            {
                Console.WriteLine("Введите команду: ");
                string input = Console.ReadLine();

                ICommand command = CommandParser.Parse(input, todos, profile);
                command.Execute();
            }
        }
    }
}
