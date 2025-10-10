using System;
using System.Text;

namespace TodoList
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Работу выполнили Буряк Степан Геннадьевич и Голубев Данил Сергеевич");

            Console.Write("Введите имя: ");
            string? firstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            string? lastName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            string? birthYearInput = Console.ReadLine();

            if (!int.TryParse(birthYearInput, out int birthYear))
            {
                Console.WriteLine("Некорректный год рождения. Пожалуйста, введите число.");
                return;
            }

            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;

            firstName = string.IsNullOrWhiteSpace(firstName) ? "Имя" : firstName.Trim();
            lastName = string.IsNullOrWhiteSpace(lastName) ? "Фамилия" : lastName.Trim();

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

            // Инициализация массива задач и бесконечный цикл команд
            string[] todos = new string[2];
            int todosCount = 0;

            Console.WriteLine("Введите 'help' чтобы увидеть список команд.");

            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();
                if (input == null)
                {
                    continue;
                }

                string command = input.Trim();
                if (command.Length == 0)
                {
                    continue;
                }

                string verb = command.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();

                if (verb == "help")
                {
                    Console.WriteLine("Доступные команды:");
                    Console.WriteLine("help   — список доступных команд");
                    Console.WriteLine("profile— вывод данных пользователя");
                    Console.WriteLine("add \"текст задачи\" — добавить новую задачу");
                    Console.WriteLine("view   — показать все задачи");
                    Console.WriteLine("exit   — выход из программы");
                    continue;
                }

                if (verb == "exit")
                {
                    Console.WriteLine("Выход...");
                    break;
                }

                // Остальные команды будут добавлены далее
                Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
            }
        }
    }
}
