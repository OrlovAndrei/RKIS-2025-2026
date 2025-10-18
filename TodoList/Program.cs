using System;
using System.Collections.Generic;
using System.Linq;

class TodoList
{
    static void Main()
    {
        Console.WriteLine("выполнил работу Турищев Иван");
        int yaerNow = DateTime.Now.Year;
        System.Console.WriteLine(yaerNow);
        System.Console.Write("Введите ваше имя: ");
        string userName = Console.ReadLine() ?? "Неизвестно";
        if (userName.Length == 0) userName = "Неизвестно";
        System.Console.Write($"{userName}, введите год вашего рождения: ");
        string yaerBirth = Console.ReadLine() ?? "Неизвестно";
        if (yaerBirth == "") yaerBirth = "Неизвестно";
        int age = -1;
        if (int.TryParse(yaerBirth, out age) && age < yaerNow)
        {
            System.Console.WriteLine($"Добавлен пользователь {userName}, возрастом {yaerNow-age}");
        }
        else System.Console.WriteLine("Пользователь не ввел возраст");

        // Изменено: используем массивы вместо List
        string[] tasks = new string[0];
        bool[] statuses = new bool[0];
        DateTime[] dates = new DateTime[0];
        
        string[] nameParts = userName.Split(' ');
        string firstName = nameParts[0];
        string lastName = nameParts.Length > 1 ? nameParts[1] : "Неизвестно";
        
        bool isRunning = true;
        
        Console.WriteLine("\nДобро пожаловать в TodoList! Введите 'help' для списка команд.");
        
        while (isRunning)
        {
            Console.Write("\n> ");
            string input = Console.ReadLine()?.Trim() ?? "";
            
            if (string.IsNullOrWhiteSpace(input))
                continue;
                
            var parsedCommand = ParseCommand(input);
            string command = parsedCommand.Command;
            List<string> flags = parsedCommand.Flags;
            string argument = parsedCommand.Argument;
            
            switch (command)
            {
                case "help":
                    ShowHelp();
                    break;
                    
                case "profile":
                    ShowProfile(firstName, lastName, yaerBirth);
                    break;
                    
                case "add":
                    AddTask(ref tasks, ref statuses, ref dates, flags, argument);
                    break;
                    
                case "view":
                    ViewTasks(tasks, statuses, dates, flags);
                    break;
                    
                case "complete":
                    CompleteTask(ref tasks, ref statuses, ref dates, flags);
                    break;
                    
                case "remove":
                    RemoveTask(ref tasks, ref statuses, ref dates, flags);
                    break;
                    
                case "edit":
                    EditTask(ref tasks, ref statuses, ref dates, flags);
                    break;
                    
                case "done":
                    MarkTaskDone(ref tasks, ref statuses, ref dates, flags, argument);
                    break;
                    
                case "delete":
                    DeleteTask(ref tasks, ref statuses, ref dates, flags, argument);
                    break;
                    
                case "update":
                    UpdateTask(ref tasks, ref statuses, ref dates, flags, argument);
                    break;
                    
                case "read": // Исправлено: reed на read
                    ReadTask(tasks, statuses, dates, flags, argument);
                    break;
                    
                case "exit":
                    isRunning = false;
                    Console.WriteLine("Программа завершена. До свивания!");
                    break;
                    
                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка доступных команд.");
                    break;
            }
        }
    }
    
    struct ParsedCommand
    {
        public string Command { get; set; }
        public List<string> Flags { get; set; }
        public string Argument { get; set; }
    }
    
    static ParsedCommand ParseCommand(string input)
    {
        var parts = input.Split(' ');
        var parsed = new ParsedCommand
        {
            Command = parts[0].ToLower(),
            Flags = new List<string>(),
            Argument = ""
        };
        
        List<string> remainingParts = new List<string>();
        
        for (int i = 1; i < parts.Length; i++)
        {
            if (parts[i].StartsWith("--"))
            {
                parsed.Flags.Add(parts[i].ToLower());
            }
            else if (parts[i].StartsWith("-") && parts[i].Length > 1)
            {
                string shortFlags = parts[i].Substring(1);
                foreach (char flagChar in shortFlags)
                {
                    parsed.Flags.Add($"-{flagChar}");
                }
            }
            else
            {
                remainingParts.Add(parts[i]);
            }
        }
        
        parsed.Argument = string.Join(" ", remainingParts);
        return parsed;
    }
    
