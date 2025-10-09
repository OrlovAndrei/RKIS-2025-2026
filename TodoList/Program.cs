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
        string name = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        string surname = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        if (!int.TryParse(Console.ReadLine(), out int birthYear))
        {
            return;
        }

        int age = DateTime.Now.Year - birthYear;
        Console.WriteLine($"Пользователь: {name} {surname}, возраст: {age}");

        string[] tasks = new string[BlockSize];
        bool[] taskStates = new bool[BlockSize];
        DateTime[] taskDates = new DateTime[BlockSize];
        int taskCount = 0;

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
                ShowProfile(name, surname, age);
            }
            else if (inputLine.StartsWith("add "))
            {
                AddNewTask(inputLine, ref tasks, ref taskStates, ref taskDates, ref taskCount);
            }
            else if (inputLine == "view")
            {
                ListTasks(tasks, taskStates, taskDates, taskCount);
            }
            else if (inputLine.StartsWith("done "))
            {
                CompleteTask(inputLine, ref taskStates, ref taskDates, taskCount);
            }
            else if (inputLine.StartsWith("delete "))
            {
                RemoveTask(inputLine, ref tasks, ref taskStates, ref taskDates, ref taskCount);
            }
            else if (inputLine.StartsWith("update "))
            {
                ChangeTaskText(inputLine, ref tasks, ref taskDates, taskCount);
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

    static void AddNewTask(string command, ref string[] tasks, ref bool[] states, ref DateTime[] dates, ref int count)
    {
        string[] parts = command.Split(new[] { ' ' }, 2);
        if (parts.Length != 2)
        {
            return;
        }

        string taskText = parts[1].Trim();

        if (taskText.StartsWith("\"") && taskText.EndsWith("\"") && taskText.Length > 2)
        {
            taskText = taskText.Substring(1, taskText.Length - 2);
        }
        else
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(taskText))
        {
            return;
        }

        if (count == tasks.Length)
            ExpandArrays(ref tasks, ref states, ref dates);

        tasks[count] = taskText;
        states[count] = false;
        dates[count] = DateTime.Now;
        count++;
        Console.WriteLine("Задача добавлена.");
    }

    static void ListTasks(string[] tasks, bool[] states, DateTime[] dates, int count)
    {
        if (count == 0)
        {
            Console.WriteLine("Задач нет.");
            return;
        }
        Console.WriteLine("Задачи:");
        for (int i = 0; i < count; i++)
        {
            string statusStr = states[i] ? "выполнено" : "не выполнено";
            Console.WriteLine($"{i + 1}. {tasks[i]} [{statusStr}] {dates[i]:dd.MM.yyyy HH:mm}");
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
