using System;
using System.Text.RegularExpressions;

namespace Todolist
{
    class Program
    {
        private const int InitialCapacity = 2;
        private const int GrowthFactor = 2;
        
        static string[] todos = new string[InitialCapacity];
        static bool[] statuses = new bool[InitialCapacity];
        static DateTime[] dates = new DateTime[InitialCapacity];
        static int todoCount = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Тревога и Назаретьянц");
            CreateUserProfile();
            Console.WriteLine("Добро пожаловать в систему управления задачами!");
            Console.WriteLine("Введите 'help' для списка команд");
            RunCommandLoop();
        }

        static void CreateUserProfile()
        {
            Console.Write("Введите ваше имя: ");
            string firstName = Console.ReadLine() ?? "Неизвестно";

            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine() ?? "Неизвестно";

            Console.Write("Введите ваш год рождения: ");
            string birthYearInput = Console.ReadLine();
            
            if (!int.TryParse(birthYearInput, out int birthYear))
            {
                birthYear = DateTime.Now.Year - 25;
            }
            
            int age = DateTime.Now.Year - birthYear;
            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст – {age}");
        }

        static void RunCommandLoop()
        {
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine() ?? "";
                
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                ProcessCommand(input);
            }
        }

        static void ProcessCommand(string input)
        {
            string command = input.Trim().ToLower();
            
            if (command.StartsWith("help"))
                ShowHelp();
            else if (command.StartsWith("add"))
                AddTodo(input);
            else if (command.StartsWith("view"))
                ViewTodos(input);
            else if (command.StartsWith("read"))
                ReadTodo(input);
            else if (command.StartsWith("done") || command.StartsWith("complete"))
                CompleteTodo(input);
            else if (command.StartsWith("delete"))
                DeleteTodo(input);
            else if (command.StartsWith("update"))
                UpdateTodo(input);
            else if (command == "exit")
                Environment.Exit(0);
            else
                Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд");
        }

        static void ShowHelp()
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ ЗАДАЧАМИ");
            Console.WriteLine("=".PadRight(50, '='));
            Console.WriteLine("help               - справка");
            Console.WriteLine("add <задача>       - добавить задачу");
            Console.WriteLine("add -d <задача>    - добавить выполненную задачу");
            Console.WriteLine("view               - показать все задачи");
            Console.WriteLine("view -d            - выполненные задачи");
            Console.WriteLine("view -u            - невыполненные задачи");
            Console.WriteLine("read <idx>         - информация о задаче");
            Console.WriteLine("update <idx> <текст> - изменить задачу");
            Console.WriteLine("done <idx>         - отметить выполненной");
            Console.WriteLine("delete <idx>       - удалить задачу");
            Console.WriteLine("exit               - выход");
            Console.WriteLine("=".PadRight(50, '='));
        }

        static void AddTodo(string input)
        {
            string taskPart = input.Substring(3).Trim();
            
            if (string.IsNullOrWhiteSpace(taskPart))
            {
                Console.WriteLine("Ошибка: не указана задача");
                return;
            }

            bool isCompleted = false;
            string task = taskPart;

            if (taskPart.StartsWith("-d ") || taskPart.StartsWith("--done "))
            {
                isCompleted = true;
                task = taskPart.Substring(taskPart.IndexOf(' ') + 1).Trim();
            }

            task = task.Trim('"', '\'');

            if (todoCount >= todos.Length)
            {
                int newSize = todos.Length * GrowthFactor;
                Array.Resize(ref todos, newSize);
                Array.Resize(ref statuses, newSize);
                Array.Resize(ref dates, newSize);
            }

            todos[todoCount] = task;
            statuses[todoCount] = isCompleted;
            dates[todoCount] = DateTime.Now;
            todoCount++;

            Console.WriteLine(isCompleted ? "Выполненная задача добавлена!" : "Задача добавлена!");
        }

        static void ViewTodos(string input)
        {
            string filter = "all";
            string args = input.Substring(4).Trim();
            
            if (args.Contains("-d")) filter = "done";
            else if (args.Contains("-u")) filter = "undone";

            int displayedCount = 0;
            Console.WriteLine("Список задач:");
            Console.WriteLine(new string('-', 50));
            
            for (int i = 0; i < todoCount; i++)
            {
                if (filter == "done" && !statuses[i]) continue;
                if (filter == "undone" && statuses[i]) continue;
                
                string taskText = todos[i] ?? "[Пустая задача]";
                if (taskText.Length > 40)
                    taskText = taskText.Substring(0, 37) + "...";
                
                string status = statuses[i] ? "Сделано" : "Не сделано";
                string date = dates[i].ToString("dd.MM.yyyy HH:mm");
                
                Console.WriteLine($"{i + 1}. {taskText}");
                Console.WriteLine($"   {status} | {date}");
                displayedCount++;
            }
            
            if (displayedCount == 0)
                Console.WriteLine(filter == "done" ? "Нет выполненных задач" : 
                               filter == "undone" ? "Нет невыполненных задач" : "Список задач пуст");
            else
                Console.WriteLine($"Показано {displayedCount} задач");
            
            Console.WriteLine(new string('-', 50));
        }

        static void ReadTodo(string input)
        {
            var match = Regex.Match(input, @"read\s+(\d+)");
            if (!match.Success || !int.TryParse(match.Groups[1].Value, out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            int index = taskNumber - 1;
            if (index < 0 || index >= todoCount)
            {
                Console.WriteLine($"Задача {taskNumber} не найдена");
                return;
            }

            string mode = "full";
            if (input.Contains("-ft")) mode = "fulltext";
            else if (input.Contains("-t")) mode = "text";
            else if (input.Contains("-s")) mode = "status";
            else if (input.Contains("-d")) mode = "date";

            string taskText = todos[index] ?? "[Пустая задача]";
            string status = statuses[index] ? "Сделано" : "Не сделано";
            string date = dates[index].ToString("dd.MM.yyyy HH:mm");

            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"Задача #{taskNumber}");
            
            if (mode == "full" || mode == "fulltext")
                Console.WriteLine($"Текст: {taskText}");
            if (mode == "full" || mode == "status")
                Console.WriteLine($"Статус: {status}");
            if (mode == "full" || mode == "date")
                Console.WriteLine($"Дата: {date}");
                
            Console.WriteLine(new string('-', 40));
        }

        static void CompleteTodo(string input)
        {
            var match = Regex.Match(input, @"(done|complete)\s+(\d+)");
            if (!match.Success || !int.TryParse(match.Groups[2].Value, out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            int index = taskNumber - 1;
            if (index < 0 || index >= todoCount)
            {
                Console.WriteLine($"Задача {taskNumber} не найдена");
                return;
            }

            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача {taskNumber} отмечена как выполненная!");
        }

        static void DeleteTodo(string input)
        {
            var match = Regex.Match(input, @"delete\s+(\d+)");
            if (!match.Success || !int.TryParse(match.Groups[1].Value, out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            int index = taskNumber - 1;
            if (index < 0 || index >= todoCount)
            {
                Console.WriteLine($"Задача {taskNumber} не найдена");
                return;
            }

            for (int i = index; i < todoCount - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }

            todoCount--;
            Console.WriteLine($"Задача {taskNumber} удалена!");
        }

        static void UpdateTodo(string input)
        {
            var match = Regex.Match(input, @"update\s+(\d+)\s+(.+)");
            if (!match.Success || !int.TryParse(match.Groups[1].Value, out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи и текст");
                return;
            }

            int index = taskNumber - 1;
            if (index < 0 || index >= todoCount)
            {
                Console.WriteLine($"Задача {taskNumber} не найдена");
                return;
            }

            string newTask = match.Groups[2].Value.Trim().Trim('"', '\'');
            todos[index] = newTask;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача {taskNumber} обновлена!");
        }
    }
}