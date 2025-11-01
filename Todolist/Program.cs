using System;

class Program
{

    static void Main()
    {
        Console.WriteLine("Работу выполнили: Должиков и Бут, группа 3834");
        Console.WriteLine("Консольный ToDoList — полнофункциональная версия.\n");

        string firstName = Prompt("Введите имя: ") ?? string.Empty;
        string lastName  = Prompt("Введите фамилию: ") ?? string.Empty;
        int birthYear    = ReadInt("Введите год рождения: ");
        
        Profile profile = new Profile(firstName, lastName, birthYear);
        Console.WriteLine($"\nПрофиль создан: {profile.GetInfo()}\n");

        TodoList todoList = new TodoList();

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
                    Console.WriteLine(profile.GetInfo());
                    break;

                case "add":
                    HandleAdd(todoList, args);
                    break;

                case "view":
                    HandleView(todoList, args);
                    break;

                case "read":
                    HandleRead(todoList, args);
                    break;

                case "done":
                    HandleDone(todoList, args);
                    break;

                case "delete":
                    HandleDelete(todoList, args);
                    break;

                case "update":
                    HandleUpdate(todoList, args);
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
        Console.WriteLine(" help                         — список команд");
        Console.WriteLine(" profile                      — показать профиль пользователя");
        Console.WriteLine(" add \"текст\"                  — добавить задачу (однострочно)");
        Console.WriteLine(" add --multiline / -m         — добавить задачу (многострочно, завершить ввод командой !end)");
        Console.WriteLine(" view [flags]                 — показать задачи (по умолчанию — только текст)");
        Console.WriteLine("    Flags:");
        Console.WriteLine("      --index, -i       — показывать индекс задачи");
        Console.WriteLine("      --status, -s      — показывать статус задачи");
        Console.WriteLine("      --update-date, -d — показывать дату последнего изменения");
        Console.WriteLine("      --all, -a         — показывать все поля одновременно");
        Console.WriteLine(" read <idx>                   — показать полный текст задачи, статус и дату изменения");
        Console.WriteLine(" done <idx>                   — отметить задачу выполненной (idx — номер задачи)");
        Console.WriteLine(" delete <idx>                 — удалить задачу по индексу");
        Console.WriteLine(" update <idx> \"новый\"         — обновить текст задачи");
        Console.WriteLine(" exit                         — выйти");
    }

    // --- Команда add ---
    static void HandleAdd(TodoList todoList, string args)
    {
        string localArgs = args ?? string.Empty;

        bool multiline = false;
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
            TodoItem item = new TodoItem(taskText);
            todoList.Add(item);

            Console.WriteLine("Многострочная задача добавлена.");
            return;
        }

        if (string.IsNullOrWhiteSpace(localArgs))
        {
            Console.WriteLine("Ошибка: укажите текст задачи. Пример: add \"Сделать задание\"");
            return;
        }

        string taskTextSingle = localArgs.Trim().Trim('"');
        TodoItem itemSingle = new TodoItem(taskTextSingle);
        todoList.Add(itemSingle);

        Console.WriteLine($"Задача добавлена: \"{taskTextSingle}\"");
    }

    // --- Команда view с флагами и табличным выводом ---
    static void HandleView(TodoList todoList, string args)
    {
        bool showIndex = false;
        bool showStatus = false;
        bool showDate = false;

        string localArgs = args ?? string.Empty;
        string argsLower = localArgs.ToLowerInvariant();

        if (localArgs.Contains("--all", StringComparison.OrdinalIgnoreCase) ||
            argsLower.Contains("-a"))
        {
            showIndex = showStatus = showDate = true;
        }
        else
        {
            // Обработка комбинаций сокращенных флагов: -is, -ds, -dis
            // Проверяем комбинации всех трех флагов: -dis, -ids, -dsi
            if (argsLower.Contains("-dis") || argsLower.Contains("-ids") || argsLower.Contains("-dsi"))
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }
            else
            {
                // Комбинация двух флагов: -is (index + status)
                if (argsLower.Contains("-is"))
                {
                    showIndex = true;
                    showStatus = true;
                }
                
                // Комбинация двух флагов: -ds (status + date)
                if (argsLower.Contains("-ds"))
                {
                    showStatus = true;
                    showDate = true;
                }
            }

            // Обработка отдельных флагов (проверяем как полные, так и сокращенные)
            if (localArgs.Contains("--index", StringComparison.OrdinalIgnoreCase) || argsLower.Contains("-i"))
                showIndex = true;
                
            if (localArgs.Contains("--status", StringComparison.OrdinalIgnoreCase) || argsLower.Contains("-s"))
                showStatus = true;
                
            if (localArgs.Contains("--update-date", StringComparison.OrdinalIgnoreCase) || argsLower.Contains("-d"))
                showDate = true;
        }

        todoList.View(showIndex, showStatus, showDate);
    }

    // --- Команда read <idx> ---
    static void HandleRead(TodoList todoList, string args)
    {
        if (!TryParseIndex(args, todoList.Count, out int indexOneBased))
            return;

        try
        {
            todoList.Read(indexOneBased);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    // --- Команда done <idx> ---
    static void HandleDone(TodoList todoList, string args)
    {
        if (!TryParseIndex(args, todoList.Count, out int indexOneBased))
            return;

        try
        {
            TodoItem item = todoList.GetItem(indexOneBased);
            item.MarkDone();
            Console.WriteLine($"Задача {indexOneBased} отмечена как выполненная.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    // --- Команда delete <idx> ---
    static void HandleDelete(TodoList todoList, string args)
    {
        if (!TryParseIndex(args, todoList.Count, out int indexOneBased))
            return;

        try
        {
            todoList.Delete(indexOneBased);
            Console.WriteLine($"Задача {indexOneBased} удалена.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    // --- Команда update <idx> "new_text" ---
    static void HandleUpdate(TodoList todoList, string args)
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

        if (idxOneBased < 1 || idxOneBased > todoList.Count)
        {
            Console.WriteLine("Ошибка: индекс вне диапазона.");
            return;
        }

        string newText = parts[1].Trim().Trim('"');
        try
        {
            TodoItem item = todoList.GetItem(idxOneBased);
            item.UpdateText(newText);
            Console.WriteLine($"Задача {idxOneBased} обновлена.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    // --- Вспомогательные методы ---
    static bool TryParseIndex(string arg, int taskCount, out int indexOneBased)
    {
        indexOneBased = -1;
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

        indexOneBased = idxOneBased;
        if (indexOneBased < 1 || indexOneBased > taskCount)
        {
            Console.WriteLine("Ошибка: индекс вне диапазона.");
            return false;
        }

        return true;
    }
}
