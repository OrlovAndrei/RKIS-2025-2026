using System;
using System.Collections.Generic;

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
        string[] nameParts = userName.Split(' ');
        string firstName = nameParts[0];
        string lastName = nameParts.Length > 1 ? nameParts[1] : "Неизвестно";
        
        bool isRunning = true;
        
        Console.WriteLine("\nДобро пожаловать в TodoList! Введите 'help' для списка команд.");
        
        while (isRunning)
        {
            Console.Write("\nВведите команду: ");
            string command = Console.ReadLine()?.ToLower().Trim() ?? "";
            
            switch (command)
            {
                case "help":
                    ShowHelp();
                    break;
                    
                case "profile":
                    ShowProfile(firstName, lastName, yaerBirth);
                    break;
                    
                case "add":
                    AddTask(tasks);
                    break;
                    
                case "view":
                    ViewTasks(tasks);
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
    
    static void ShowHelp()
    {
        Console.WriteLine("\nДоступные команды:");
        Console.WriteLine("help    - выводит список всех доступных команд с кратким описанием");
        Console.WriteLine("profile - выводит данные пользователя");
        Console.WriteLine("add     - добавляет новую задачу. Формат: add \"текст задачи\"");
        Console.WriteLine("view    - выводит все задачи из списка");
        Console.WriteLine("exit    - завершает программу");
    }
    
    static void ShowProfile(string firstName, string lastName, string birthYear)
    {
        Console.WriteLine($"\n{firstName} {lastName}, {birthYear}");
    }
    
    static void AddTask(List<string> tasks)
    {
        Console.Write("Введите текст задачи (в кавычках): ");
        string input = Console.ReadLine()?.Trim() ?? "";
        
        if (input.StartsWith("\"") && input.EndsWith("\""))
        {
            string task = input.Substring(1, input.Length - 2);
            if (!string.IsNullOrWhiteSpace(task))
            {
                tasks.Add(task);
                Console.WriteLine("Задача успешно добавлена!");
            }
            else
            {
                Console.WriteLine("Ошибка: текст задачи не может быть пустым");
            }
        }
        else
        {
            Console.WriteLine("Ошибка: неправильный формат. Используйте: add \"текст задачи\"");
        }
    }
    
    static void ViewTasks(List<string> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        Console.WriteLine("\nСписок задач:");
        for (int i = 0; i < tasks.Count; i++)
        {
            if (!string.IsNullOrWhiteSpace(tasks[i]))
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
        }
    }
}
