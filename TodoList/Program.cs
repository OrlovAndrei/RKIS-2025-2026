using System;
using System.Collections.Generic;

class TodoList
{
    static void Main()
    {
        Console.WriteLine("выполнил работу Турищев Иван Талышева Полина");
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
                    AddTask(tasks, statuses, dates);
                    break;
                    
                case "view":
                    ViewTasks(tasks, statuses, dates);
                    break;
                    
                case "complete":
                    CompleteTask(tasks, statuses, dates);
                    break;
                    
                case "remove":
                    RemoveTask(tasks, statuses, dates);
                    break;
                    
                case "edit":
                    EditTask(tasks, statuses, dates);
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
        Console.WriteLine("help     - выводит список всех доступных команд с кратким описанием");
        Console.WriteLine("profile  - выводит данные пользователя");
        Console.WriteLine("add      - добавляет новую задачу. Формат: add \"текст задачи\"");
        Console.WriteLine("view     - выводит все задачи из списка");
        Console.WriteLine("complete - отмечает задачу как выполненную");
        Console.WriteLine("remove   - удаляет задачу");
        Console.WriteLine("edit     - редактирует текст задачи");
        Console.WriteLine("exit     - завершает программу");
    }
    
    static void ShowProfile(string firstName, string lastName, string birthYear)
    {
        Console.WriteLine($"\n{firstName} {lastName}, {birthYear}");
    }
    
    static void AddTask(List<string> tasks, List<bool> statuses, List<DateTime> dates)
    {
        Console.Write("Введите текст задачи (в кавычках): ");
        string input = Console.ReadLine()?.Trim() ?? "";
        
        if (input.StartsWith("\"") && input.EndsWith("\""))
        {
            string task = input.Substring(1, input.Length - 2);
            if (!string.IsNullOrWhiteSpace(task))
            {
                // Одновременное добавление во все три массива
                tasks.Add(task);
                statuses.Add(false); // записывает значение false
                dates.Add(DateTime.Now); // записывает текущую дату
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
    
    static void ViewTasks(List<string> tasks, List<bool> statuses, List<DateTime> dates)
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
                string statusText = statuses[i] ? "сделано" : "не сделано";
                string dateInfo = dates[i].ToString("dd.MM.yyyy HH:mm");
                Console.WriteLine($"{i + 1}. {tasks[i]} | {statusText} | {dateInfo}");
            }
        }
    }
    
    static void CompleteTask(List<string> tasks, List<bool> statuses, List<DateTime> dates)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        ViewTasks(tasks, statuses, dates);
        Console.Write("Введите номер задачи для отметки как выполненной: ");
        
        if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count)
        {
            int index = taskNumber - 1;
            statuses[index] = true;
            dates[index] = DateTime.Now; // обновляем дату при изменении статуса
            Console.WriteLine($"Задача '{tasks[index]}' отмечена как выполненная!");
        }
        else
        {
            Console.WriteLine("Ошибка: неверный номер задачи");
        }
    }
    
    static void RemoveTask(List<string> tasks, List<bool> statuses, List<DateTime> dates)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        ViewTasks(tasks, statuses, dates);
        Console.Write("Введите номер задачи для удаления: ");
        
        if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count)
        {
            int index = taskNumber - 1;
            string removedTask = tasks[index];
            
            // Одновременное удаление из всех трех массивов
            tasks.RemoveAt(index);
            statuses.RemoveAt(index);
            dates.RemoveAt(index);
            
            Console.WriteLine($"Задача '{removedTask}' успешно удалена!");
        }
        else
        {
            Console.WriteLine("Ошибка: неверный номер задачи");
        }
    }
    
    static void EditTask(List<string> tasks, List<bool> statuses, List<DateTime> dates)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        ViewTasks(tasks, statuses, dates);
        Console.Write("Введите номер задачи для редактирования: ");
        
        if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count)
        {
            int index = taskNumber - 1;
            Console.Write("Введите новый текст задачи (в кавычках): ");
            string input = Console.ReadLine()?.Trim() ?? "";
            
            if (input.StartsWith("\"") && input.EndsWith("\""))
            {
                string newTask = input.Substring(1, input.Length - 2);
                if (!string.IsNullOrWhiteSpace(newTask))
                {
                    // Одновременное обновление всех трех массивов
                    tasks[index] = newTask;
                    dates[index] = DateTime.Now; // обновляем дату изменения
                    Console.WriteLine("Задача успешно отредактирована!");
                }
                else
                {
                    Console.WriteLine("Ошибка: текст задачи не может быть пустым");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: неправильный формат. Используйте: \"новый текст задачи\"");
            }
        }
        else
        {
            Console.WriteLine("Ошибка: неверный номер задачи");
        }
    }
}
