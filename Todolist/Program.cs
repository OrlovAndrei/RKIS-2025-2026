using System;

class Program
{
    private const int InitialCapacity = 2;

    static void Main()
    {
        Console.WriteLine("Работу выполнили: Должиков и Бут, группа 3834");
        Console.WriteLine("Консольный ToDoList — полнофункциональная версия.\n");

        string firstName = Prompt("Введите имя: ");
        string lastName  = Prompt("Введите фамилию: ");
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
                    AddTask(ref todos, ref statuses, ref dates, ref taskCount, args);
                    break;

                case "view":
                    ViewTasks(todos, statuses, dates, taskCount);
                    break;

                case "done":
                    MarkDone(ref statuses, ref dates, taskCount, args);
                    break;

                case "delete":
                    DeleteTask(ref todos, ref statuses, ref dates, ref taskCount, args);
                    break;

                case "update":
                    UpdateTask(ref todos, ref dates, taskCount, args);
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
        Console.WriteLine(" add \"текст\"              — добавить задачу");
        Console.WriteLine(" view                     — показать задачи (индекс, текст, сделано/не сделано, дата)");
        Console.WriteLine(" done <idx>               — отметить задачу выполненной (idx — номер задачи)");
        Console.WriteLine(" delete <idx>             — удалить задачу по индексу");
        Console.WriteLine(" update <idx> \"новый\"     — обновить текст задачи");
        Console.WriteLine(" exit                     — выйти");
    }

    static void PrintProfile(string firstName, string lastName, int birthYear)
    {
        Console.WriteLine($"{firstName} {lastName}, {birthYear} г.р.");
    }

    // --- Команда add ---
    static void AddTask(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int taskCount, string args)
    {
        if (string.IsNullOrWhiteSpace(args))
        {
            Console.WriteLine("Ошибка: укажите текст задачи. Пример: add \"Сделать задание\"");
            return;
        }

        string taskText = args.Trim().Trim('"');

        if (taskCount >= todos.Length)
            ExpandAll(ref todos, ref statuses, ref dates);

        todos[taskCount] = taskText;
        statuses[taskCount] = false;
        dates[taskCount] = DateTime.Now;
        taskCount++;

        Console.WriteLine($"Задача добавлена: \"{taskText}\"");
    }

    // --- Команда view ---
    static void ViewTasks(string[] todos, bool[] statuses, DateTime[] dates, int taskCount)
    {
        Console.WriteLine("Ваши задачи:");
        if (taskCount == 0)
        {
            Console.WriteLine(" (список пуст)");
            return;
        }

        for (int i = 0; i < taskCount; i++)
        {
            string state = statuses[i] ? "сделано" : "не сделано";
            Console.WriteLine($"{i + 1}. {todos[i]} [{state}] ({dates[i]})");
        }
    }

    // --- Команда done <idx> ---
    static void MarkDone(ref bool[] statuses, ref DateTime[] dates, int taskCount, string args)
    {
        if (!TryParseIndex(args, taskCount, out int indexZeroBased))
            return;

        statuses[indexZeroBased] = true;
        dates[indexZeroBased] = DateTime.Now;
        Console.WriteLine($"Задача {indexZeroBased + 1} отмечена как выполненная.");
    }

    // --- Команда delete <idx> ---
    static void DeleteTask(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int taskCount, string args)
    {
        if (!TryParseIndex(args, taskCount, out int indexZeroBased))
            return;

        // Сдвиг влево всех массивов
        for (int i = indexZeroBased; i < taskCount - 1; i++)
        {
            todos[i] = todos[i + 1];
            statuses[i] = statuses[i + 1];
            dates[i] = dates[i + 1];
        }

        // Очистка последнего элемента
        todos[taskCount - 1] = null;
        statuses[taskCount - 1] = default;
        dates[taskCount - 1] = default;

        taskCount--;
        Console.WriteLine($"Задача {indexZeroBased + 1} удалена.");
    }

    // --- Команда update <idx> "new_text" ---
    static void UpdateTask(ref string[] todos, ref DateTime[] dates, int taskCount, string args)
    {
        if (string.IsNullOrWhiteSpace(args))
        {
            Console.WriteLine("Ошибка: укажите индекс и новый текст. Пример: update 2 \"Новый текст\"");
            return;
        }

        // Разделим аргументы: индекс и остальной текст
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
