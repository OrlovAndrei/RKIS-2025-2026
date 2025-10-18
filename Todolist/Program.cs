using System;

class Program
{
    private const int InitialCapacity = 2;

    static void Main()
    {
        Console.WriteLine("Работу выполнили: Должиков и Бут, группа 3834");
        Console.WriteLine("Консольный ToDoList — версия с метаданными задач.\n");

        string firstName = Prompt("Введите имя: ");
        string lastName  = Prompt("Введите фамилию: ");
        int birthYear    = ReadInt("Введите год рождения: ");
        int age = DateTime.Now.Year - birthYear;
        Console.WriteLine($"\nПрофиль создан: {firstName} {lastName}, возраст – {age}\n");

        // Основные массивы, изменяемые синхронно
        string[] todos    = new string[InitialCapacity];
        bool[] statuses   = new bool[InitialCapacity];
        DateTime[] dates  = new DateTime[InitialCapacity];
        int taskCount = 0;

        Console.WriteLine("Введите команду (help для списка команд).");

        while (true)
        {
            Console.Write("\n>>> ");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                continue;

            string command = ExtractCommand(input);

            if (command == "help")
            {
                PrintHelp();
                continue;
            }

            if (command == "profile")
            {
                PrintProfile(firstName, lastName, birthYear);
                continue;
            }

            if (command == "add")
            {
                HandleAdd(ref todos, ref statuses, ref dates, ref taskCount, input);
                continue;
            }

            if (command == "view")
            {
                HandleView(todos, statuses, dates, taskCount);
                continue;
            }

            if (command == "exit")
            {
                Console.WriteLine("До свидания!");
                break;
            }

            Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
        }
    }

    // --- Утилиты ---
    static string Prompt(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (int.TryParse(s, out int v))
                return v;
            Console.WriteLine("Неверный ввод. Попробуйте ещё раз.");
        }
    }

    static string ExtractCommand(string input)
    {
        string[] parts = input.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        return parts.Length > 0 ? parts[0].ToLower() : string.Empty;
    }

    static void PrintHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine(" help                 — список команд");
        Console.WriteLine(" profile              — показать профиль пользователя");
        Console.WriteLine(" add \"текст\"          — добавить задачу");
        Console.WriteLine(" view                 — показать задачи (индекс, текст, сделано/не сделано, дата)");
        Console.WriteLine(" exit                 — выйти");
    }

    static void PrintProfile(string firstName, string lastName, int birthYear)
    {
        Console.WriteLine($"{firstName} {lastName}, {birthYear} г.р.");
    }

    // --- Команды: add / view ---
    static void HandleAdd(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int taskCount, string input)
    {
        string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            Console.WriteLine("Ошибка: укажите текст задачи. Пример: add \"Сделать задание\"");
            return;
        }

        string taskText = parts[1].Trim().Trim('"');

        if (taskCount >= todos.Length)
        {
            ExpandAllArrays(ref todos, ref statuses, ref dates);
        }

        todos[taskCount] = taskText;
        statuses[taskCount] = false;
        dates[taskCount] = DateTime.Now;
        taskCount++;

        Console.WriteLine($"Задача добавлена: \"{taskText}\"");
    }

    static void HandleView(string[] todos, bool[] statuses, DateTime[] dates, int taskCount)
    {
        Console.WriteLine("Ваши задачи:");
        if (taskCount == 0)
        {
            Console.WriteLine(" (список пуст)");
            return;
        }

        for (int i = 0; i < taskCount; i++)
        {
            string doneText = statuses[i] ? "сделано" : "не сделано";
            Console.WriteLine($"{i + 1}. {todos[i]} [{doneText}] ({dates[i]})");
        }
    }

    // --- Общая логика расширения массивов синхронно ---
    static void ExpandAllArrays(ref string[] todos, ref bool[] statuses, ref DateTime[] dates)
    {
        int newSize = todos.Length * 2;

        string[] newTodos = new string[newSize];
        bool[] newStatuses = new bool[newSize];
        DateTime[] newDates = new DateTime[newSize];

        for (int i = 0; i < todos.Length; i++)
        {
            newTodos[i] = todos[i];
            newStatuses[i] = statuses[i];
            newDates[i] = dates[i];
        }

        todos = newTodos;
        statuses = newStatuses;
        dates = newDates;
    }
}
