using System;

namespace ToddList
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Бурнашов и Хазиев");

            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            string yearInput = Console.ReadLine();

            int birthYear = int.Parse(yearInput);
            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;

            string[] todos = new string[2];

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст – {age}");
            Console.WriteLine("Введите 'help' для списка команд");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (input == null || input.ToLower() == "exit")
                {
                    Console.WriteLine("Выход из программы...");
                    break;
                }

                string command = input.ToLower().Trim();

                if (command == "help")
                {
                    ShowHelp();
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                }
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - вывести список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add \"текст задачи\" - добавить новую задачу");
            Console.WriteLine("view - показать все задачи");
            Console.WriteLine("exit - выйти из программы");
        }
    }
}