    static void ShowHelp()
    {
        Console.WriteLine("\nДоступные команды:");
        Console.WriteLine("help     - выводит список всех доступных команд с кратким описанием");
        Console.WriteLine("profile  - выводит данные пользователя");
        Console.WriteLine("add      - добавляет новую задачу. Формат: add \"текст задачи\"");
        Console.WriteLine("view     - выводит все задачи из списка. Флаги: -i (пронумерованный), -s (только статус), -d (только даты)");
        Console.WriteLine("complete - отмечает задачу как выполненную");
        Console.WriteLine("remove   - удаляет задачу");
        Console.WriteLine("edit     - редактирует текст задачи");
        Console.WriteLine("done     - отмечает задачу выполненной. Формат: done <индекс>");
        Console.WriteLine("delete   - удаляет задачу по индексу. Формат: delete <индекс>");
        Console.WriteLine("update   - обновляет текст задачи. Формат: update <индекс> \"новый текст\"");
        Console.WriteLine("read     - просмотр полного текста задачи. Формат: read <индекс>"); // Исправлено: reed на read
        Console.WriteLine("exit     - завершает программу");
        
        Console.WriteLine("\nФлаги команд:");
        Console.WriteLine("--multiline, -m - многострочный ввод для команды add (завершить ввод: !end)");
        Console.WriteLine("-i  - пронумерованный вывод (для view)");
        Console.WriteLine("-s  - показывать только статус (для view)");
        Console.WriteLine("-d  - показывать только даты (для view)");
        Console.WriteLine("-f  - принудительное выполнение (для delete)");
        Console.WriteLine("-l  - показывать длинный формат (для read)"); // Исправлено: reed на read
        Console.WriteLine("Комбинации: -is, -id, -sd и т.д.");
    }
    
    static void ShowProfile(string firstName, string lastName, string birthYear)
    {
        Console.WriteLine($"\n{firstName} {lastName}, {birthYear}");
    }
    
    static void AddTask(ref string[] tasks, ref bool[] statuses, ref DateTime[] dates, List<string> flags, string argument)
    {
        string task = "";
        
        // Проверяем флаг многострочного ввода
        if (flags.Contains("--multiline") || flags.Contains("-m"))
        {
            Console.WriteLine("Введите текст задачи (для завершения введите !end):");
            List<string> lines = new List<string>();
            string line;
            
            while (true)
            {
                line = Console.ReadLine()?.Trim() ?? "";
                if (line == "!end")
                    break;
                lines.Add(line);
            }
            
            task = string.Join("\n", lines);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.Write("Введите текст задачи: ");
                argument = Console.ReadLine()?.Trim() ?? "";
            }
            
            if (argument.StartsWith("\"") && argument.EndsWith("\""))
            {
                task = argument.Substring(1, argument.Length - 2);
            }
            else
            {
                task = argument;
            }
        }
        
