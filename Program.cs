using System;

class Program
{
    private const int InitialCapacity = 2;

    static void Main()
    {
        Console.WriteLine("Работу выполнили: Должиков и Бут, группа 3834");
        Console.WriteLine("Консольный ToDoList — полнофункциональная версия.\n");

        string firstName = Prompt("Введите имя: ") ?? string.Empty;
        string lastName  = Prompt("Введите фамилию: ") ?? string.Empty;
        int birthYear    = ReadInt("Введите год рождения: ");
        int age = DateTime.Now.Year - birthYear;
        Console.WriteLine($"\nПрофиль создан: {firstName} {lastName}, возраст – {age}\n");

        // Три синхронных массива
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

            string[] parts = input.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string command = parts.Length > 0 ? parts[0].ToLower() : string.Empty;
            string args = parts.Length > 1 ? parts[1] : string.Empty;

            switch (command)
            {
                case "help":
                    PrintHelp();
                    break;

                case "profile":
                    PrintProfile(firstName, lastName, birthYear);
                    break;

                case "add":
                    HandleAdd(ref todos, ref statuses, ref dates, ref taskCount, args);
                    break;

                case "view":
                    ViewTasks(todos, statuses, dates, taskCount, args);
                    break;

                case "done":
                    HandleDone(ref statuses, ref dates, taskCount, args);
                    break;

                case "delete":
                    HandleDelete(ref todos, ref statuses, ref dates, ref taskCount, args);
                    break;

                case "update":
                    HandleUpdate(ref todos, ref dates, taskCount, args);
                    break;

                case "exit":
                    Console.WriteLine("До свидания!");
                    return;

                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                    break;
            }
        }
    }

    // --- Вспомогательные методы ввода/валидации ---
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

    static void PrintHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine(" help                     — список команд");
        Console.WriteLine(" profile                  — показать профиль пользователя");
        Console.WriteLine(" add \"текст\"              — добавить задачу (однострочно)");
        Console.WriteLine(" add --multiline / -m     — добавить задачу (многострочно, завершить ввод командой !end)");
        Console.WriteLine(" view                     — показать задачи (по умолчанию — только текст)");
        Console.WriteLine(" done <idx>               — отметить задачу выполненной (idx — номер задачи)");
        Console.WriteLine(" delete <idx>             — удалить задачу по индексу");
        Console.WriteLine(" update <idx> \"новый\"     — обновить текст задачи");
        Console.WriteLine(" exit                     — выйти");
    }

    static void PrintProfile(string firstName, string lastName, int birthYear)
    {
        Console.WriteLine($"{firstName} {lastName}, {birthYear} г.р.");
    }

    // --- Команда add (обёртка) ---
    static void HandleAdd(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int taskCount, string args)
    {
        AddTask(ref todos, ref statuses, ref dates, ref taskCount, args);
    }

    // --- Команда add: поддержка многострочного режима ---
    static void AddTask(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int taskCount, string args)
    {
        string localArgs = args ?? string.Empty;

        bool multiline = false;
        // Определим, указан ли флаг многострочного ввода
        if (localArgs.Contains("--multiline", StringComparison.OrdinalIgnoreCase) ||
            localArgs.Contains("-m", StringComparison.OrdinalIgnoreCase))
        {
            multiline = true;
        }

        if (multiline)
        {
            Console.WriteLine("Многострочный режим. Введите строки задачи. Введите '!end' на отдельной строке чтобы завершить.");
            string line;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            while (true)
            {
                line = Console.ReadLine();
                if (line != null && line.Trim() == "!end")
                    break;
                if (line != null)
                {
                    if (sb.Length > 0)
                        sb.Append('\n');
                    sb.Append(line);
                }
            }

            string taskText = sb.ToString();

            if (taskCount >= todos.Length)
                ExpandAll(ref todos, ref statuses, ref dates);

            todos[taskCount] = taskText;
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
            taskCount++;

            Console.WriteLine("Многострочная задача добавлена.");
            return;
        }

        // Обычный однострочный режим
        if (string.IsNullOrWhiteSpace(localArgs))
        {
            Console.WriteLine("Ошибка: укажите текст задачи. Пример: add \"Сделать задание\"");
            return;
        }

        string taskTextSingle = localArgs.Trim().Trim('"');

        if (taskCount >= todos.Length)
            ExpandAll(ref todos, ref statuses, ref dates);

        todos[taskCount] = taskTextSingle;
        statuses[taskCount] = false;
        dates[taskCount] = DateTime.Now;
        taskCount++;

        Console.WriteLine($"Задача добавлена: \"{taskTextSingle}\"");
    }

    // --- Команда view (пока базовая, показывает текст) ---
    static void ViewTasks(string[] todos, bool[] statuses, DateTime[] dates, int taskCount, string args)
    {
        Console.WriteLine("Ваши задачи:");
        if (taskCount == 0)
        {
            Console.WriteLine(" (список пуст)");
            return;
        }

        for (int i = 0; i < taskCount; i++)
        {
            string text = todos[i] ?? string.Empty;
            Console.WriteLine($"{i + 1}. {text}");
        }
    }

    // --- Команда done <idx> ---
    static void HandleDone(ref bool[] statuses, ref DateTime[] dates, int taskCount, string args)
    {
        MarkDone(ref statuses, ref dates, taskCount, args);
    }

    static void MarkDone(ref bool[] statuses, ref DateTime[] dates, int taskCount, string args)
    {
        if (!TryParseIndex(args, taskCount, out int indexZeroBased))
            return;

        statuses[indexZeroBased] = true;
        dates[indexZeroBased] = DateTime.Now;
        Console.WriteLine($"Задача {indexZeroBased + 1} отмечена как выполненная.");
    }

    // --- Команда delete <idx> ---
    static void HandleDelete(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int taskCount, string args)
    {
        DeleteTask(ref todos, ref statuses, ref dates, ref taskCount, args);
    }

    static void DeleteTask(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int taskCount, string args)
    {
        if (!TryParseIndex(args, taskCount, out int indexZeroBased))
            return;

        for (int i = indexZeroBased; i < taskCount - 1; i++)
        {
            todos[i] = todos[i + 1];
            statuses[i] = statuses[i + 1];
            dates[i] = dates[i + 1];
        }

        todos[taskCount - 1] = null;
        statuses[taskCount - 1] = default;
        dates[taskCount - 1] = default;

        taskCount--;
        Console.WriteLine($"Задача {indexZeroBased + 1} удалена.");
    }

    // --- Команда update <idx> "new_text" ---
    static void HandleUpdate(ref string[] todos, ref DateTime[] dates, int taskCount, string args)
    {
        UpdateTask(ref todos, ref dates, taskCount, args);
    }

    static void UpdateTask(ref string[] todos, ref DateTime[] dates, int taskCount, string args)
    {
        if (string.IsNullOrWhiteSpace(args))
        {
            Console.WriteLine("Ошибка: укажите индекс и новый текст. Пример: update 2 \"Новый текст\"");
            return;
        }

        string[] parts = args.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            Console.WriteLine("Ошибка: неверный формат. Пример: update 2 \"Новый текст\"");
            return;
        }

        if (!int.TryParse(parts[0], out int idxOneBased))
        {
            Console.WriteLine("Ошибка: индекс должен быть числом.");
            return;
        }

        int indexZeroBased = idxOneBased - 1;
        if (indexZeroBased < 0 || indexZeroBased >= taskCount)
        {
            Console.WriteLine("Ошибка: индекс вне диапазона.");
            return;
        }

        string newText = parts[1].Trim().Trim('"');
        todos[indexZeroBased] = newText;
        dates[indexZeroBased] = DateTime.Now;
        Console.WriteLine($"Задача {idxOneBased} обновлена.");
    }

    // --- Вспомогательные: расширение и парсинг индекса ---
    static void ExpandAll(ref string[] todos, ref bool[] statuses, ref DateTime[] dates)
    {
        int newSize = Math.Max(2, todos.Length * 2);
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

    static bool TryParseIndex(string arg, int taskCount, out int indexZeroBased)
    {
        indexZeroBased = -1;
        if (string.IsNullOrWhiteSpace(arg))
        {
            Console.WriteLine("Ошибка: укажите индекс задачи.");
            return false;
        }

        if (!int.TryParse(arg.Trim(), out int idxOneBased))
        {
            Console.WriteLine("Ошибка: индекс должен быть числом.");
            return false;
        }

        indexZeroBased = idxOneBased - 1;
        if (indexZeroBased < 0 || indexZeroBased >= taskCount)
        {
            Console.WriteLine("Ошибка: индекс вне диапазона.");
            return false;
        }

        return true;
    }
}
