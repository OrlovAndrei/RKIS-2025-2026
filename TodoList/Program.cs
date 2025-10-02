using System;

class Program
{
        // Массив в 2 элемента
        static string[] todos = new string[2];
        static string firstName = "";
        static string lastName = "";
        static int birthYear = 0;
        static void Main()
    {
        // Запрос данных
        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        string yearInput = Console.ReadLine();

        // Перевод года рождения
        int birthYear = int.Parse(yearInput);
        int currentYear = DateTime.Now.Year;
        int age = currentYear - birthYear;
        
        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

        RunTodoApp();

    }
    static void RunTodoApp()
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
        int age = DateTime.Now.Year - birthYear;
        Console.WriteLine($"{firstName} {lastName}, {birthYear} (возраст: {age})");
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
        
        // Ищем место в массиве
        int emptyIndex = -1;
        for (int i = 0; i < todos.Length; i++)
        {
            if (string.IsNullOrEmpty(todos[i]))
            {
                emptyIndex = i;
                break;
            }
        }
        
        // Если нет, то расширяем массив
        if (emptyIndex == -1)
        {
            ExpandTodosArray();
            emptyIndex = todos.Length / 2; // Первый элемент 
        }
        
        todos[emptyIndex] = todoText;
        Console.WriteLine($"Задача добавлена: {todoText}");
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
        
        todos = newTodos;
        Console.WriteLine($"Массив задач расширен до {newSize} элементов");
    }
}