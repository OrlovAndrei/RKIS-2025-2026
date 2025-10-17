using System;

class Program
{
    // === Константы ===
    private const int InitialArraySize = 2;

    // === Данные пользователя ===
    private static string userFirstName;
    private static string userLastName;
    private static int userBirthYear;

    // === Данные задач ===
    private static string[] taskTexts = new string[InitialArraySize];
    private static bool[] taskStatuses = new bool[InitialArraySize];
    private static DateTime[] taskDates = new DateTime[InitialArraySize];
    private static int taskCount = 0;

    static void Main()
    {
        Console.WriteLine("Введите имя:");
        userFirstName = Console.ReadLine();

        Console.WriteLine("Введите фамилию:");
        userLastName = Console.ReadLine();

        Console.WriteLine("Введите год рождения:");
        while (!int.TryParse(Console.ReadLine(), out userBirthYear))
        {
            Console.WriteLine("Ошибка: введите корректный год рождения.");
        }

        int currentYear = DateTime.Now.Year;
        int userAge = currentYear - userBirthYear;
        Console.WriteLine($"Добавлен пользователь {userFirstName} {userLastName}, возраст - {userAge}");
        Console.WriteLine("Работу выполнил Рублёв");

        while (true)
        {
            Console.Write("> ");
            string command = Console.ReadLine().Trim();

            if (command == "help")
                ShowHelp();
            else if (command == "profile")
                ShowProfile();
            else if (command.StartsWith("add"))
                HandleAddCommand(command);
            else if (command.StartsWith("view"))
                HandleViewCommand(command);
            else if (command.StartsWith("done "))
                HandleDoneCommand(command);
            else if (command.StartsWith("delete "))
                HandleDeleteCommand(command);
            else if (command.StartsWith("update "))
                HandleUpdateCommand(command);
            else if (command.StartsWith("read "))
                HandleReadCommand(command);
            else if (command == "exit")
            {
                Console.WriteLine("Программа завершена.");
                break;
            }
            else
                Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
        }
    }

    // === Методы обработки команд ===

