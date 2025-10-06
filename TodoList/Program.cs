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
        private static int nextTodoIndex = 0; // Индекс для следующей задачи
        
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

        // Перевод года рождения
        _birthYear = int.Parse(yearInput);
        int currentYear = DateTime.Now.Year;
        int age = currentYear - _birthYear;
        
        Console.WriteLine($"Добавлен пользователь {_firstName} {_lastName}, возраст - {age}");
    }
    static void RunTodoApplication()
    {
        Console.WriteLine("Добро пожаловать в TodoList! Введите 'help' для списка команд.");

        while (true)
        {
            Console.Write("> ");
            string command = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(command))
                continue;

            ProcessCommand(command.ToLower());
        }
    }
    static void ProcessCommand(string command)
    {
        switch (command)
        {
            case "help":
                ShowHelp();
                break;
            case "profile":
                ShowProfile();
                break;
            case "view":
                ViewTodos();
                break;
            case "exit":
                Environment.Exit(0);
                break;
            default:
                if (command.StartsWith("add"))
                {
                    AddTodo(command);
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                }
                break;
        }
    }
    static void ShowHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("help - вывести список команд");
        Console.WriteLine("profile - показать данные пользователя");
        Console.WriteLine("add - добавить задачу (формат: add \"текст задачи\")");
        Console.WriteLine("view - показать все задачи");
        Console.WriteLine("exit - выйти из программы");
    }
    static void ShowProfile()
    {
        int age = DateTime.Now.Year - _birthYear;
        Console.WriteLine($"{_firstName} {_lastName}, {_birthYear} (возраст: {age})");
    }

        static void AddTodo(string command)
    {
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
        if (nextTodoIndex >= todos.Length)
        {
            ExpandTodosArray();
        }
        
        todos[nextTodoIndex] = todoText;
        Console.WriteLine($"Задача добавлена: {todoText} (всего задач: {nextTodoIndex + 1})");
        nextTodoIndex++; // Увеличиваем индекс
    }
    static void ExpandTodosArray()
    {
        int newSize = todos.Length * 2;
        string[] newTodos = new string[newSize];
        
        // Копируем существующие задачи
        for (int i = 0; i < todos.Length; i++)
        {
            newTodos[i] = todos[i];
        }
        todos = newTodos; // Присвоение нового массива
    }
        static void ViewTodos()
    {
         if (nextTodoIndex == 0)
        {
            Console.WriteLine("Задач нет.");
            return;
        }
        
        Console.WriteLine("Список задач:");
        for (int i = 0; i < nextTodoIndex; i++) // Заменила nextTodoIndex вместо todos.Length
        {
            Console.WriteLine($"{i + 1}. {todos[i]}");
        }
    }
}