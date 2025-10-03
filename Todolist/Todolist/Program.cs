using System;

namespace ToddList
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Бурнашов и Хазиев");

            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            string yearInput = Console.ReadLine();

            int birthYear = int.Parse(yearInput);
            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;

            string[] todos = new string[2];
            int taskCount = 0;

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
                    ShowProfile(firstName, lastName, birthYear);
                }
                else if (command.StartsWith("add"))
                {
                    AddTask(input, ref todos, ref taskCount);
                }
                else if (command == "view")
                {
                    ViewTasks(todos, taskCount);
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                }
            }
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

        static void ShowProfile(string firstName, string lastName, int birthYear)
        {
            Console.WriteLine($"{firstName} {lastName}, {birthYear}");
        }

        static void AddTask(string input, ref string[] todos, ref int taskCount)
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
            taskCount++;
            Console.WriteLine($"Задача добавлена: '{taskText}'");
            Console.WriteLine($"Всего задач: {taskCount}, Размер массива: {todos.Length}");
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

        static void ViewTasks(string[] todos, int taskCount)
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Задач нет");
                return;
            }

            Console.WriteLine($"Список задач (всего: {taskCount}, размер массива: {todos.Length}):");
            for (int i = 0; i < taskCount; i++)
            {
                Console.WriteLine($"{i + 1}. {todos[i]}");
            }
        }
    }
}