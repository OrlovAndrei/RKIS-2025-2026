using System;

class TodoList
{
    // Константы
    const int INITIAL_ARRAY_SIZE = 2;
    const int ARRAY_BLOCK_SIZE = 2;

    static string[] tasks = new string[INITIAL_ARRAY_SIZE];
    static bool[] taskStatuses = new bool[INITIAL_ARRAY_SIZE]; // статус выполнения
    static DateTime[] taskDates = new DateTime[INITIAL_ARRAY_SIZE]; // дата изменения/создания
    static int taskCount = 0;

    // Переменные для профиля
    static string userFirstName;
    static string userLastName;
    static int userAge;

    static void Main()
    {
        // Ввод профиля
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

            // Обработка команд
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

    // --- Общие методы команд ---

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

    static void ShowProfile()
    {
        Console.WriteLine($"{userFirstName} {userLastName}, возраст: {userAge}");
    }

    // --- Реализация команд ---

    static void AddTaskCommand(string command)
    {
        // Вынесена логика добавления задачи
        string[] parts = command.Split(new[] { ' ' }, 2);
        if (parts.Length != 2)
        {
            Console.WriteLine("Некорректная команда. Используйте: add \"текст\"");
            return;
        }

        string taskText = parts[1].Trim();

        if (taskText.StartsWith("\"") && taskText.EndsWith("\"") && taskText.Length > 2)
        {
            taskText = taskText.Substring(1, taskText.Length - 2);
        }
        else
        {
            Console.WriteLine("Текст задачи должен быть в кавычках.");
            return;
        }

        AddNewTask(taskText);
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

    static void MarkTaskAsDone(string command)
    {
        string[] parts = command.Split(' ');
        if (parts.Length != 2 || !int.TryParse(parts[1], out int index) || index < 1 || index > taskCount)
        {
            Console.WriteLine("Некорректный номер задачи.");
            return;
        }

        int taskIndex = index - 1;
        taskStatuses[taskIndex] = true;
        taskDates[taskIndex] = DateTime.Now;
        Console.WriteLine($"Задача {index} отмечена как выполненная.");
    }

    static void DeleteTaskCommand(string command)
    {
        string[] parts = command.Split(' ');
        if (parts.Length != 2 || !int.TryParse(parts[1], out int index) || index < 1 || index > taskCount)
        {
            Console.WriteLine("Некорректный номер задачи.");
            return;
        }

        DeleteTask(index - 1);
    }

    static void UpdateTaskCommand(string command)
    {
        // Формат: update <номер> "новый текст"
        string[] parts = command.Split(new[] { ' ' }, 3);
        if (parts.Length != 3 || !int.TryParse(parts[1], out int index) || index < 1 || index > taskCount)
        {
            Console.WriteLine("Некорректная команда. Используйте: update <номер> \"новый текст\"");
            return;
        }

        string newTextPart = parts[2].Trim();

        if (newTextPart.StartsWith("\"") && newTextPart.EndsWith("\"") && newTextPart.Length > 2)
        {
            string newText = newTextPart.Substring(1, newTextPart.Length - 2);
            if (string.IsNullOrWhiteSpace(newText))
            {
                Console.WriteLine("Текст не может быть пустым.");
                return;
            }
            UpdateTask(index - 1, newText);
        }
        else
        {
            Console.WriteLine("Текст задачи должен быть в кавычках.");
            return;
        }
    }

    // --- Методы для работы с массивами ---

    static void AddNewTask(string taskText)
    {
        if (taskCount == tasks.Length)
            ExpandArrays();

        tasks[taskCount] = taskText;
        taskStatuses[taskCount] = false; // по условию — изначально не выполнена
        taskDates[taskCount] = DateTime.Now; // дата создания
        taskCount++;
        Console.WriteLine("Задача добавлена.");
        // Commit (если использовать систему контроля версий, то тут)
    }

    static void DeleteTask(int index)
    {
        for (int i = index; i < taskCount - 1; i++)
        {
            tasks[i] = tasks[i + 1];
            taskStatuses[i] = taskStatuses[i + 1];
            taskDates[i] = taskDates[i + 1];
        }
        taskCount--;
        Console.WriteLine($"Задача {index + 1} удалена.");
        // Commit
    }

    static void UpdateTask(int index, string newText)
    {
        tasks[index] = newText;
        taskDates[index] = DateTime.Now;
        Console.WriteLine($"Задача {index + 1} обновлена.");
        // Commit
    }

    static void ExpandArrays()
    {
        int newSize = tasks.Length + ARRAY_BLOCK_SIZE;
        Array.Resize(ref tasks, newSize);
        Array.Resize(ref taskStatuses, newSize);
        Array.Resize(ref taskDates, newSize);
    }
}