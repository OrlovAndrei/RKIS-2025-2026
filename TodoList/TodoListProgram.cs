using System;
using System.Text;

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
                        HandleAdd(todos, input);
                        break;
                    case "view":
                        HandleView(todos);
                        break;
                    case "profile":
                        HandleProfile(profile);
                        break;
                    case "done":
                        HandleDone(todos, input);
                        break;
                    case "delete":
                        HandleDelete(todos, input);
                        break;
                    case "update":
                        HandleUpdate(todos, input);
                        break;
                    case "read":
                        HandleRead(todos, input);
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
            Console.WriteLine("help                   — список доступных команд");
            Console.WriteLine("profile                — вывод данных пользователя");
            Console.WriteLine("add \"текст\"          — добавить новую задачу");
            Console.WriteLine("view                   — показать все задачи (таблица)");
            Console.WriteLine("done <idx>             — отметить задачу выполненной");
            Console.WriteLine("delete <idx>           — удалить задачу");
            Console.WriteLine("update <idx> \"текст\"   — обновить текст задачи");
            Console.WriteLine("read <idx>             — показать полную информацию о задаче");
            Console.WriteLine("exit                   — выход из программы");
        }

        private static void HandleProfile(Profile profile)
        {
            Console.WriteLine(profile.GetInfo());
        }

        private static void HandleAdd(TodoList todos, string input)
        {
            if (!TryParseQuotedText(input, out string? taskText) || string.IsNullOrWhiteSpace(taskText))
            {
                Console.WriteLine("Некорректный формат. Используйте: add \"текст задачи\"");
                return;
            }

            var item = new TodoItem(taskText.Trim());
            todos.Add(item);

            Console.WriteLine($"Добавлена задача: \"{taskText.Trim()}\"");
        }

        private static void HandleView(TodoList todos)
        {
            todos.View(showIndex: true, showDone: true, showDate: true);
        }

        private static void HandleDone(TodoList todos, string input)
        {
            if (!TryParseIndex(input, todos.Count, out int idx))
                return;

            TodoItem item = todos.GetItem(idx - 1);
            item.MarkDone();

            Console.WriteLine($"Задача {idx} отмечена как выполненная.");
        }

        private static void HandleDelete(TodoList todos, string input)
        {
            if (!TryParseIndex(input, todos.Count, out int idx))
                return;

            todos.Delete(idx - 1);
            Console.WriteLine($"Задача {idx} удалена.");
        }

        private static void HandleUpdate(TodoList todos, string input)
        {
            // Ожидается формат: update <idx> "new_text"
            var parts = input.Split('"');
            if (parts.Length < 2)
            {
                Console.WriteLine("Некорректный формат. Используйте: update <idx> \"новый текст\"");
                return;
            }

            string left = parts[0].Trim();
            var leftParts = left.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (leftParts.Length < 2 || !int.TryParse(leftParts[1], out int idx) || idx < 1 || idx > todos.Count)
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

            TodoItem item = todos.GetItem(idx - 1);
            item.UpdateText(newText);

            Console.WriteLine($"Задача {idx} обновлена.");
        }

        private static void HandleRead(TodoList todos, string input)
        {
            if (!TryParseIndex(input, todos.Count, out int idx))
                return;

            TodoItem item = todos.GetItem(idx - 1);
            Console.WriteLine(item.GetFullInfo());
        }

        private static bool TryParseIndex(string input, int max, out int idx)
        {
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
            var parts = input.Split('"');
            if (parts.Length >= 2)
            {
                text = parts[1];
                return true;
            }

            text = null;
            return false;
        }
    }
}
