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
            else if (command == "view")
            {
                ViewTasks();
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

    private static void ViewTasks()
    {
        Console.WriteLine("Список задач:");
        for (var i = 0; i < taskCount; i++)
            Console.WriteLine($"{i + 1}. {taskList[i]} статус:{taskStatuses[i]} {taskDates[i]}");
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
        done - отметить выполненным
        delete - удалить задачу
        view — просмотр всех задач
        exit — завершить программу
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