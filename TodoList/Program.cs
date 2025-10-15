using System;
class Program
{
    const int InitialCapacity = 2;

    static void Main(string[] args)
    {
        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(firstName))
        {
            Console.WriteLine("Имя не может быть пустым.");
            return;
        }

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Фамилия не может быть пустой.");
            return;
        }

        Console.Write("Введите год рождения: ");
        string birthYearInput = Console.ReadLine();
        int birthYear;
        if (!int.TryParse(birthYearInput, out birthYear))
        {
            Console.WriteLine("Неверный формат года рождения. Введите число.");
            return;
        }

        int currentYear = DateTime.Now.Year;
        int age = currentYear - birthYear;

        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

        string[] tasks = new string[InitialCapacity];
        bool[] statuses = new bool[InitialCapacity];
        DateTime[] dates = new DateTime[InitialCapacity];
        int taskCount = 0;

        Console.WriteLine("Введите команду (help - список команд):");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Команда не может быть пустой.");
                continue;
            }

            if (input == "help")
            {
                ProcessHelp();
            }
            else if (input == "profile")
            {
                ProcessProfile(firstName, lastName, age);
            }
            else if (input.StartsWith("add "))
            {
                ProcessAdd(input, tasks, statuses, dates, ref taskCount);
            }
            else if (input.StartsWith("view "))
            {
                ProcessView(input, tasks, statuses, dates, taskCount);
            }
            else if (input == "view")
            {
                ProcessView("view", tasks, statuses, dates, taskCount);
            }
            else if (input.StartsWith("read "))
            {
                ProcessRead(input, tasks, statuses, dates, taskCount);
            }
            else if (input.StartsWith("done "))
            {
                ProcessDone(input, statuses, dates, taskCount);
            }
            else if (input.StartsWith("delete "))
            {
                ProcessDelete(input, tasks, statuses, dates, ref taskCount);
            }
            else if (input.StartsWith("update "))
            {
                ProcessUpdate(input, tasks, dates, taskCount);
            }
            else if (input == "exit")
            {
                ProcessExit();
                break;
            }
            else
            {
                Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
            }
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
        Console.WriteLine("update <индекс> --multiline (-m) - обновить многострочную задачу (ввод построчно, завершите '!end').");
        Console.WriteLine("exit - выйти из программы.")
    }

    static void ProcessProfile(string firstName, string lastName, int age)
    {
        Console.WriteLine($"Пользователь: {firstName} {lastName}, возраст - {age}");
    }

    static void ProcessAdd(string input, string[] tasks, bool[] statuses, DateTime[] dates, ref int taskCount)
    {
        if (input == null) return;
        string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2 || parts[0] != "add")
        {
            Console.WriteLine("Неверный формат команды. Используйте: add \"текст задачи\" или add --multiline");
            return;
        }

        bool isMultiline = false;
        string taskText = "";

        if (parts[1] == "--multiline" || parts[1] == "-m")
        {
            isMultiline = true;
        }
        else if (parts.Length == 2 && parts[1].StartsWith("\"") && parts[1].EndsWith("\""))
        {
            taskText = parts[1].Substring(1, parts[1].Length - 2);
        }
        else
        {
            Console.WriteLine("Неверный формат команды. Используйте: add \"текст задачи\" или add --multiline");
            return;
        }

        if (isMultiline)
        {
            Console.WriteLine("Введите текст задачи построчно. Введите '!end' для завершения:");
            string line;
            while (true)
            {
                Console.Write("> ");
                line = Console.ReadLine();
                if (line == "!end")
                {
                    break;
                }
                taskText += line + "\n";
            }
            taskText = taskText.TrimEnd('\n');
        }

        if (string.IsNullOrWhiteSpace(taskText))
        {
            Console.WriteLine("Текст задачи не может быть пустым.");
            return;
        }

        if (taskCount == tasks.Length)
        {
            (tasks, statuses, dates) = ResizeArrays(tasks, statuses, dates);
        }

        tasks[taskCount] = taskText;
        statuses[taskCount] = false;
        dates[taskCount] = DateTime.Now;
        taskCount++;

        Console.WriteLine("Задача добавлена.");
    }

    static void ProcessView(string input, string[] tasks, bool[] statuses, DateTime[] dates, int taskCount)
    {
        if (input == null) return;
        bool showIndex = false;
        bool showStatus = false;
        bool showDate = false;
        bool showAll = false;

        string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < parts.Length; i++)
        {
            string flag = parts[i];
            if (flag == "--index" || flag == "-i")
            {
                showIndex = true;
            }
            else if (flag == "--status" || flag == "-s")
            {
                showStatus = true;
            }
            else if (flag == "--update-date" || flag == "-d")
            {
                showDate = true;
            }
            else if (flag == "--all" || flag == "-a")
            {
                showAll = true;
            }
            else if (flag.StartsWith("-") && flag.Length > 1 && flag[1] != '-')
            {
                for (int j = 1; j < flag.Length; j++)
                {
                    char f = flag[j];
                    if (f == 'i') showIndex = true;
                    else if (f == 's') showStatus = true;
                    else if (f == 'd') showDate = true;
                    else if (f == 'a') showAll = true;
                }
            }
            else
            {
                Console.WriteLine("Неизвестный флаг: " + flag);
                return;
            }
        }

        if (taskCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        Console.WriteLine("Список задач:");

        List<string> headers = new List<string>();
        List<int> widths = new List<int>();
        List<Func<int, string>> cellGetters = new List<Func<int, string>>();

        if (showIndex || showAll)
        {
            headers.Add("Индекс");
            widths.Add(6);
            cellGetters.Add(j => (j + 1).ToString());
        }

        if (showStatus || showAll)
        {
            headers.Add("Статус");
            widths.Add(12);
            cellGetters.Add(j => statuses[j] ? "выполнена" : "не выполнена");
        }

        if (showDate || showAll)
        {
            headers.Add("Дата изменения");
            widths.Add(16);
            cellGetters.Add(j => dates[j].ToString("dd.MM.yyyy HH:mm"));
        }

        headers.Add("Задача");
        widths.Add(33);
        cellGetters.Add(j =>
        {
            string fullText = tasks[j]?.Replace("\n", " ") ?? "";
            return fullText.Length > 30 ? fullText.Substring(0, 30) + "..." : fullText;
        });

        string headerLine = "";
        for (int i = 0; i < headers.Count; i++)
        {
            headerLine += headers[i].PadRight(widths[i]);
        }
        Console.WriteLine(headerLine);

        string separator = new string('-', headerLine.Length);
        Console.WriteLine(separator);

        for (int j = 0; j < taskCount; j++)
        {
            string row = "";
            for (int k = 0; k < cellGetters.Count; k++)
            {
                row += cellGetters[k](j).PadRight(widths[k]);
            }
            Console.WriteLine(row);
        }
    }

    static void ProcessRead(string input, string[] tasks, bool[] statuses, DateTime[] dates, int taskCount)
    {
        if (input == null) return;
        string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2 || parts[0] != "read")
        {
            Console.WriteLine("Неверный формат команды. Используйте: read <индекс>");
            return;
        }

        if (!int.TryParse(parts[1], out int idx) || idx < 1 || idx > taskCount)
        {
            Console.WriteLine("Неверный индекс задачи. Индекс должен быть числом от 1 до " + taskCount + ".");
            return;
        }

        Console.WriteLine("Задача:");
        Console.WriteLine(tasks[idx - 1]);
        string statusText = statuses[idx - 1] ? "выполнена" : "не выполнена";
        Console.WriteLine("Статус: " + statusText);
        Console.WriteLine("Дата последнего изменения: " + dates[idx - 1].ToString("dd.MM.yyyy HH:mm"));
    }

    static void ProcessDone(string input, bool[] statuses, DateTime[] dates, int taskCount)
    {
        if (input == null) return;
        string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2 || parts[0] != "done")
        {
            Console.WriteLine("Неверный формат команды. Используйте: done <индекс>");
            return;
        }

        if (!int.TryParse(parts[1], out int idx) || idx < 1 || idx > taskCount)
        {
            Console.WriteLine("Неверный индекс. Индекс должен быть числом от 1 до " + taskCount + ".");
            return;
        }

        statuses[idx - 1] = true;
        dates[idx - 1] = DateTime.Now;
        Console.WriteLine("Задача отмечена как выполненная.");
    }

    static void ProcessDelete(string input, string[] tasks, bool[] statuses, DateTime[] dates, ref int taskCount)
    {
        if (input == null) return;
        string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2 || parts[0] != "delete")
        {
            Console.WriteLine("Неверный формат команды. Используйте: delete <индекс>");
            return;
        }

        if (!int.TryParse(parts[1], out int idx) || idx < 1 || idx > taskCount)
        {
            Console.WriteLine("Неверный индекс. Индекс должен быть числом от 1 до " + taskCount + ".");
            return;
        }

        for (int i = idx - 1; i < taskCount - 1; i++)
        {
            tasks[i] = tasks[i + 1];
            statuses[i] = statuses[i + 1];
            dates[i] = dates[i + 1];
        }
        taskCount--;
        Console.WriteLine("Задача удалена.");
    }

    static void ProcessUpdate(string input, string[] tasks, DateTime[] dates, int taskCount)
    {
        if (input == null) return;
        string[] parts = input.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2 || parts[0] != "update")
        {
            Console.WriteLine("Неверный формат команды. Используйте: update <индекс> \"новый текст\" или update <индекс> --multiline");
            return;
        }

        if (!int.TryParse(parts[1], out int idx) || idx < 1 || idx > taskCount)
        {
            Console.WriteLine("Неверный индекс. Индекс должен быть числом от 1 до " + taskCount + ".");
            return;
        }

        bool isMultiline = false;
        string newText = "";

        if (parts.Length == 3)
        {
            if (parts[2] == "--multiline" || parts[2] == "-m")
            {
                isMultiline = true;
            }
            else if (parts[2].StartsWith("\"") && parts[2].EndsWith("\""))
            {
                newText = parts[2].Substring(1, parts[2].Length - 2);
            }
            else
            {
                Console.WriteLine("Неверный формат. Используйте: update <индекс> \"новый текст\" или update <индекс> --multiline");
                return;
            }
        }
        else if (parts.Length == 2)
        {
            isMultiline = true;
        }

        if (isMultiline)
        {
            Console.WriteLine("Введите новый текст задачи построчно. Введите '!end' для завершения:");
            string line;
            while (true)
            {
                Console.Write("> ");
                line = Console.ReadLine();
                if (line == "!end")
                {
                    break;
                }
                newText += line + "\n";
            }
            newText = newText.TrimEnd('\n');
        }

        if (string.IsNullOrWhiteSpace(newText))
        {
            Console.WriteLine("Новый текст задачи не может быть пустым.");
            return;
        }

        tasks[idx - 1] = newText;
        dates[idx - 1] = DateTime.Now;
        Console.WriteLine("Задача обновлена.");
    }

    static void ProcessExit()
    {
        Console.WriteLine("Выход из программы.");
    }

    static (string[], bool[], DateTime[]) ResizeArrays(string[] tasks, bool[] statuses, DateTime[] dates)
    {
        int newSize = tasks.Length * 2;
        string[] newTasks = new string[newSize];
        bool[] newStatuses = new bool[newSize];
        DateTime[] newDates = new DateTime[newSize];

        for (int i = 0; i < tasks.Length; i++)
        {
            newTasks[i] = tasks[i];
            newStatuses[i] = statuses[i];
            newDates[i] = dates[i];
        }

        return (newTasks, newStatuses, newDates);
    }
}
