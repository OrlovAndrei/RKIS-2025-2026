using System;

class Program
{
        private const int InitialArraySize = 2;

        private static string[] _todos = new string[InitialArraySize];
        private static bool[] _statuses = new bool[InitialArraySize];
        private static DateTime[] _dates = new DateTime[InitialArraySize];

        // Массив в 2 элемента
        private static string _firstName = "";
        private static string _lastName = "";
        private static int _birthYear = 0;
        private static int _nextTodoIndex = 0; // Индекс для следующей задачи
        
        static void Main()
    {
        InitializeUserProfile();
        RunTodoApplication();
    }
    static void InitializeUserProfile()
    {
        // Запрос данных
        Console.Write("Введите имя: ");
        _firstName = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        _lastName = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        string yearInput = Console.ReadLine();

        if (!int.TryParse(yearInput, out _birthYear))
    {
        Console.WriteLine("Неверный формат года. Установлен 2000 год по умолчанию.");
        _birthYear = 2000;
    }
    
    int currentYear = DateTime.Now.Year;
    int age = currentYear - _birthYear;
    
    Console.WriteLine($"Добавлен пользователь {_firstName} {_lastName}, возраст - {age}");
    }
    static void RunTodoApplication()
    {
        Console.WriteLine("Добро пожаловать в TodoList! Введите 'help' для списка команд.");

        while (true)
        {
            string command = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(command))
                continue;

            ProcessCommand(command.ToLower());
        }
    }
    static void ProcessCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
            return;

          switch (command.Split(' ')[0])
        {
        case "add":
            AddTodo(command);
            break;
        case "done":
            MarkTaskAsDone(command);
            break;
        case "delete":
            DeleteTask(command);
            break;
        case "update":
            UpdateTask(command);
            break;
        case "view":
            string flags = command.Length > 4 ? command.Substring(4).Trim() : "";
            ViewTodos(flags);
            break;
        case "read":
            ReadTask(command);
            break;
        case "help":
            ShowHelp();
            break;
        case "profile":
            ShowProfile();
            break;
        case "exit":
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
            break;
        }
           
    }

    static void ShowHelp()
    {
    Console.WriteLine(@"СПРАВКА ПО КОМАНДАМ:
    help                    - вывести список команд
    profile                 - показать данные пользователя
    add ""текст""            - добавить задачу
    add --multiline (-m)    - добавить задачу в многострочном режиме
    view                    - показать только текст задач
    view --index (-i)       - показать с индексами
    view --status (-s)      - показать со статусами
    view --update-date (-d) - показать с датами
    view --all (-a)         - показать всю информацию
    read <номер>            - просмотреть полный текст задачи
    done <номер>            - отметить задачу выполненной
    delete <номер>          - удалить задачу
    update <номер> ""текст"" - обновить текст задачи
    exit                    - выйти из программы");
    }
    
    static void ShowProfile()
    {
        int age = DateTime.Now.Year - _birthYear;
        Console.WriteLine($"{_firstName} {_lastName}, {_birthYear} (возраст: {age})");
    }

        static void AddTodo(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
        {
        Console.WriteLine("Команда не может быть пустой.");
        return;
        }
        if (command.Contains("--multiline") || command.Contains("-m"))
        {
        AddTodoMultiline();
        return;
        }
        // Извлекаем текст 
        string[] parts = command.Split('"');
        if (parts.Length < 2)
        {
            Console.WriteLine("Неверный формат. Используйте: add \"текст задачи\"");
            return;
        }
        
        string todoText = parts[1].Trim();
        if (string.IsNullOrWhiteSpace(todoText))
        {
            Console.WriteLine("Текст задачи не может быть пустым.");
            return;
        }
        
        // Используем вместо поиска пустого места
        if (_nextTodoIndex >= _todos.Length)
        {
            ExpandAllArray();
        }
        
        _todos[_nextTodoIndex] = todoText;
        _statuses[_nextTodoIndex] = false; 
        _dates[_nextTodoIndex] = DateTime.Now;
        
        Console.WriteLine($"Задача добавлена: {todoText} (всего задач: {_nextTodoIndex + 1})");
        _nextTodoIndex++; // Увеличиваем индекс
    }
    static void AddTodoMultiline()
    {
    Console.WriteLine("Введите текст задачи (для завершения введите !end):");
    
    string multilineText = "";
    while (true)
    {
        Console.Write("> ");
        string line = Console.ReadLine();
        
        if (line == null)
            continue;

        if (line == "!end")
            break;
            
        if (!string.IsNullOrEmpty(multilineText))
            multilineText += "\n";
            
        multilineText += line;
    }
    
    if (string.IsNullOrWhiteSpace(multilineText))
    {
        Console.WriteLine("Текст задачи не может быть пустым.");
        return;
    }
    
    if (_nextTodoIndex >= _todos.Length)
    {
        ExpandAllArray();
    }
    
    _todos[_nextTodoIndex] = multilineText;
    _statuses[_nextTodoIndex] = false; 
    _dates[_nextTodoIndex] = DateTime.Now;
    
    Console.WriteLine($"Многострочная задача добавлена (всего задач: {_nextTodoIndex + 1})");
    _nextTodoIndex++;
    }
    static void ExpandAllArray()
    {
        int newSize = _todos.Length * 2;
        Array.Resize(ref _todos, newSize);
        Array.Resize(ref _statuses, newSize);
        Array.Resize(ref _dates, newSize);
    }
        static void ViewTodos(string flags)
    {
    if (_nextTodoIndex == 0)
    {
        Console.WriteLine("Задач нет.");
        return;
    }
    
    //Обработка флагов для view
    bool showAll = flags.Contains("-a") || flags.Contains("--all");
    bool showIndex = flags.Contains("--index") || flags.Contains("-i") || showAll;
    bool showStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
    bool showDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;

    if (flags.Contains("-") && flags.Length > 1 && !flags.Contains("--"))
    {
        string shortFlags = flags.Replace("-", "").Replace(" ", "");
        showIndex = showIndex || shortFlags.Contains("i");
        showStatus = showStatus || shortFlags.Contains("s");
        showDate = showDate || shortFlags.Contains("d");
        if (shortFlags.Contains("a"))
        {
            showIndex = true;
            showStatus = true;
            showDate = true;
        }
    }
    
    if (!showIndex && !showStatus && !showDate)
    {
        Console.WriteLine("Список задач:");
        for (int i = 0; i < _nextTodoIndex; i++)
        {
            string shortText = GetShortText(_todos[i], 30);
            Console.WriteLine($"{i + 1}. {shortText}");
        }
        return;
    }
    
    Console.WriteLine("Список задач:");
    
    string header = "";
    if (showIndex) header += "№    ";
    header += "Текст задачи                     ";
    if (showStatus) header += "Статус      ";
    if (showDate) header += "Дата изменения    ";
    
    Console.WriteLine(header);
    Console.WriteLine(new string('-', header.Length));
    
    for (int i = 0; i < _nextTodoIndex; i++)
    {
        string line = "";
        
        if (showIndex)
            line += $"{i + 1,-4} ";
            
        string shortText = GetShortText(_todos[i], 30);
        line += $"{shortText,-30}";
        
        if (showStatus)
        {
            string status = _statuses[i] ? "Сделано" : "Не сделано";
            line += $" {status,-10}";
        }
        
        if (showDate)
        {
            string date = _dates[i].ToString("dd.MM.yyyy HH:mm");
            line += $" {date}";
        }
        
        Console.WriteLine(line);
    }
    }
    //помогающий метод для обрезки текста
    static string GetShortText(string text, int maxLength)
    {
    if (string.IsNullOrEmpty(text))
        return "";
        
    if (text.Length <= maxLength)
        return text;

    return text.Substring(0, maxLength - 3) + "...";
    }
    static void MarkTaskAsDone(string command)
    {
        string[] parts = command.Split(' ');
        if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
        {
            Console.WriteLine("Неверный формат. Используйте: done <номер_задачи>");
            return;
        }

        int taskIndex = taskNumber - 1;
        if (taskIndex < 0 || taskIndex >= _nextTodoIndex)
        {
            Console.WriteLine($"Задачи с номером {taskNumber} не существует.");
            return;
        }
        // Обновление статуса и даты
        _statuses[taskIndex] = true;
        _dates[taskIndex] = DateTime.Now;
        Console.WriteLine($"Задача '{_todos[taskIndex]}' отмечена как выполненная");
    }
    static void DeleteTask(string command)
    {
    string[] parts = command.Split(' ');
    if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
        {
        Console.WriteLine("Неверный формат. Используйте: delete <номер_задачи>");
        return;
        }
    
    int taskIndex = taskNumber - 1;
    if (taskIndex < 0 || taskIndex >= _nextTodoIndex)
        {
        Console.WriteLine($"Задачи с номером {taskNumber} не существует.");
        return;
        }
    
    
    for (int i = taskIndex; i < _nextTodoIndex - 1; i++)
        {
        _todos[i] = _todos[i + 1];
        _statuses[i] = _statuses[i + 1];
        _dates[i] = _dates[i + 1];
        }
    
    _nextTodoIndex--;
    Console.WriteLine($"Задача удалена");
    }
    static void UpdateTask(string command)
    {
    if (string.IsNullOrWhiteSpace(command))
    {
        Console.WriteLine("Команда не может быть пустой.");
        return;
    }

    string[] parts = command.Split('"');
    if (parts.Length < 2)
    {
        Console.WriteLine("Неверный формат. Используйте: update <номер> \"новый текст\"");
        return;
    }
    
    string indexPart = parts[0].Replace("update", "").Trim();
    if (!int.TryParse(indexPart, out int taskNumber))
    {
        Console.WriteLine("Неверный номер задачи.");
        return;
    }
    
    int taskIndex = taskNumber - 1;
    if (taskIndex < 0 || taskIndex >= _nextTodoIndex)
    {
        Console.WriteLine($"Задачи с номером {taskNumber} не существует.");
        return;
    }
    
    string newText = parts[1].Trim();
    if (string.IsNullOrWhiteSpace(newText))
    {
        Console.WriteLine("Текст задачи не может быть пустым.");
        return;
    }
    
    // Обновление текста и даты
    _todos[taskIndex] = newText; 
    _dates[taskIndex] = DateTime.Now; 
    Console.WriteLine($"Задача обновлена");
    }
    static void ReadTask(string command)
    {
    string[] parts = command.Split(' ');
    if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
    {
        Console.WriteLine("Неверный формат. Используйте: read <номер_задачи>");
        return;
    }

    int taskIndex = taskNumber - 1;
    if (taskIndex < 0 || taskIndex >= _nextTodoIndex)
    {
        Console.WriteLine($"Задачи с номером {taskNumber} не существует.");
        return;
    }

    Console.WriteLine($"=== Задача #{taskNumber} ===");
    Console.WriteLine($"Текст: {_todos[taskIndex]}");
    Console.WriteLine($"Статус: {(_statuses[taskIndex] ? "Выполнена" : "Не выполнена")}");
    Console.WriteLine($"Дата изменения: {_dates[taskIndex].ToString("dd.MM.yyyy HH:mm")}");
    }
}