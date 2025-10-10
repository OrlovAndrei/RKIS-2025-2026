using System;
using System.Text;

namespace TodoList
{
    internal class Program
    {
        // Константы
        private const int InitialCapacity = 2;
        private const string Prompt = "> ";

        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

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

            int currentYear = DateTime.Now.Year;
            int userAge = currentYear - userBirthYear;

            userFirstName = string.IsNullOrWhiteSpace(userFirstName) ? "Имя" : userFirstName.Trim();
            userLastName = string.IsNullOrWhiteSpace(userLastName) ? "Фамилия" : userLastName.Trim();

            Console.WriteLine($"Добавлен пользователь {userFirstName} {userLastName}, возраст - {userAge}");

            // Данные задач
            string[] todos = new string[InitialCapacity];
            bool[] statuses = new bool[InitialCapacity];
            DateTime[] dates = new DateTime[InitialCapacity];
            int todosCount = 0;

            Console.WriteLine("Введите 'help' чтобы увидеть список команд.");
            while (true)
            {
                Console.Write(Prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                string command = input.Trim();
                string verb = command.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();

                switch (verb)
                {
                    case "help":
                        PrintHelp();
                        break;
                    case "exit":
                        Console.WriteLine("Выход...");
                        return;
                    case "add":
                        HandleAdd(ref todos, ref statuses, ref dates, ref todosCount, input);
                        break;
                    case "view":
                        HandleView(todos, statuses, dates, todosCount);
                        break;
                    case "profile":
                        HandleProfile(userFirstName!, userLastName!, userBirthYear);
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                        break;
                }
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help   — список доступных команд");
            Console.WriteLine("profile— вывод данных пользователя");
            Console.WriteLine("add \"текст задачи\" — добавить новую задачу");
            Console.WriteLine("view   — показать все задачи");
            Console.WriteLine("exit   — выход из программы");
        }

        private static void HandleProfile(string firstName, string lastName, int birthYear)
        {
            Console.WriteLine($"{firstName} {lastName}, {birthYear}");
        }

        private static void HandleAdd(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int count, string input)
        {
            if (!TryParseQuotedText(input, out string? taskText) || string.IsNullOrWhiteSpace(taskText))
            {
                Console.WriteLine("Некорректный формат. Используйте: add \"текст задачи\"");
                return;
            }

            EnsureCapacity(ref todos, ref statuses, ref dates, count);
            todos[count] = taskText.Trim();
            statuses[count] = false;
            dates[count] = DateTime.Now;
            count++;
            Console.WriteLine($"Добавлена задача: \"{taskText.Trim()}\"");
        }

        private static void HandleView(string[] todos, bool[] statuses, DateTime[] dates, int count)
        {
            if (count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            Console.WriteLine("Задачи:");
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrWhiteSpace(todos[i]))
                {
                    string statusText = statuses[i] ? "сделано" : "не сделано";
                    Console.WriteLine($"{i + 1} {todos[i]} {statusText} {dates[i]:yyyy-MM-dd HH:mm:ss}");
                }
            }
        }

        private static bool TryParseQuotedText(string input, out string? text)
        {
            // Поиск в кавычках: команда "текст"
            var parts = input.Split('"');
            if (parts.Length >= 2)
            {
                text = parts[1];
                return true;
            }

            text = null;
            return false;
        }

        private static void EnsureCapacity(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, int count)
        {
            if (count < todos.Length)
            {
                return;
            }

            int currentLength = todos.Length;
            int newLength = currentLength == 0 ? InitialCapacity : currentLength * 2;

            // Todos
            string[] biggerTodos = new string[newLength];
            for (int i = 0; i < currentLength; i++)
            {
                biggerTodos[i] = todos[i];
            }
            todos = biggerTodos;

            // Statuses
            bool[] biggerStatuses = new bool[newLength];
            for (int i = 0; i < currentLength; i++)
            {
                biggerStatuses[i] = statuses[i];
            }
            statuses = biggerStatuses;

            // Dates
            DateTime[] biggerDates = new DateTime[newLength];
            for (int i = 0; i < currentLength; i++)
            {
                biggerDates[i] = dates[i];
            }
            dates = biggerDates;
        }
    }
}
