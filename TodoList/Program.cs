using System;

class TodoManager
{
    const int INITIAL_ARRAY_SIZE = 2;
    const int BlockSize = 2;

    static string[] tasks = new string[INITIAL_ARRAY_SIZE];
    static bool[] taskStatuses = new bool[INITIAL_ARRAY_SIZE];
    static DateTime[] taskDates = new DateTime[INITIAL_ARRAY_SIZE];
    static int taskCount = 0;
    static string userFirstName;
    static string userLastName;
    static int userAge;
    static void Main()
    {
        Console.Write("Введите имя: ");
        userFirstName = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        userLastName = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        if (!int.TryParse(Console.ReadLine(), out int birthYear))
        {
            Console.WriteLine("Некорректный ввод года рождения. Завершение программы.");
            return;
        }
        userAge = DateTime.Now.Year - birthYear;
        Console.WriteLine($"Пользователь: {userFirstName} {userLastName}, возраст: {userAge}");

        Console.WriteLine("Введите команду (help - список команд):");

        while (true)
        {
            Console.Write("> ");
            string inputLine = Console.ReadLine().Trim();

            if (string.IsNullOrWhiteSpace(inputLine))
                continue;

            if (inputLine == "help")
            {
                PrintHelp();
            }
            else if (inputLine == "profile")
            {
                ShowProfile();
            }
            else if (inputLine.StartsWith("add "))
            {
                AddTaskCommand(inputLine);
            }
            else if (inputLine == "view")
            {
                ListTasks();
            }
            else if (inputLine.StartsWith("done "))
            {
                MarkTaskAsDone(inputLine);
            }
            else if (inputLine.StartsWith("delete "))
            {
                DeleteTaskCommand(inputLine);
            }
            else if (inputLine.StartsWith("update "))
            {
                UpdateTaskCommand(inputLine);
            }
            else if (inputLine == "exit")
            {
                Console.WriteLine("Выход. До свидания!");
                break;
            }
            else
            {
                Console.WriteLine("Нераспознанная команда. Введите help для справки.");
            }
        }
    }

    static void PrintHelp()
    {
        Console.WriteLine("Команды:");
        Console.WriteLine("help - список команд");
        Console.WriteLine("profile - показать данные пользователя");
        Console.WriteLine("add \"текст\" - добавить задачу");
        Console.WriteLine("view - список задач");
        Console.WriteLine("done <номер> - отметить как выполненную");
        Console.WriteLine("delete <номер> - удалить");
        Console.WriteLine("update <номер> \"новый текст\" - изменить задачу");
        Console.WriteLine("exit - завершение");
    }

    static void ShowProfile(string firstName, string lastName, int age)
    {
        Console.WriteLine($"{firstName} {lastName}, возраст: {age}");
    }

    static void AddNewTask(string taskText)
{
    tasks.Add(taskText);
    taskStatuses.Add(false);
    taskDates.Add(DateTime.Now);
    Console.WriteLine("Задача добавлена.");
}

    static void ListTasks()
    {
        if (taskCount == 0)
        {
            Console.WriteLine("Задач нет.");
            return;
        }
        Console.WriteLine("Задачи:");
        for (int i = 0; i < taskCount; i++)
        {
            string statusStr = taskStatuses[i] ? "выполнено" : "не выполнено";
            Console.WriteLine($"{i + 1}. {tasks[i]} [{statusStr}] {taskDates[i]:dd.MM.yyyy HH:mm}");
        }
    }

    static void CompleteTask(string command, ref bool[] states, ref DateTime[] dates, int count)
    {
        string[] parts = command.Split(' ');
        if (parts.Length != 2 || !int.TryParse(parts[1], out int taskNumber) || taskNumber < 1 || taskNumber > count)
        {
            return;
        }

        states[taskNumber - 1] = true;
        dates[taskNumber - 1] = DateTime.Now;
        Console.WriteLine($"Задача {taskNumber} отмечена как выполненная.");
    }

    static void RemoveTask(string command, ref string[] tasks, ref bool[] states, ref DateTime[] dates, ref int count)
    {
        string[] parts = command.Split(' ');
        if (parts.Length != 2 || !int.TryParse(parts[1], out int taskNumber) || taskNumber < 1 || taskNumber > count)
        {
            return;
        }

        for (int i = taskNumber - 1; i < count - 1; i++)
        {
            tasks[i] = tasks[i + 1];
            states[i] = states[i + 1];
            dates[i] = dates[i + 1];
        }
        count--;
        Console.WriteLine($"Задача {taskNumber} удалена.");
    }

    static void ChangeTaskText(string command, ref string[] tasks, ref DateTime[] dates, int count)
    {
        string[] parts = command.Split(new[] { ' ' }, 3);
        if (parts.Length != 3 || !int.TryParse(parts[1], out int taskNumber) || taskNumber < 1 || taskNumber > count)
        {
            return;
        }

        string newTextPart = parts[2].Trim();
        if (newTextPart.StartsWith("\"") && newTextPart.EndsWith("\"") && newTextPart.Length > 2)
        {
            string newText = newTextPart.Substring(1, newTextPart.Length - 2);
            if (string.IsNullOrWhiteSpace(newText))
            {
                return;
            }
            tasks[taskNumber - 1] = newText;
            dates[taskNumber - 1] = DateTime.Now;
            Console.WriteLine("Задача обновлена.");
        }
        else
        {
            return;
        }
    }

    static void ExpandArrays(ref string[] tasks, ref bool[] states, ref DateTime[] dates)
    {
        int newSize = tasks.Length + BlockSize;
        Array.Resize(ref tasks, newSize);
        Array.Resize(ref states, newSize);
        Array.Resize(ref dates, newSize);
    }
}
