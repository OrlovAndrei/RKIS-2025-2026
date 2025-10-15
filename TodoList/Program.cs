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
        try
        {
            birthYear = int.Parse(birthYearInput);
        }
        catch (FormatException)
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
            else if (input == "view")
            {
                ProcessView(tasks, statuses, dates, taskCount);
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
        Console.WriteLine("view - показать все задачи");
        Console.WriteLine("done <индекс> - отметить задачу как выполненную");
        Console.WriteLine("delete <индекс> - удалить задачу");
        Console.WriteLine("update <индекс> \"новый текст\" - обновить текст задачи");
        Console.WriteLine("exit - выйти из программы");
    }

    static void ProcessProfile(string firstName, string lastName, int age)
    {
        Console.WriteLine($"{firstName} {lastName}, возраст - {age}");
    }

    static void ProcessAdd(string input, string[] tasks, bool[] statuses, DateTime[] dates, ref int taskCount)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        bool isInvalidCommand = parts.Length < 1 || parts[0] != "add";
        if (isInvalidCommand)
        {
            Console.WriteLine("Неверный формат команды. Используйте: add \"текст задачи\" или add --multiline");
        }
        else
        {
            bool isMultiline = false;
            string taskText = "";
            bool hasValidParts = true;

            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i] == "--multiline" || parts[i] == "-m")
                {
                    isMultiline = true;
                }
                else if (parts[i].StartsWith("\"") && parts[i].EndsWith("\"") && parts[i].Length > 2)
                {
                    taskText = parts[i].Substring(1, parts[i].Length - 2);
                    break;
                }
                else
                {
                    hasValidParts = false;
                }
            }

            if (!hasValidParts)
            {
                Console.WriteLine("Неверный формат команды. Используйте: add \"текст задачи\" или add --multiline");
            }
            else
            {
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

                bool isTaskTextValid = !string.IsNullOrWhiteSpace(taskText);
                if (!isTaskTextValid)
                {
                    Console.WriteLine("Текст задачи не может быть пустым.");
                }
                else
                {
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
            }
        }
    }
    static void ProcessView(string[] tasks, bool[] statuses, DateTime[] dates, int taskCount)
    {
        if (taskCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
        }
        else
        {
            Console.WriteLine("Список задач:");
           
            List<string> headers = new List<string>();
            List<int> widths = new List<int>();
            List<Func<int, string>> cellGetters = new List<Func<int, string>>();

             int colIndex = 0;

            if (showIndex || showAll)
            {
                headers.Add("Индекс");
                widths.Add(6);
                cellGetters.Add(j => (j + 1).ToString());
                colIndex++;
            }

            if (showStatus || showAll)
            {
                headers.Add("Статус");
                widths.Add(10); 
                cellGetters.Add(j => statuses[j] ? "сделано" : "не сделано");
                colIndex++;
            }

            if (showDate || showAll)
            {
                headers.Add("Дата изменения");
                widths.Add(20); 
                cellGetters.Add(j => dates[j].ToString("dd.MM.yyyy HH:mm"));
                colIndex++;
            }

            headers.Add("Задача");
            widths.Add(33); 
            cellGetters.Add(j =>
            {
                string fullText = tasks[j].Replace("\n", " "); 
                string result;
                if (fullText.Length > 30)
                {
                    result = fullText.Substring(0, 30) + "...";
                }
                else
                {
                    result = fullText;
                }
                return result;
            });

            string headerLine = string.Join("", headers.Select((h, i) => i < colIndex ? h.PadRight(widths[i]) : h.PadRight(widths[i])));
            Console.WriteLine(headerLine);

            string separator = new string('-', headerLine.Length);
            Console.WriteLine(separator);

            for (int j = 0; j < taskCount; j++)
            {
                string row = "";
                for (int k = 0; k < cellGetters.Count; k++)
                {
                    string cell = cellGetters[k](j);
                    if (k == 0 && (showIndex || showAll)) 
                    {
                        row += cell.PadLeft(widths[k]).PadRight(widths[k]);
                    }
                    else
                    {
                        row += cell.PadRight(widths[k]);
                    }
                }
                Console.WriteLine(row);
            }
        }
    }

    static void ProcessDone(string input, bool[] statuses, DateTime[] dates, int taskCount)
    {
        string[] parts = input.Split(' ');
        if (parts.Length == 2 && int.TryParse(parts[1], out int idx) && idx >= 1 && idx <= taskCount)
        {
            statuses[idx - 1] = true;
            dates[idx - 1] = DateTime.Now;
            Console.WriteLine("Задача отмечена как выполненная.");
        }
        else
        {
            Console.WriteLine("Неверный индекс. Используйте: done <индекс>");
        }
    }

    static void ProcessDelete(string input, string[] tasks, bool[] statuses, DateTime[] dates, ref int taskCount)
    {
        string[] parts = input.Split(' ');
        if (parts.Length == 2 && int.TryParse(parts[1], out int idx) && idx >= 1 && idx <= taskCount)
        {
            for (int i = idx - 1; i < taskCount - 1; i++)
            {
                tasks[i] = tasks[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }
            taskCount--;
            Console.WriteLine("Задача удалена.");
        }
        else
        {
            Console.WriteLine("Неверный индекс. Используйте: delete <индекс>");
        }
    }

    static void ProcessUpdate(string input, string[] tasks, DateTime[] dates, int taskCount)
    {
        string[] parts = input.Split(' ', 3);
        if (parts.Length == 3 && int.TryParse(parts[1], out int idx) && idx >= 1 && idx <= taskCount)
        {
            string newTextPart = parts[2].Trim();
            if (newTextPart.StartsWith("\"") && newTextPart.EndsWith("\"") && newTextPart.Length > 2)
            {
                string newText = newTextPart.Substring(1, newTextPart.Length - 2);
                if (string.IsNullOrWhiteSpace(newText))
                {
                    Console.WriteLine("Новый текст задачи не может быть пустым.");
                    return;
                }
                tasks[idx - 1] = newText;
                dates[idx - 1] = DateTime.Now;
                Console.WriteLine("Задача обновлена.");
            }
            else
            {
                Console.WriteLine("Неверный формат. Используйте: update <индекс> \"новый текст\"");
            }
        }
        else
        {
            Console.WriteLine("Неверный индекс или формат. Используйте: update <индекс> \"новый текст\"");
        }
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
