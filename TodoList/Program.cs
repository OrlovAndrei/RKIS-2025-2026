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
                ExecuteHelp();
            else if (command.StartsWith("add"))
                ExecuteAdd(input);
            else if (command.StartsWith("view"))
                ExecuteView(input);
            else if (command.StartsWith("read"))
                ExecuteRead(input);
            else if (command.StartsWith("done") || command.StartsWith("complete"))
                ExecuteComplete(input);
            else if (command.StartsWith("delete"))
                ExecuteDelete(input);
            else if (command.StartsWith("update"))
                ExecuteUpdate(input);
            else if (command == "exit")
                ExecuteExit();
            else
                Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд");
        }

        static void ExecuteHelp()
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

        static void ExecuteAdd(string input)
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
                ResizeTodoArrays();
            }

            AddNewTodo(task, isCompleted);
            Console.WriteLine(isCompleted ? "Выполненная задача добавлена!" : "Задача добавлена!");
        }

        static void ExecuteView(string input)
        {
            string filter = GetViewFilter(input);
            DisplayTodos(filter);
        }

        static void ExecuteRead(string input)
        {
            if (!TryParseTaskNumber(input, @"read\s+(\d+)", out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            int index = taskNumber - 1;
            if (!IsValidTaskIndex(index))
            {
                Console.WriteLine($"Задача {taskNumber} не найдена");
                return;
            }

            string mode = GetReadMode(input);
            DisplayTaskDetails(taskNumber, index, mode);
        }

        static void ExecuteComplete(string input)
        {
            if (!TryParseTaskNumber(input, @"(done|complete)\s+(\d+)", out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            int index = taskNumber - 1;
            if (!IsValidTaskIndex(index))
            {
                Console.WriteLine($"Задача {taskNumber} не найдена");
                return;
            }

            MarkTaskAsCompleted(index);
            Console.WriteLine($"Задача {taskNumber} отмечена как выполненная!");
        }

        static void ExecuteDelete(string input)
        {
            if (!TryParseTaskNumber(input, @"delete\s+(\d+)", out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            int index = taskNumber - 1;
            if (!IsValidTaskIndex(index))
            {
                Console.WriteLine($"Задача {taskNumber} не найдена");
                return;
            }

            DeleteTask(index);
            Console.WriteLine($"Задача {taskNumber} удалена!");
        }

        static void ExecuteUpdate(string input)
        {
            var match = Regex.Match(input, @"update\s+(\d+)\s+(.+)");
            if (!match.Success || !int.TryParse(match.Groups[1].Value, out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи и текст");
                return;
            }

            int index = taskNumber - 1;
            if (!IsValidTaskIndex(index))
            {
                Console.WriteLine($"Задача {taskNumber} не найдена");
                return;
            }

            string newTask = match.Groups[2].Value.Trim().Trim('"', '\'');
            UpdateTaskText(index, newTask);
            Console.WriteLine($"Задача {taskNumber} обновлена!");
        }

        static void ExecuteExit()
        {
            Console.WriteLine("Выход из программы...");
            Environment.Exit(0);
        }

        static void ResizeTodoArrays()
        {
            int newSize = todos.Length * GrowthFactor;
            Array.Resize(ref todos, newSize);
            Array.Resize(ref statuses, newSize);
            Array.Resize(ref dates, newSize);
        }

        static void AddNewTodo(string task, bool isCompleted)
        {
            todos[todoCount] = task;
            statuses[todoCount] = isCompleted;
            dates[todoCount] = DateTime.Now;
            todoCount++;
        }

        static string GetViewFilter(string input)
        {
            string args = input.Substring(4).Trim();
            if (args.Contains("-d")) return "done";
            if (args.Contains("-u")) return "undone";
            return "all";
        }

        static void DisplayTodos(string filter)
        {
            int displayedCount = 0;
            Console.WriteLine("Список задач:");
            Console.WriteLine(new string('-', 50));
            
            for (int i = 0; i < todoCount; i++)
            {
                if (filter == "done" && !statuses[i]) continue;
                if (filter == "undone" && statuses[i]) continue;
                
                DisplaySingleTodo(i);
                displayedCount++;
            }
            
            DisplayTodoSummary(displayedCount, filter);
            Console.WriteLine(new string('-', 50));
        }

        static void DisplaySingleTodo(int index)
        {
            string taskText = todos[index] ?? "[Пустая задача]";
            if (taskText.Length > 40)
                taskText = taskText.Substring(0, 37) + "...";
            
            string status = statuses[index] ? "Сделано" : "Не сделано";
            string date = dates[index].ToString("dd.MM.yyyy HH:mm");
            
            Console.WriteLine($"{index + 1}. {taskText}");
            Console.WriteLine($"   {status} | {date}");
        }

        static void DisplayTodoSummary(int displayedCount, string filter)
        {
            if (displayedCount == 0)
            {
                string message = filter == "done" ? "Нет выполненных задач" : 
                               filter == "undone" ? "Нет невыполненных задач" : "Список задач пуст";
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine($"Показано {displayedCount} задач");
            }
        }

        static bool TryParseTaskNumber(string input, string pattern, out int taskNumber)
        {
            taskNumber = 0;
            var match = Regex.Match(input, pattern);
            return match.Success && int.TryParse(match.Groups[match.Groups.Count - 1].Value, out taskNumber);
        }

        static bool IsValidTaskIndex(int index)
        {
            return index >= 0 && index < todoCount;
        }

        static string GetReadMode(string input)
        {
            if (input.Contains("-ft")) return "fulltext";
            if (input.Contains("-t")) return "text";
            if (input.Contains("-s")) return "status";
            if (input.Contains("-d")) return "date";
            return "full";
        }

        static void DisplayTaskDetails(int taskNumber, int index, string mode)
        {
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

        static void MarkTaskAsCompleted(int index)
        {
            statuses[index] = true;
            dates[index] = DateTime.Now;
        }

        static void DeleteTask(int index)
        {
            for (int i = index; i < todoCount - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }
            todoCount--;
        }

        static void UpdateTaskText(int index, string newTask)
        {
            todos[index] = newTask;
            dates[index] = DateTime.Now;
        }
    }
}