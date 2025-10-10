using System;

namespace TodoList
{
    class Program
    {
        static string firstName, lastName;
        static int age;
        
        static string[] todos = new string[2];
        static bool[] statuses = new bool[2];
        static DateTime[] dates = new DateTime[2];
        static int taskCount = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Бурнашов и Хазиев");
            CreateUser();
            
            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст – {age}");
            Console.WriteLine("Введите 'help' для списка команд");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (input == null || input.ToLower() == "exit")
                {
                    Console.WriteLine("Выход из программы...");
                    break;
                }

                string command = input.ToLower().Trim();

                if (command == "help")
                {
                    ShowHelp();
                }
                else if (command == "profile")
                {
                    ShowProfile();
                }
                else if (command.StartsWith("add"))
                {
                    AddTask(input);
                }
                else if (command.StartsWith("done"))
                {
                    MarkDoneTask(input);
                }
                else if (command == "view")
                {
                    ViewTasks();
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                }
            }
        }

        static void CreateUser()
        {
            Console.Write("Введите имя: ");
            firstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            lastName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            string yearInput = Console.ReadLine();

            int birthYear = int.Parse(yearInput);
            int currentYear = DateTime.Now.Year;
            age = currentYear - birthYear;
        }

        static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - вывести список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add \"текст задачи\" - добавить новую задачу");
            Console.WriteLine("view - показать все задачи");
            Console.WriteLine("exit - выйти из программы");
        }

        static void ShowProfile()
        {
            Console.WriteLine($"{firstName} {lastName}, {age}");
        }

        static void AddTask(string input)
        {
            string taskText = ExtractTaskText(input);
            
            if (string.IsNullOrWhiteSpace(taskText))
            {
                Console.WriteLine("Ошибка: текст задачи не может быть пустым");
                Console.WriteLine("Формат: add \"текст задачи\"");
                return;
            }

            if (taskCount >= todos.Length)
            {
                ResizeArray(ref todos);
            }

            todos[taskCount] = taskText;
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
            taskCount++;
            Console.WriteLine($"Задача добавлена: '{taskText}'");
            Console.WriteLine($"Всего задач: {taskCount},Размер массива: {todos.Length}");
        }

        static string ExtractTaskText(string input)
        {
            string[] parts = input.Split('"');
            
            if (parts.Length >= 2)
            {
                return parts[1];
            }
            else
            {
                return input.Substring(3).Trim();
            }
        }

        static void ResizeArray(ref string[] todos)
        {
            int newSize = todos.Length * 2;
            string[] newArray = new string[newSize];
            
            for (int i = 0; i < todos.Length; i++)
            {
                newArray[i] = todos[i];
            }
            
            todos = newArray;
            Console.WriteLine($"Массив расширен до {newSize} элементов");
        }

        static void ViewTasks()
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Задач нет");
                return;
            }

            Console.WriteLine($"Список задач (всего: {taskCount}, размер массива: {todos.Length}):");
            for (int i = 0; i < taskCount; i++)
            {
                string status = statuses[i] ? "выполнена" : "не выполнена";
                Console.WriteLine($"{i + 1}) {dates[i]} {todos[i]} - {status}");
            }
        }
        
        static void MarkDoneTask(string input)
        {
            var parts = input.Split(' ', 2);
            if (parts.Length < 2 || !int.TryParse(parts[1], out int i) || i < 1 || i > taskCount)
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи.");
                return;
            }

            statuses[i - 1] = true;
            Console.WriteLine($"Задача '{todos[i - 1]}' отмечена как выполненная.");
        }
    }
}