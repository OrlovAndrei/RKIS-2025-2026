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
                if (input == null)
                    continue;
                if (string.IsNullOrWhiteSpace(input))
                    continue;

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
                        HandleAdd(todos, statuses, dates, ref todosCount, input);
                        break;
                    case "view":
                        HandleView(todos, statuses, dates, todosCount);
                        break;
                    case "profile":
                        HandleProfile(userFirstName!, userLastName!, userBirthYear);
                        break;
                    case "done":
                        HandleDone(statuses, dates, todosCount, input);
                        break;
                    case "delete":
                        HandleDelete(todos, statuses, dates, ref todosCount, input);
                        break;
                    case "update":
                        HandleUpdate(todos, dates, todosCount, input);
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
            Console.WriteLine("help                 — список доступных команд");
            Console.WriteLine("profile              — вывод данных пользователя");
            Console.WriteLine("add \"текст\"        — добавить новую задачу");
            Console.WriteLine("view                 — показать все задачи");
            Console.WriteLine("done <idx>           — отметить задачу выполненной");
            Console.WriteLine("delete <idx>         — удалить задачу");
            Console.WriteLine("update <idx> \"текст\" — обновить текст задачи");
            Console.WriteLine("exit                 — выход из программы");
        }

        private static void HandleProfile(string firstName, string lastName, int birthYear)
        {
            Console.WriteLine($"{firstName} {lastName}, {birthYear}");
        }

        private static void HandleAdd(string[] todos, bool[] statuses, DateTime[] dates, ref int count, string input)
        {
            if (!TryParseQuotedText(input, out string? taskText) || string.IsNullOrWhiteSpace(taskText))
            {
                Console.WriteLine("Некорректный формат. Используйте: add \"текст задачи\"");
                return;
            }

            EnsureCapacity(todos, statuses, dates, count);
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

        private static void HandleDone(bool[] statuses, DateTime[] dates, int count, string input)
        {
            if (!TryParseIndex(input, out int idx, count))
                return;
            int i = idx - 1;
            statuses[i] = true;
            dates[i] = DateTime.Now;
            Console.WriteLine($"Задача {idx} отмечена как выполненная.");
        }

        private static void HandleDelete(string[] todos, bool[] statuses, DateTime[] dates, ref int count, string input)
        {
            if (!TryParseIndex(input, out int idx, count))
                return;
            int i = idx - 1;
            // Сдвиг всех элементов влево, начиная с i+1
            for (int j = i; j < count - 1; j++)
            {
                todos[j] = todos[j + 1];
                statuses[j] = statuses[j + 1];
                dates[j] = dates[j + 1];
            }
            // Очистка последнего (лишнего) места после сдвига не обязательна, но сделаем для порядка
            todos[count - 1] = string.Empty;
            statuses[count - 1] = false;
            dates[count - 1] = default;
            count--;
            Console.WriteLine($"Задача {idx} удалена.");
        }

        private static void HandleUpdate(string[] todos, DateTime[] dates, int count, string input)
        {
            // Ожидается формат: update <idx> "new_text"
            // Сначала выделим индекс до первой кавычки
            var parts = input.Split('"');
            if (parts.Length < 2)
            {
                Console.WriteLine("Некорректный формат. Используйте: update <idx> \"новый текст\"");
                return;
            }

            // Левая часть до кавычки содержит команду и индекс
            string left = parts[0].Trim();
            var leftParts = left.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (leftParts.Length < 2 || !int.TryParse(leftParts[1], out int idx) || idx < 1 || idx > count)
            {
                Console.WriteLine("Некорректный индекс. Используйте: update <idx> \"новый текст\"");
                return;
            }

            string newText = parts[1].Trim();
            if (string.IsNullOrWhiteSpace(newText))
            {
                Console.WriteLine("Пустой текст. Используйте: update <idx> \"новый текст\"");
                return;
            }

            int i = idx - 1;
            todos[i] = newText;
            dates[i] = DateTime.Now;
            Console.WriteLine($"Задача {idx} обновлена.");
        }

        private static bool TryParseIndex(string input, out int idx, int max)
        {
            // Ожидается: команда <idx>
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2 || !int.TryParse(parts[1], out idx) || idx < 1 || idx > max)
            {
                Console.WriteLine("Некорректный индекс. Используйте: <команда> <idx>");
                idx = -1;
                return false;
            }
            return true;
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

        private static void EnsureCapacity(string[] todos, bool[] statuses, DateTime[] dates, int count)
        {
            if (count < todos.Length)
                return;

            int currentLength = todos.Length;
            int newLength = currentLength == 0 ? InitialCapacity : currentLength * 2;

            string[] biggerTodos = new string[newLength];
            bool[] biggerStatuses = new bool[newLength];
            DateTime[] biggerDates = new DateTime[newLength];

            for (int i = 0; i < currentLength; i++)
            {
                biggerTodos[i] = todos[i];
                biggerStatuses[i] = statuses[i];
                biggerDates[i] = dates[i];
            }

            Array.Copy(biggerTodos, todos, newLength);
            Array.Copy(biggerStatuses, statuses, newLength);
            Array.Copy(biggerDates, dates, newLength);
        }
    }
}