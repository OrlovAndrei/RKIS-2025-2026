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

        // Добавленный код с командами
        List<string> tasks = new List<string>();
        List<bool> statuses = new List<bool>(); // true - выполнено, false - не выполнено
        List<DateTime> dates = new List<DateTime>(); // дата создания/изменения задачи
        
        string[] nameParts = userName.Split(' ');
        string firstName = nameParts[0];
        string lastName = nameParts.Length > 1 ? nameParts[1] : "Неизвестно";
        
        bool isRunning = true;
        
        Console.WriteLine("\nДобро пожаловать в TodoList! Введите 'help' для списка команд.");
        
        while (isRunning)
        {
            Console.Write("\nВведите команду: ");
            string input = Console.ReadLine()?.Trim() ?? "";
            
            if (string.IsNullOrWhiteSpace(input))
                continue;
                
            // Парсинг команды с учетом флагов
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
                    AddTask(tasks, statuses, dates, flags, argument);
                    break;
                    
                case "view":
                    ViewTasks(tasks, statuses, dates, flags);
                    break;
                    
                case "complete":
                    CompleteTask(tasks, statuses, dates, flags);
                    break;
                    
                case "remove":
                    RemoveTask(tasks, statuses, dates, flags);
                    break;
                    
                case "edit":
                    EditTask(tasks, statuses, dates, flags);
                    break;
                    
                case "done":
                    MarkTaskDone(tasks, statuses, dates, flags, argument);
                    break;
                    
                case "delete":
                    DeleteTask(tasks, statuses, dates, flags, argument);
                    break;
                    
                case "update":
                    UpdateTask(tasks, statuses, dates, flags, argument);
                    break;
                    
                case "reed": // Новая команда для просмотра полного текста задачи
                    ReadTask(tasks, statuses, dates, flags, argument);
                    break;
                    
                case "exit":
                    isRunning = false;
                    Console.WriteLine("Программа завершена. До свидания!");
                    break;
                    
                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка доступных команд.");
                    break;
            }
        }
    }
    
    // Структура для хранения разобранной команды
    struct ParsedCommand
    {
        public string Command { get; set; }
        public List<string> Flags { get; set; }
        public string Argument { get; set; }
    }
    
    // Метод для парсинга команды с флагами
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
        
        // Обрабатываем флаги
        for (int i = 1; i < parts.Length; i++)
        {
            if (parts[i].StartsWith("--"))
            {
                // Длинные флаги (--flag)
                parsed.Flags.Add(parts[i].ToLower());
            }
            else if (parts[i].StartsWith("-") && parts[i].Length > 1)
            {
                // Короткие флаги (-f или -abc)
                string shortFlags = parts[i].Substring(1);
                foreach (char flagChar in shortFlags)
                {
                    parsed.Flags.Add($"-{flagChar}");
                }
            }
            else
            {
                // Все остальное - аргументы
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
        Console.WriteLine("add      - добавляет новую задачу. Формат: add [--multiline] \"текст задачи\"");
        Console.WriteLine("view     - выводит все задачи из списка. Флаги: -i (пронумерованный), -s (только статус), -d (только даты)");
        Console.WriteLine("complete - отмечает задачу как выполненную");
        Console.WriteLine("remove   - удаляет задачу");
        Console.WriteLine("edit     - редактирует текст задачи");
        Console.WriteLine("done     - отмечает задачу выполненной. Формат: done <индекс>");
        Console.WriteLine("delete   - удаляет задачу по индексу. Формат: delete <индекс>");
        Console.WriteLine("update   - обновляет текст задачи. Формат: update <индекс> \"новый текст\"");
        Console.WriteLine("reed     - просмотр полного текста задачи. Формат: reed <индекс>");
        Console.WriteLine("exit     - завершает программу");
        
        Console.WriteLine("\nФлаги команд:");
        Console.WriteLine("--multiline  - многострочный ввод для команды add");
        Console.WriteLine("-i           - пронумерованный вывод (для view)");
        Console.WriteLine("-s           - показывать только статус (для view)");
        Console.WriteLine("-d           - показывать только даты (для view)");
        Console.WriteLine("-f           - принудительное выполнение (для delete)");
        Console.WriteLine("-l           - показывать длинный формат (для reed)");
        Console.WriteLine("Комбинации:  -is, -id, -sd и т.д.");
    }
    
    static void ShowProfile(string firstName, string lastName, string birthYear)
    {
        Console.WriteLine($"\n{firstName} {lastName}, {birthYear}");
    }
    
    static void AddTask(List<string> tasks, List<bool> statuses, List<DateTime> dates, List<string> flags, string argument)
    {
        string task = "";
        
        if (flags.Contains("--multiline"))
        {
            Console.WriteLine("Введите текст задачи (для завершения введите пустую строку):");
            List<string> lines = new List<string>();
            string line;
            
            while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
            {
                lines.Add(line);
            }
            
            task = string.Join(Environment.NewLine, lines);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.Write("Введите текст задачи (в кавычках): ");
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
            tasks.Add(task);
            statuses.Add(false);
            dates.Add(DateTime.Now);
            Console.WriteLine("Задача успешно добавлена!");
        }
        else
        {
            Console.WriteLine("Ошибка: текст задачи не может быть пустым");
        }
    }
    
    static void ViewTasks(List<string> tasks, List<bool> statuses, List<DateTime> dates, List<string> flags)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        bool showNumbers = flags.Contains("-i") || flags.Contains("--numbered");
        bool showStatus = !flags.Contains("-s"); // -s инвертирует показ статуса
        bool showDates = !flags.Contains("-d");  // -d инвертирует показ дат
        
        // Если указаны комбинированные флаги, разбираем их
        if (flags.Any(f => f.Length == 2 && f[0] == '-' && f[1] != '-'))
        {
            foreach (var flag in flags.Where(f => f.Length == 2 && f[0] == '-'))
            {
                if (flag.Contains('i')) showNumbers = true;
                if (flag.Contains('s')) showStatus = false;
                if (flag.Contains('d')) showDates = false;
            }
        }
        
        Console.WriteLine("\nСписок задач:");
        for (int i = 0; i < tasks.Count; i++)
        {
            if (!string.IsNullOrWhiteSpace(tasks[i]))
            {
                string number = showNumbers ? $"{i + 1}. " : "";
                string statusText = showStatus ? (statuses[i] ? " | сделано" : " | не сделано") : "";
                string dateInfo = showDates ? $" | {dates[i]:dd.MM.yyyy HH:mm}" : "";
                
                // Обрезаем длинный текст для краткого просмотра
                string displayText = tasks[i];
                if (displayText.Length > 50 && !flags.Contains("--full"))
                {
                    displayText = displayText.Substring(0, 47) + "...";
                }
                
                // Заменяем переносы строк для компактного отображения
                displayText = displayText.Replace("\r\n", " ").Replace("\n", " ");
                
                Console.WriteLine($"{number}{displayText}{statusText}{dateInfo}");
            }
        }
    }
    
    // Новая команда для просмотра полного текста задачи
    static void ReadTask(List<string> tasks, List<bool> statuses, List<DateTime> dates, List<string> flags, string argument)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        if (string.IsNullOrWhiteSpace(argument))
        {
            Console.WriteLine("Ошибка: укажите номер задачи. Формат: reed <индекс>");
            return;
        }
        
        if (int.TryParse(argument, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count)
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
            
            // Дополнительная информация при использовании флага -l
            if (flags.Contains("-l") || flags.Contains("--long"))
            {
                Console.WriteLine($"Длина текста: {tasks[index].Length} символов");
                Console.WriteLine($"Количество строк: {tasks[index].Split('\n').Length}");
                TimeSpan timeSinceCreation = DateTime.Now - dates[index];
                Console.WriteLine($"Задача создана: {timeSinceCreation.Days} дней назад");
            }
        }
        else
        {
            Console.WriteLine("Ошибка: неверный номер задачи. Используйте: reed <индекс>");
        }
    }
    
    static void CompleteTask(List<string> tasks, List<bool> statuses, List<DateTime> dates, List<string> flags)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        ViewTasks(tasks, statuses, dates, new List<string>());
        Console.Write("Введите номер задачи для отметки как выполненной: ");
        
        if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count)
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
    
    static void RemoveTask(List<string> tasks, List<bool> statuses, List<DateTime> dates, List<string> flags)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        ViewTasks(tasks, statuses, dates, new List<string>());
        Console.Write("Введите номер задачи для удаления: ");
        
        if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count)
        {
            int index = taskNumber - 1;
            string removedTask = tasks[index];
            
            // Проверка флага принудительного удаления
            if (flags.Contains("-f") || flags.Contains("--force"))
            {
                // Принудительное удаление без подтверждения
                tasks.RemoveAt(index);
                statuses.RemoveAt(index);
                dates.RemoveAt(index);
                Console.WriteLine($"Задача '{removedTask}' успешно удалена!");
            }
            else
            {
                // Обычное удаление с подтверждением
                Console.Write($"Вы уверены, что хотите удалить задачу '{removedTask}'? (y/n): ");
                string confirmation = Console.ReadLine()?.ToLower() ?? "";
                if (confirmation == "y" || confirmation == "yes")
                {
                    tasks.RemoveAt(index);
                    statuses.RemoveAt(index);
                    dates.RemoveAt(index);
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
    
    static void EditTask(List<string> tasks, List<bool> statuses, List<DateTime> dates, List<string> flags)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        ViewTasks(tasks, statuses, dates, new List<string>());
        Console.Write("Введите номер задачи для редактирования: ");
        
        if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count)
        {
            int index = taskNumber - 1;
            
            string newTask = "";
            if (flags.Contains("--multiline"))
            {
                Console.WriteLine("Введите новый текст задачи (для завершения введите пустую строку):");
                List<string> lines = new List<string>();
                string line;
                
                while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
                {
                    lines.Add(line);
                }
                
                newTask = string.Join(Environment.NewLine, lines);
            }
            else
            {
                Console.Write("Введите новый текст задачи (в кавычках): ");
                string input = Console.ReadLine()?.Trim() ?? "";
                
                if (input.StartsWith("\"") && input.EndsWith("\""))
                {
                    newTask = input.Substring(1, input.Length - 2);
                }
                else
                {
                    newTask = input;
                }
            }
            
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
    
    static void MarkTaskDone(List<string> tasks, List<bool> statuses, List<DateTime> dates, List<string> flags, string argument)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        if (int.TryParse(argument, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count)
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
    
    static void DeleteTask(List<string> tasks, List<bool> statuses, List<DateTime> dates, List<string> flags, string argument)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        if (int.TryParse(argument, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count)
        {
            int index = taskNumber - 1;
            string deletedTask = tasks[index];
            
            // Проверка флага принудительного удаления
            if (flags.Contains("-f") || flags.Contains("--force"))
            {
                // Принудительное удаление без подтверждения
                tasks.RemoveAt(index);
                statuses.RemoveAt(index);
                dates.RemoveAt(index);
                Console.WriteLine($"Задача '{deletedTask}' успешно удалена!");
            }
            else
            {
                // Обычное удаление с подтверждением
                Console.Write($"Вы уверены, что хотите удалить задачу '{deletedTask}'? (y/n): ");
                string confirmation = Console.ReadLine()?.ToLower() ?? "";
                if (confirmation == "y" || confirmation == "yes")
                {
                    tasks.RemoveAt(index);
                    statuses.RemoveAt(index);
                    dates.RemoveAt(index);
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
    
    static void UpdateTask(List<string> tasks, List<bool> statuses, List<DateTime> dates, List<string> flags, string argument)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        // Разделяем индекс и новый текст
        string[] parts = argument.Split(' ', 2);
        if (parts.Length < 2)
        {
            Console.WriteLine("Ошибка: неправильный формат. Используйте: update <индекс> \"новый текст\"");
            return;
        }
        
        if (int.TryParse(parts[0], out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count)
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
