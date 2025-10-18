namespace TodoList;

class Program
{
    private static string name;
    private static string surname;
    private static int age;

    private static string[] taskList = new string[2];
    private static bool[] taskStatuses = new bool[2];
    private static DateTime[] taskDates = new DateTime[2];
    private static int taskCount;

    public static void Main()
    {
        Console.WriteLine("Работу выполнил Кулаков");
        Console.Write("Введите имя: ");
        name = Console.ReadLine();
        Console.Write("Введите фамилию: ");
        surname = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        var year = int.Parse(Console.ReadLine());
        age = DateTime.Now.Year - year;

        Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");

        while (true)
        {
            Console.WriteLine("Введите команду: ");
            var command = Console.ReadLine();

            if (command == "help")
            {
                Help();
            }
            else if (command == "profile")
            {
                Profile();
            }
            else if (command.StartsWith("add "))
            {
                AddTask(command);
            }
            else if (command.StartsWith("view"))
            {
                ViewTasks(command);
            }
            else if (command.StartsWith("read"))
            {
                ReadTask(command);
            }
            else if (command.StartsWith("done "))
            {
                DoneTask(command);
            }
            else if (command.StartsWith("delete "))
            {
                DeleteTask(command);
            }
            else if (command.StartsWith("update "))
            {
                UpdateTask(command);
            }
            else if (command == "exit")
            {
                Console.WriteLine("Программа завершена.");
                break;
            }
            else
            {
                Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
            }
        }
    }

    private static void UpdateTask(string input)
    {
        string[] parts = input.Split(' ', 3);
        var taskIndex = int.Parse(parts[1]) - 1;

        var newText = parts[2];
        taskList[taskIndex] = newText;
        taskDates[taskIndex] = DateTime.Now;
        Console.WriteLine($"Задача {taskIndex} обновлена.");
    }

    private static void DeleteTask(string input)
    {
        string[] parts = input.Split(' ', 2);
        var taskIndex = int.Parse(parts[1]) - 1;

        for (var i = taskIndex; i < taskCount - 1; i++)
        {
            taskList[i] = taskList[i + 1];
            taskStatuses[i] = taskStatuses[i + 1];
            taskDates[i] = taskDates[i + 1];
        }

        taskCount--;
        Console.WriteLine($"Задача {taskIndex + 1} удалена.");
    }

    private static void DoneTask(string input)
    {
        string[] parts = input.Split(' ', 2);
        var taskIndex = int.Parse(parts[1]) - 1;

        taskStatuses[taskIndex] = true;
        taskDates[taskIndex] = DateTime.Now;

        Console.WriteLine($"Задача {taskIndex + 1} выполнена.");
    }

    private static void ReadTask(string input)
    {
        string[] parts = input.Split(' ', 2);
        var taskIndex = int.Parse(parts[1]) - 1;

        Console.WriteLine($"Полная информация о задаче {taskIndex}");
        Console.WriteLine($"Текст: {taskList[taskIndex]}");
        Console.WriteLine($"Статус: {(taskStatuses[taskIndex] ? "Выполнено" : "Не выполнено")}");
        Console.WriteLine($"Изменено: {taskDates[taskIndex]:dd.MM.yyyy HH:mm:ss}");
    }
    private static void ViewTasks(string input)
    {
        var flags = ParseFlags(input);

        bool hasAll = flags.Contains("--all") || flags.Contains("-a");
        bool hasIndex = flags.Contains("--index") || flags.Contains("-i");
        bool hasStatus = flags.Contains("--status") || flags.Contains("-s");
        bool hasDate = flags.Contains("--update-date") || flags.Contains("-d");
        
        const int indexSize = 8;
        const int textSize = 34;
        const int statusSize = 14;
        const int dateSize = 18;
        
        List<string> headers = new List<string>();
        if (hasIndex || hasAll) headers.Add("Индекс".PadRight(indexSize));
        headers.Add("Задача".PadRight(textSize));
        if (hasStatus || hasAll) headers.Add("Статус".PadRight(statusSize));
        if (hasDate || hasAll) headers.Add("Изменено".PadRight(dateSize));

        string header = string.Join(" | ", headers);
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));
        
        for (int i = 0; i < taskCount; i++)
        {
            string title = taskList[i].Replace("\n", " ");
            if (title.Length > 30) title = title.Substring(0, 30) + "...";
            
            List<string> rows = new List<string>();
            if (hasIndex || hasAll) rows.Add((i + 1).ToString().PadRight(indexSize));
            rows.Add(title.PadRight(textSize));
            if (hasStatus || hasAll) rows.Add((taskStatuses[i] ? "Выполнено" : "Не выполнено").PadRight(statusSize));
            if (hasDate || hasAll) rows.Add(taskDates[i].ToString("yyyy-MM-dd HH:mm").PadRight(dateSize));

            Console.WriteLine(string.Join(" | ", rows));
        }
    }

    private static void AddTask(string input)
    {
        var flags = ParseFlags(input);

        if (flags.Contains("-m") || flags.Contains("--multi"))
        {
            Console.WriteLine("Для выхода из многострочного режима введите !end");
            List<string> lines = new List<string>();
            while (true)
            {
                string line = Console.ReadLine();
                if (line == "!end") break;
                lines.Add(line);
            }
            AddTaskToArray(string.Join("\n", lines));
        }
        else
        {
            AddTaskToArray(input.Split(" ", 2)[1]);
        }
    }
    
    private static void AddTaskToArray(string task)
    {
        if (taskCount == taskList.Length) ExpandArrays();

        taskList[taskCount] = task;
        taskStatuses[taskCount] = false;
        taskDates[taskCount] = DateTime.Now;

        taskCount++;
        Console.WriteLine($"Задача добавлена: {task}");
    }
    private static string[] ParseFlags(string command)
    {
        List<string> flags = new List<string>();
        foreach (var text in command.Split(' '))
        {
            if (text.StartsWith("-"))
            {
                for (int i = 1; i < text.Length; i++)
                {
                    flags.Add("-" + text[i]);
                }
            }
            else if (text.StartsWith("--"))
            {
                flags.Add(text);
            }
        }
        return flags.ToArray();
    }

    private static void Profile()
    {
        Console.WriteLine($"{name} {surname}, {age}");
    }

    private static void Help()
    {
        Console.WriteLine("""
        Доступные команды:
        help — список команд
        profile — выводит данные профиля
        add "текст задачи" — добавляет задачу
          Флаги: --multiline -m
        done - отметить выполненным
        delete - удалить задачу
        view — просмотр всех задач
          Флаги: --index -i, --status -s, --update-date -d, --all -a
        exit — завершить программу
        read - посмотреть полный текст задачи
        """);
    }

    private static void ExpandArrays()
    {
        var newSize = taskList.Length * 2;
        Array.Resize(ref taskList, newSize);
        Array.Resize(ref taskStatuses, newSize);
        Array.Resize(ref taskDates, newSize);
    }
}