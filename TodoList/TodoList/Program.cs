using System;

class Program
{
    const int INITIAL_SIZE = 10;

    static string[] todos = new string[INITIAL_SIZE];
    static bool[] statuses = new bool[INITIAL_SIZE];
    static DateTime[] dates = new DateTime[INITIAL_SIZE];
    static int count = 0;

    static void Main()
    {
        Console.WriteLine("TodoList запущен. Введите help для списка команд.");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            string[] parts = input.Split(' ');
            string command = parts[0];

            string[] flags = GetFlags(parts);

            switch (command)
            {
                case "add":
                    AddCommand(parts, flags);
                    break;

                case "view":
                    ViewCommand(flags);
                    break;

                case "done":
                    DoneCommand(parts);
                    break;

                case "delete":
                    DeleteCommand(parts);
                    break;

                case "update":
                    UpdateCommand(parts);
                    break;

                case "read":
                    ReadCommand(parts);
                    break;

                case "help":
                    HelpCommand();
                    break;

                default:
                    Console.WriteLine("Неизвестная команда. Введите help.");
                    break;
            }
        }
    }

    //===============================================================
    //  ADD
    //===============================================================
    static void AddCommand(string[] parts, string[] flags)
    {
        bool multiline = HasFlag(flags, "--multiline") || HasFlag(flags, "-m");

        if (multiline)
        {
            Console.WriteLine("Введите строки задачи. Пустая строка или !end - завершить ввод");

            string result = "";
            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line) || line == "!end")
                break;

                result += line + "\n";
            }

            Add(result.TrimEnd('\n'));
        }
        else
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Используйте: add <текст задачи>");
                return;
            }

            Add(parts[1]);
        }
    }

    static void Add(string text)
    {
        EnsureSize();

        todos[count] = text;
        statuses[count] = false;
        dates[count] = DateTime.Now;

        count++;
        Console.WriteLine("Задача добавлена.");
    }

    //===============================================================
    //  VIEW
    //===============================================================
    static void ViewCommand(string[] flags)
    {
        if (count == 0)
        {
            Console.WriteLine("Список пуст.");
            return;
        }

        bool showIndex = HasFlag(flags, "--index") || HasFlag(flags, "-i");
        bool showStatus = HasFlag(flags, "--status") || HasFlag(flags, "-s");
        bool showDate = HasFlag(flags, "--update-date") || HasFlag(flags, "-d");
        bool showAll = HasFlag(flags, "--all") || HasFlag(flags, "-a");

        if (showAll)
        {
            showIndex = showStatus = showDate = true;
        }

        // шапка
        Console.WriteLine("------------------------------------------");
        Console.WriteLine($"Всего задач: {count}");
        Console.Write("| ");

        if (showIndex) Console.Write("ID | ");
        if (showStatus) Console.Write("Статус     | ");
        if (showDate) Console.Write("Обновлено          | ");

        Console.WriteLine("Текст задачи");
        Console.WriteLine("------------------------------------------");

        for (int i = 0; i < count; i++)
        {
            Console.Write("| ");

            if (showIndex) Console.Write($"{i,2} | ");

            if (showStatus)
            {
                string s = statuses[i] ? "выполнена" : "не выполнена";
                Console.Write($"{s,-11} | ");
            }

            if (showDate)
            {
                Console.Write($"{dates[i]:yyyy-MM-dd HH:mm} | ");
            }

            string text = todos[i].Length > 30 ? todos[i].Substring(0, 30) + "..." : todos[i];
            Console.WriteLine(text);
        }

        Console.WriteLine("------------------------------------------");
    }

    //===============================================================
    //  DONE
    //===============================================================
    static void DoneCommand(string[] parts)
    {
        if (!TryGetIndex(parts, out int index)) return;

        statuses[index] = true;
        dates[index] = DateTime.Now;

        Console.WriteLine("Задача отмечена как выполненная.");
    }

    //===============================================================
    //  DELETE
    //===============================================================
    static void DeleteCommand(string[] parts)
    {
        if (!TryGetIndex(parts, out int index)) return;

        for (int i = index; i < count - 1; i++)
        {
            todos[i] = todos[i + 1];
            statuses[i] = statuses[i + 1];
            dates[i] = dates[i + 1];
        }

        count--;
        Console.WriteLine("Задача удалена.");
    }

    //===============================================================
    //  UPDATE
    //===============================================================
    static void UpdateCommand(string[] parts)
    {
        if (parts.Length < 3)
        {
            Console.WriteLine("Используйте: update <id> <новый текст>");
            return;
        }

        if (!int.TryParse(parts[1], out int index) || !IsValidIndex(index))
        {
            Console.WriteLine("Неверный индекс!");
            return;
        }

        string newText = parts[2];
        todos[index] = newText;
        dates[index] = DateTime.Now;

        Console.WriteLine("Задача обновлена.");
    }

    //===============================================================
    //  READ
    //===============================================================
    static void ReadCommand(string[] parts)
    {
        if (!TryGetIndex(parts, out int index)) return;

        if (string.IsNullOrWhiteSpace(todos[index]))
    {
        Console.WriteLine("Задача пустая.");
        return;
    }
    
        Console.WriteLine("------ Полный текст задачи ------");
        Console.WriteLine(todos[index]);
        Console.WriteLine();
        Console.WriteLine("Статус: " + (statuses[index] ? "выполнена" : "не выполнена"));
        Console.WriteLine("Дата последнего изменения: " + dates[index]);
        Console.WriteLine("---------------------------------");
    }

    //===============================================================
    //  HELP
    //===============================================================
    static void HelpCommand()
    {
        Console.WriteLine("Список команд:");
        Console.WriteLine(" add <текст>                  — добавить задачу");
        Console.WriteLine(" add --multiline              — многострочный ввод");
        Console.WriteLine(" view                         — показать список задач");
        Console.WriteLine(" view -i -s -d               — флаги отображения");
        Console.WriteLine(" read <id>                    — показать полный текст задачи");
        Console.WriteLine(" update <id> <текст>          — изменить текст");
        Console.WriteLine(" done <id>                    — отметить выполненной");
        Console.WriteLine(" delete <id>                  — удалить задачу");
        Console.WriteLine(" help                         — помощь");

        Console.WriteLine();
        Console.WriteLine("Флаги команды view:");
        Console.WriteLine(" -i, --index        — показать индекс задачи");
        Console.WriteLine(" -s, --status       — показать статус");
        Console.WriteLine(" -d, --update-date  — показать дату изменения");
        Console.WriteLine(" -a, --all          — показать все столбцы");
    }

    //===============================================================
    //  ВСПОМОГАТЕЛЬНЫЕ
    //===============================================================
    static bool HasFlag(string[] flags, string flag)
        => Array.IndexOf(flags, flag) != -1;

    static string[] GetFlags(string[] parts)
    {
        return Array.FindAll(parts, p => p.StartsWith("-"));
    }

    static bool TryGetIndex(string[] parts, out int index)
    {
        index = -1;

        if (parts.Length < 2)
        {
            Console.WriteLine("Не указан индекс!");
            return false;
        }

        if (!int.TryParse(parts[1], out index) || !IsValidIndex(index))
        {
            Console.WriteLine("Неверный индекс!");
            return false;
        }

        return true;
    }

    static bool IsValidIndex(int index)
        => index >= 0 && index < count;

    static void EnsureSize()
    {
        if (count < todos.Length) return;

        int newSize = todos.Length * 2;
        Array.Resize(ref todos, newSize);
        Array.Resize(ref statuses, newSize);
        Array.Resize(ref dates, newSize);
    }
}