    private static void ShowHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("help — список команд");
        Console.WriteLine("profile — показать профиль пользователя");
        Console.WriteLine("add \"текст задачи\" — добавить задачу однострочно");
        Console.WriteLine("add --multiline/-m — добавить задачу многострочно");
        Console.WriteLine("view — показать все задачи");
        Console.WriteLine("view --index/-i, --status/-s, --update-date/-d, --all/-a — флаги отображения");
        Console.WriteLine("done <idx> — отметить задачу выполненной");
        Console.WriteLine("delete <idx> — удалить задачу");
        Console.WriteLine("update <idx> \"новый текст\" — обновить задачу");
        Console.WriteLine("read <idx> — показать полный текст задачи");
        Console.WriteLine("exit — выход из программы");
    }

    private static void ShowProfile()
    {
        Console.WriteLine($"{userFirstName} {userLastName}, {userBirthYear}");
    }

    private static void HandleAddCommand(string command)
    {
        bool multiline = command.Contains("--multiline") || command.Contains("-m");

        if (multiline)
        {
            Console.WriteLine("Введите строки задачи (ввод завершите !end):");
            string fullText = "";
            while (true)
            {
                string line = Console.ReadLine();
                if (line == "!end") break;
                fullText += line + "\n";
            }
            fullText = fullText.TrimEnd('\n');
            AddTask(fullText);
        }
        else
        {
            string[] parts = command.Split('"');
            if (parts.Length < 2)
            {
                Console.WriteLine("Ошибка: используйте формат add \"текст задачи\"");
                return;
            }
            AddTask(parts[1]);
        }
    }

    private static void AddTask(string text)
    {
        ExpandArraysIfNeeded();
        taskTexts[taskCount] = text;
        taskStatuses[taskCount] = false;
        taskDates[taskCount] = DateTime.Now;
        taskCount++;
        Console.WriteLine($"Задача добавлена: \"{text}\"");
    }

    private static void HandleViewCommand(string command)
    {
        bool showIndex = command.Contains("--index") || command.Contains("-i");
        bool showStatus = command.Contains("--status") || command.Contains("-s");
        bool showDate = command.Contains("--update-date") || command.Contains("-d");
        bool showAll = command.Contains("--all") || command.Contains("-a");

        if (showAll)
        {
            showIndex = showStatus = showDate = true;
        }

        ShowTasks(showIndex, showStatus, showDate);
    }

    private static void ShowTasks(bool showIndex = false, bool showStatus = false, bool showDate = false)
    {
        if (taskCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        Console.WriteLine("Ваши задачи:");
        for (int i = 0; i < taskCount; i++)
        {
            string output = "";
            if (showIndex) output += $"[{i + 1}] ";
            output += $"{(taskTexts[i].Length > 30 ? taskTexts[i].Substring(0, 30) + "..." : taskTexts[i])}";
            if (showStatus) output += $" | {(taskStatuses[i] ? "сделано" : "не сделано")}";
            if (showDate) output += $" | {taskDates[i]}";
            Console.WriteLine(output);
        }
    }

    private static void HandleDoneCommand(string command)
    {
        if (!TryParseIndex(command, out int index)) return;
        if (index < 1 || index > taskCount) { Console.WriteLine("Ошибка: некорректный индекс задачи."); return; }
        int idx = index - 1;
        taskStatuses[idx] = true;
        taskDates[idx] = DateTime.Now;
        Console.WriteLine($"Задача [{index}] отмечена как выполненная.");
    }

    private static void HandleDeleteCommand(string command)
    {
        if (!TryParseIndex(command, out int index)) return;
        if (index < 1 || index > taskCount) { Console.WriteLine("Ошибка: некорректный индекс задачи."); return; }
        int idx = index - 1;
        for (int i = idx; i < taskCount - 1; i++)
        {
            taskTexts[i] = taskTexts[i + 1];
            taskStatuses[i] = taskStatuses[i + 1];
            taskDates[i] = taskDates[i + 1];
        }
        taskCount--;
        Console.WriteLine($"Задача [{index}] удалена.");
    }

    private static void HandleUpdateCommand(string command)
    {
        string[] parts = command.Split(' ', 3);
        if (parts.Length < 3) { Console.WriteLine("Ошибка: используйте формат update <индекс> \"новый текст\""); return; }
        if (!int.TryParse(parts[1], out int index) || index < 1 || index > taskCount) { Console.WriteLine("Ошибка: некорректный индекс задачи."); return; }
        string[] quotedParts = parts[2].Split('"');
        if (quotedParts.Length < 2) { Console.WriteLine("Ошибка: используйте кавычки вокруг текста задачи."); return; }
        string newText = quotedParts[1];
        int idx = index - 1;
        taskTexts[idx] = newText;
        taskDates[idx] = DateTime.Now;
        Console.WriteLine($"Задача [{index}] обновлена: \"{newText}\"");
    }

    private static void HandleReadCommand(string command)
    {
        if (!TryParseIndex(command, out int index)) return;
        if (index < 1 || index > taskCount) { Console.WriteLine("Ошибка: некорректный индекс задачи."); return; }
        int idx = index - 1;
        string statusText = taskStatuses[idx] ? "сделано" : "не сделано";
        Console.WriteLine($"Задача [{index}]: {taskTexts[idx]}");
        Console.WriteLine($"Статус: {statusText}");
        Console.WriteLine($"Дата последнего изменения: {taskDates[idx]}");
    }

    // === Вспомогательные методы ===

    private static void ExpandArraysIfNeeded()
    {
        if (taskCount < taskTexts.Length) return;
        int newSize = taskTexts.Length * 2;
        string[] newTaskTexts = new string[newSize];
        bool[] newTaskStatuses = new bool[newSize];
        DateTime[] newTaskDates = new DateTime[newSize];
        for (int i = 0; i < taskCount; i++)
        {
            newTaskTexts[i] = taskTexts[i];
            newTaskStatuses[i] = taskStatuses[i];
            newTaskDates[i] = taskDates[i];
        }
        taskTexts = newTaskTexts;
        taskStatuses = newTaskStatuses;
        taskDates = newTaskDates;
    }

    private static bool TryParseIndex(string command, out int index)
    {
        index = -1;
        string[] parts = command.Split(' ');
        if (parts.Length < 2 || !int.TryParse(parts[1], out index))
        {
            Console.WriteLine("Ошибка: укажите индекс задачи, например done 1");
            return false;
        }
        return true;
    }
}