        if (!string.IsNullOrWhiteSpace(task))
        {
            Array.Resize(ref tasks, tasks.Length + 1);
            Array.Resize(ref statuses, statuses.Length + 1);
            Array.Resize(ref dates, dates.Length + 1);
            
            tasks[tasks.Length - 1] = task;
            statuses[statuses.Length - 1] = false;
            dates[dates.Length - 1] = DateTime.Now;
            Console.WriteLine("Задача успешно добавлена!");
        }
        else
        {
            Console.WriteLine("Ошибка: текст задачи не может быть пустым");
        }
    }
    
    static void ViewTasks(string[] tasks, bool[] statuses, DateTime[] dates, List<string> flags)
    {
        if (tasks.Length == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        bool showNumbers = flags.Contains("-i");
        bool showStatus = !flags.Contains("-s");
        bool showDates = !flags.Contains("-d");
        
        foreach (var flag in flags.Where(f => f.Length == 2 && f[0] == '-'))
        {
            if (flag.Contains('i')) showNumbers = true;
            if (flag.Contains('s')) showStatus = false;
            if (flag.Contains('d')) showDates = false;
        }
        
        Console.WriteLine("\nСписок задач:");
        
        // Находим максимальную длину для выравнивания
        int maxTextLength = 30;
        int maxStatusLength = 12; // " | не сделано" = 12 символов
        
        for (int i = 0; i < tasks.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(tasks[i]))
            {
                string number = showNumbers ? $"{i + 1}. " : "";
                
                // Обрабатываем текст задачи - ограничиваем 30 символами
                string displayText = tasks[i].Replace("\n", " ");
                if (displayText.Length > maxTextLength)
                {
                    displayText = displayText.Substring(0, maxTextLength) + ":";
                }
                
                // Выравниваем текст до максимальной длины
                displayText = displayText.PadRight(maxTextLength + 1);
                
                // Форматируем статус
                string statusText = "";
                if (showStatus)
                {
                    statusText = statuses[i] ? " | сделано" : " | не сделано";
                    statusText = statusText.PadRight(maxStatusLength);
                }
                
                // Форматируем дату
                string dateInfo = "";
                if (showDates)
                {
                    dateInfo = $" | {dates[i]:dd.MM.yyyy HH:mm}";
                }
                
                Console.WriteLine($"{number}{displayText}{statusText}{dateInfo}");
            }
        }
    }
    
    static void ReadTask(string[] tasks, bool[] statuses, DateTime[] dates, List<string> flags, string argument)
    {
        if (tasks.Length == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        if (string.IsNullOrWhiteSpace(argument))
        {
            Console.WriteLine("Ошибка: укажите номер задачи. Формат: read <индекс>"); // Исправлено: reed на read
            return;
        }
        
        if (int.TryParse(argument, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Length)
        {
            int index = taskNumber - 1;
            
            string statusText = statuses[index] ? "сделано" : "не сделано";
            string dateInfo = dates[index].ToString("dd.MM.yyyy HH:mm");
            
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine($"ЗАДАЧА #{taskNumber}");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"Статус: {statusText}");
            Console.WriteLine($"Дата создания/изменения: {dateInfo}");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine("ПОЛНЫЙ ТЕКСТ ЗАДАЧИ:");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine(tasks[index]);
            Console.WriteLine(new string('=', 60));
        }
        else
        {
            Console.WriteLine("Ошибка: неверный номер задачи. Используйте: read <индекс>"); // Исправлено: reed на read
        }
    }
    
    static void CompleteTask(ref string[] tasks, ref bool[] statuses, ref DateTime[] dates, List<string> flags)
    {
        if (tasks.Length == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        ViewTasks(tasks, statuses, dates, new List<string>());
        Console.Write("Введите номер задачи для отметки как выполненной: ");
        
        if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Length)
        {
            int index = taskNumber - 1;
            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача '{tasks[index]}' отмечена как выполненная!");
        }
        else
        {
            Console.WriteLine("Ошибка: неверный номер задачи");
        }
    }
    
    static void RemoveTask(ref string[] tasks, ref bool[] statuses, ref DateTime[] dates, List<string> flags)
    {
        if (tasks.Length == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        ViewTasks(tasks, statuses, dates, new List<string>());
        Console.Write("Введите номер задачи для удаления: ");
        
        if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Length)
        {
            int index = taskNumber - 1;
            string removedTask = tasks[index];
            
            if (flags.Contains("-f"))
            {
                RemoveItem(ref tasks, index);
                RemoveItem(ref statuses, index);
                RemoveItem(ref dates, index);
                Console.WriteLine($"Задача '{removedTask}' успешно удалена!");
            }
            else
            {
                Console.Write($"Вы уверены, что хотите удалить задачу '{removedTask}'? (y/n): ");
                string confirmation = Console.ReadLine()?.ToLower() ?? "";
                if (confirmation == "y" || confirmation == "yes")
                {
                    RemoveItem(ref tasks, index);
                    RemoveItem(ref statuses, index);
                    RemoveItem(ref dates, index);
                    Console.WriteLine($"Задача '{removedTask}' успешно удалена!");
                }
                else
                {
                    Console.WriteLine("Удаление отменено");
                }
            }
        }
        else
        {
            Console.WriteLine("Ошибка: неверный номер задачи");
        }
    }
    
    static void RemoveItem<T>(ref T[] array, int index)
    {
        T[] newArray = new T[array.Length - 1];
        for (int i = 0; i < index; i++)
            newArray[i] = array[i];
        for (int i = index + 1; i < array.Length; i++)
            newArray[i - 1] = array[i];
        array = newArray;
    }
    
    static void EditTask(ref string[] tasks, ref bool[] statuses, ref DateTime[] dates, List<string> flags)
    {
        if (tasks.Length == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        ViewTasks(tasks, statuses, dates, new List<string>());
        Console.Write("Введите номер задачи для редактирования: ");
        
        if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Length)
        {
            int index = taskNumber - 1;
            
            Console.Write("Введите новый текст задачи: ");
            string newTask = Console.ReadLine()?.Trim() ?? "";
            
            if (!string.IsNullOrWhiteSpace(newTask))
            {
                tasks[index] = newTask;
                dates[index] = DateTime.Now;
                Console.WriteLine("Задача успешно отредактирована!");
            }
            else
            {
                Console.WriteLine("Ошибка: текст задачи не может быть пустым");
            }
        }
        else
        {
            Console.WriteLine("Ошибка: неверный номер задачи");
        }
    }
    
    static void MarkTaskDone(ref string[] tasks, ref bool[] statuses, ref DateTime[] dates, List<string> flags, string argument)
    {
        if (tasks.Length == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        if (int.TryParse(argument, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Length)
        {
            int index = taskNumber - 1;
            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача '{tasks[index]}' отмечена как выполненная!");
        }
        else
        {
            Console.WriteLine("Ошибка: неверный номер задачи. Используйте: done <индекс>");
        }
    }
    
    static void DeleteTask(ref string[] tasks, ref bool[] statuses, ref DateTime[] dates, List<string> flags, string argument)
    {
        if (tasks.Length == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        if (int.TryParse(argument, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Length)
        {
            int index = taskNumber - 1;
            string deletedTask = tasks[index];
            
            if (flags.Contains("-f"))
            {
                RemoveItem(ref tasks, index);
                RemoveItem(ref statuses, index);
                RemoveItem(ref dates, index);
                Console.WriteLine($"Задача '{deletedTask}' успешно удалена!");
            }
            else
            {
                Console.Write($"Вы уверены, что хотите удалить задачу '{deletedTask}'? (y/n): ");
                string confirmation = Console.ReadLine()?.ToLower() ?? "";
                if (confirmation == "y" || confirmation == "yes")
                {
                    RemoveItem(ref tasks, index);
                    RemoveItem(ref statuses, index);
                    RemoveItem(ref dates, index);
                    Console.WriteLine($"Задача '{deletedTask}' успешно удалена!");
                }
                else
                {
                    Console.WriteLine("Удаление отменено");
                }
            }
        }
        else
        {
            Console.WriteLine("Ошибка: неверный номер задачи. Используйте: delete <индекс>");
        }
    }
    
    static void UpdateTask(ref string[] tasks, ref bool[] statuses, ref DateTime[] dates, List<string> flags, string argument)
    {
        if (tasks.Length == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        string[] parts = argument.Split(' ', 2);
        if (parts.Length < 2)
        {
            Console.WriteLine("Ошибка: неправильный формат. Используйте: update <индекс> \"новый текст\"");
            return;
        }
        
        if (int.TryParse(parts[0], out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Length)
        {
            int index = taskNumber - 1;
            string newText = parts[1].Trim();
            
            if (newText.StartsWith("\"") && newText.EndsWith("\""))
            {
                string taskText = newText.Substring(1, newText.Length - 2);
                if (!string.IsNullOrWhiteSpace(taskText))
                {
                    tasks[index] = taskText;
                    dates[index] = DateTime.Now;
                    Console.WriteLine($"Задача успешно обновлена!");
                }
                else
                {
                    Console.WriteLine("Ошибка: текст задачи не может быть пустым");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: неправильный формат текста. Используйте кавычки: \"новый текст\"");
            }
        }
        else
        {
            Console.WriteLine("Ошибка: неверный номер задачи. Используйте: update <индекс> \"новый текст\"");
        }
    }
}
