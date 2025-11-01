using System;

class Program
{
    private const int InitialCapacity = 2;
    private const int TaskTextMaxDisplay = 30;

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
                    HandleView(todos, statuses, dates, taskCount, args);
                    break;

                case "read":
                    HandleRead(todos, statuses, dates, taskCount, args);
                    break;

                case "done":
                    HandleDone(statuses, dates, taskCount, args);
                    break;

                case "delete":
                    HandleDelete(ref todos, ref statuses, ref dates, ref taskCount, args);
                    break;

                case "update":
                    HandleUpdate(todos, dates, taskCount, args);
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

    static void PrintProfile(string firstName, string lastName, int birthYear)
    {
        Console.WriteLine($"{firstName} {lastName}, {birthYear} г.р.");
    }

    // --- Команда add ---
    static void HandleAdd(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int taskCount, string args)
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

            if (taskCount >= todos.Length)
                ExpandAll(ref todos, ref statuses, ref dates);

            todos[taskCount] = taskText;
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
            taskCount++;

            Console.WriteLine("Многострочная задача добавлена.");
            return;
        }

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

    // --- Команда view с флагами и табличным выводом ---
    static void HandleView(string[] todos, bool[] statuses, DateTime[] dates, int taskCount, string args)
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

        Console.WriteLine("Ваши задачи:");
        if (taskCount == 0)
        {
            Console.WriteLine(" (список пуст)");
            return;
        }

        // Подготовка ширин колонок
        int idxWidth = showIndex ? Math.Max(3, (taskCount.ToString().Length + 1)) : 0; // пример: "1."
        int statusWidth = showStatus ? 10 : 0; // "сделано"/"не сделано"
        int dateWidth = showDate ? 20 : 0; // формат даты
        int textWidth = TaskTextMaxDisplay;

        // Заголовок
        System.Text.StringBuilder header = new System.Text.StringBuilder();
        if (showIndex)
            header.Append(PadCenter("Idx", idxWidth) + " | ");
        header.Append(PadCenter("Text", textWidth));
        if (showStatus)
            header.Append(" | " + PadCenter("Status", statusWidth));
        if (showDate)
            header.Append(" | " + PadCenter("Updated", dateWidth));

        Console.WriteLine(header.ToString());
        Console.WriteLine(new string('-', header.Length));

        // Строки
        for (int i = 0; i < taskCount; i++)
        {
            string text = todos[i] ?? string.Empty;
            string textDisplay = TruncateWithEllipsis(text, textWidth);

            System.Text.StringBuilder row = new System.Text.StringBuilder();
            if (showIndex)
                row.Append((i + 1).ToString().PadRight(idxWidth) + " | ");
            row.Append(textDisplay.PadRight(textWidth));
            if (showStatus)
            {
                string state = statuses[i] ? "сделано" : "не сделано";
                row.Append(" | " + state.PadRight(statusWidth));
            }
            if (showDate)
            {
                string d = dates[i] == default ? "-" : dates[i].ToString("yyyy-MM-dd HH:mm");
                row.Append(" | " + d.PadRight(dateWidth));
            }

            Console.WriteLine(row.ToString());
        }
    }

    static string TruncateWithEllipsis(string s, int max)
    {
        if (s == null) return new string(' ', max);
        if (s.Length <= max) return s;
        if (max <= 3) return s.Substring(0, max);
        return s.Substring(0, max - 3) + "...";
    }

    static string PadCenter(string text, int width)
    {
        if (width <= 0) return string.Empty;
        if (text == null) text = string.Empty;
        if (text.Length >= width) return text.Substring(0, width);
        int left = (width - text.Length) / 2;
        int right = width - text.Length - left;
        return new string(' ', left) + text + new string(' ', right);
    }

    // --- Команда read <idx> ---
    static void HandleRead(string[] todos, bool[] statuses, DateTime[] dates, int taskCount, string args)
    {
        if (!TryParseIndex(args, taskCount, out int indexZeroBased))
            return;

        string text = todos[indexZeroBased] ?? string.Empty;
        string statusText = statuses[indexZeroBased] ? "выполнена" : "не выполнена";
        string dateText = dates[indexZeroBased] == default ? "-" : dates[indexZeroBased].ToString("yyyy-MM-dd HH:mm");

        Console.WriteLine($"Задача {indexZeroBased + 1}:");
        Console.WriteLine("-----------");
        Console.WriteLine(text);
        Console.WriteLine("-----------");
        Console.WriteLine($"Статус: {statusText}");
        Console.WriteLine($"Дата последнего изменения: {dateText}");
    }

    // --- Команда done <idx> ---
    static void HandleDone(bool[] statuses, DateTime[] dates, int taskCount, string args)
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
    static void HandleUpdate(string[] todos, DateTime[] dates, int taskCount, string args)
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
