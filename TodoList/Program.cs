using System;

struct TaskItem
{
    public string Text;
    public bool IsDone;
    public DateTime LastUpdate;
}

class Program
{
    static void Main(string[] args)
    {
        TaskItem[] tasks = new TaskItem[2];
        int count = 0;

        Console.Write("Имя: ");
        string fn = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(fn)) { Console.WriteLine("Имя пустое."); return; }

        Console.Write("Фамилия: ");
        string ln = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(ln)) { Console.WriteLine("Фамилия пустая."); return; }

        Console.Write("Год рождения: ");
        if (!int.TryParse(Console.ReadLine(), out int by)) { Console.WriteLine("Неверный год."); return; }
        int age = DateTime.Now.Year - by;
        Console.WriteLine($"Пользователь {fn} {ln}, возраст {age}");

        Console.WriteLine("help - команды");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(input)) { Console.WriteLine("Пусто."); continue; }

            if (input == "help") ProcessHelp();
            else if (input == "profile") ProcessProfile(fn, ln, age);
            else if (input.StartsWith("add ")) ProcessAdd(input, ref tasks, ref count);
            else if (input.StartsWith("view")) ProcessView(input, tasks, count);
            else if (input.StartsWith("read ")) ProcessRead(input, tasks, count);
            else if (input.StartsWith("done ")) ProcessDone(ref tasks, count, input);
            else if (input.StartsWith("delete ")) ProcessDelete(ref tasks, ref count, input);
            else if (input.StartsWith("update ")) ProcessUpdate(input, tasks, count);
            else if (input == "exit") { ProcessExit(); break; }
            else Console.WriteLine("Неизвестно. help для списка.");
        }
    }

    static void ProcessHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("help - показать список команд");
        Console.WriteLine("profile - показать данные пользователя");
        Console.WriteLine("add \"текст задачи\" - добавить задачу");
        Console.WriteLine("add --multiline (-m) - добавить многострочную задачу (ввод построчно, завершите '!end')");
        Console.WriteLine("view - показать все задачи");
        Console.WriteLine("view --index (-i) - показать с номерами");
        Console.WriteLine("view --status (-s) - показать с статусом (выполнена/не выполнена)");
        Console.WriteLine("view --update-date (-d) - показать с датой последнего изменения (dd.MM.yyyy HH:mm)");
        Console.WriteLine("view --all (-a) - показать все колонки (номер, статус, дата, текст)");
        Console.WriteLine("read <индекс> - показать полный текст задачи, статус и дату последнего изменения");
        Console.WriteLine("done <индекс> - отметить задачу как выполненную");
        Console.WriteLine("delete <индекс> - удалить задачу");
        Console.WriteLine("update <индекс> \"новый текст\" - обновить текст задачи");
        Console.WriteLine("update <индекс> --multiline (-m) - обновить многострочную задачу (ввод построчно, завершите '!end')");
        Console.WriteLine("exit - выйти из программы");
    }

    static void ProcessProfile(string fn, string ln, int a) => Console.WriteLine($"Пользователь: {fn} {ln}, возраст {a}");

    static void ProcessAdd(string input, ref TaskItem[] tasks, ref int count)
    {
        string cmd = input.Substring(4).Trim();
        string text = (cmd == "-m" || cmd == "--multiline") ? ReadMultiline() : cmd.Trim('\"');
        if (string.IsNullOrEmpty(text)) { Console.WriteLine("Текст пустой."); return; }
        if (count == tasks.Length) tasks = Resize(ref tasks);
        tasks[count] = new TaskItem { Text = text, IsDone = false, LastUpdate = DateTime.Now };
        count++;
        Console.WriteLine("Добавлено.");
    }

    static string ReadMultiline()
    {
        Console.WriteLine("Ввод построчно, !end для конца:");
        string res = "";
        while (true) { Console.Write("> "); string l = Console.ReadLine(); if (l == "!end") break; res += l + "\n"; }
        return res.TrimEnd('\n');
    }

    static void ProcessView(string input, TaskItem[] tasks, int count)
    {
        bool idx = false, stat = false, date_ = false;
        foreach (string p in input.Split(' ').Skip(1))
        {
            if (p.Contains('i') || p == "--index") idx = true;
            if (p.Contains('s') || p == "--status") stat = true;
            if (p.Contains('d') || p == "--update-date") date_ = true;
            if (p.Contains('a') || p == "--all") idx = stat = date_ = true;
        }
        if (count == 0) { Console.WriteLine("Нет задач."); return; }
        for (int i = 0; i < count; i++)
        {
            string row = "";
            if (idx) row += $"{i + 1}. ";
            if (stat) row += tasks[i].IsDone ? "[✓] " : "[ ] ";
            row += tasks[i].Text.Replace("\n", " ");
            if (date_) row += $" ({tasks[i].LastUpdate:dd.MM.yyyy HH:mm})";
            Console.WriteLine(row);
        }
    }

    static void ProcessRead(string input, TaskItem[] tasks, int count)
    {
        if (!TryParseId(input.Split(' ')[1], count, out int id)) Console.WriteLine("Неверный id.");
        else
        {
            var t = tasks[id - 1];
            Console.WriteLine($"Задача:\n{t.Text}\nСтатус: {(t.IsDone ? "Сделано" : "Не сделано")}\nДата: {t.LastUpdate:dd.MM.yyyy HH:mm}");
        }
    }

    static void ProcessDone(ref TaskItem[] tasks, int count, string input)
    {
        if (!TryParseId(input.Split(' ')[1], count, out int id)) Console.WriteLine("Неверный id.");
        else { tasks[id - 1].IsDone = true; tasks[id - 1].LastUpdate = DateTime.Now; Console.WriteLine("Готово"); }
    }

    static void ProcessDelete(ref TaskItem[] tasks, ref int count, string input)
    {
        if (!TryParseId(input.Split(' ')[1], count, out int id)) Console.WriteLine("Неверный id.");
        else { for (int i = id - 1; i < count - 1; i++) tasks[i] = tasks[i + 1]; count--; Console.WriteLine("Удалено"); }
    }

    static void ProcessUpdate(string input, TaskItem[] tasks, int count)
    {
        string[] parts = input.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2 || !TryParseId(parts[1], count, out int id)) { Console.WriteLine("Неверно."); return; }
        string text = (parts.Length == 2 || parts[2] == "-m" || parts[2] == "--multiline") ? ReadMultiline() : parts[2].Trim('\"');
        if (string.IsNullOrEmpty(text)) { Console.WriteLine("Текст пустой."); return; }
        tasks[id - 1].Text = text;
        tasks[id - 1].LastUpdate = DateTime.Now;
        Console.WriteLine("Обновлено");
    }

    static void ProcessExit() => Console.WriteLine("Выход");

    static bool TryParseId(string s, int count, out int id) => int.TryParse(s, out id) && id >= 1 && id <= count;

    static TaskItem[] Resize(ref TaskItem[] old)
    {
        TaskItem[] nw = new TaskItem[old.Length * 2];
        Array.Copy(old, nw, old.Length);
        return nw;
    }
}
