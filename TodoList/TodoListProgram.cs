using System;

namespace TodoList
{
    internal class Program
    {
        private const string Prompt = "> ";

        private static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Буряк Степан Геннадьевич и Голубев Данил Сергеевич");

            // Ввод профиля пользователя
            Console.Write("Введите имя: ");
            string? userFirstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            string? userLastName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            string? birthYearInput = Console.ReadLine();
            if (!int.TryParse(birthYearInput, out int userBirthYear))
            {
                Console.WriteLine("Некорректный год рождения. Пожалуйста, введите число.");
                return;
            }

            var profile = new Profile(userFirstName ?? string.Empty, userLastName ?? string.Empty, userBirthYear);
            Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");

            var todos = new TodoList();

            Console.WriteLine("Введите 'help' чтобы увидеть список команд.");
            while (true)
            {
                Console.Write(Prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                ICommand command = CommandParser.Parse(input, todos, profile);
                command.Execute();
            }
        }
    }
}
