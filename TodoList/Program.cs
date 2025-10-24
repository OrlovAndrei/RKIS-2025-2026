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
            string firstName = Console.ReadLine();

            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            int birthYear = int.Parse(Console.ReadLine());
            int age = DateTime.Now.Year - birthYear;

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст – {age}");
        }

        static void RunCommandLoop()
        {
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                ProcessCommand(input);
            }
        }

        static void ProcessCommand(string input)
        {
            string command = input.Trim().ToLower();
            
            if (command.StartsWith("help"))
            {
                ShowHelp();
            }
            else if (command.StartsWith("add"))
            {
                AddTodo(input);
            }
            else if (command.StartsWith("view"))
            {
                ViewTodos(input);
            }
            else if (command.StartsWith("read"))
            {
                ReadTodo(input);
            }
            else if (command.StartsWith("done") || command.StartsWith("complete"))
            {
                CompleteTodo(input);
            }
            else if (command.StartsWith("delete"))
            {
                DeleteTodo(input);
            }
            else if (command.StartsWith("update"))
            {
                UpdateTodo(input);
            }
            else if (command.StartsWith("profile"))
            {
                ShowProfile();
            }
            else if (command == "exit")
            {
                Console.WriteLine("Выход из программы...");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine($"Неизвестная команда: {command.Split(' ')[0]}");
                Console.WriteLine("Введите 'help' для списка команд");
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ ЗАДАЧАМИ");
            Console.WriteLine("=".PadRight(60, '='));
            
            Console.WriteLine("\nОСНОВНЫЕ КОМАНДЫ:");
            Console.WriteLine("  help               - показать эту справку");
            Console.WriteLine("  exit               - выход из программы");
            
            Console.WriteLine("\nРАБОТА С ПОЛЬЗОВАТЕЛЕМ:");
            Console.WriteLine("  profile            - показать профиль пользователя");
            
            Console.WriteLine("\nРАБОТА С ЗАДАЧАМИ:");
            Console.WriteLine("  add <задача>       - добавить новую задачу");
            Console.WriteLine("  add -d <задача>    - добавить выполненную задачу");
            
            Console.WriteLine("\nПРОСМОТР ЗАДАЧ:");
            Console.WriteLine("  view               - показать все задачи (кратко)");
            Console.WriteLine("  view -a            - показать все задачи");
            Console.WriteLine("  view -d            - показать только выполненные задачи");
            Console.WriteLine("  view -u            - показать только невыполненные задачи");
            
            Console.WriteLine("\nДЕТАЛЬНЫЙ ПРОСМОТР:");
            Console.WriteLine("  read <idx>         - подробная информация о задаче");
            Console.WriteLine("  read <idx> -f      - полная информация (по умолчанию)");
            Console.WriteLine("  read <idx> -t      - показать только текст задачи");
            Console.WriteLine("  read <idx> -s      - показать только статус задачи");
            Console.WriteLine("  read <idx> -d      - показать только дату задачи");
            Console.WriteLine("  read <idx> -ft     - показать полный текст без обрезки");
            
            Console.WriteLine("\nРЕДАКТИРОВАНИЕ ЗАДАЧ:");
            Console.WriteLine("  update <idx> <текст> - изменить текст задачи");
            Console.WriteLine("  done <idx>         - отметить задачу как выполненную");
            Console.WriteLine("  complete <idx>     - отметить задачу как выполненную");
            Console.WriteLine("  delete <idx>       - удалить задачу");
            
            Console.WriteLine("\nПРИМЕРЫ ИСПОЛЬЗОВАНИЯ:");
            Console.WriteLine("  add Купить молоко");
            Console.WriteLine("  add -d Прочитать книгу");
            Console.WriteLine("  view -d");
            Console.WriteLine("  read 1 -ft");
            Console.WriteLine("  update 1 \"Купить молоко и хлеб\"");
            Console.WriteLine("  done 1");
            Console.WriteLine("  delete 2");
            
            Console.WriteLine("\nПОДСКАЗКИ:");
            Console.WriteLine("  • <idx> - номер задачи из списка");
            Console.WriteLine("  • Используйте кавычки для задач с пробелами");
            Console.WriteLine("  • Команды view и read поддерживают флаги фильтрации");
            
            Console.WriteLine("=".PadRight(60, '='));
        }

        static void ShowProfile()
        {
            Console.WriteLine("Профиль пользователя:");
            Console.WriteLine("Для просмотра данных перезапустите программу");
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
                task = taskPart.StartsWith("-d ") ? 
                    taskPart.Substring(3).Trim() : 
                    taskPart.Substring(7).Trim();
            }

            if (string.IsNullOrWhiteSpace(task))
            {
                Console.WriteLine("Ошибка: не указана задача после флага");
                return;
            }

            task = task.Trim('"', '\'');

            if (todoCount >= todos.Length)
            {
                int newSize = todos.Length * GrowthFactor;
                Array.Resize(ref todos, newSize);
                Array.Resize(ref statuses, newSize);
                Array.Resize(ref dates, newSize);
                Console.WriteLine($"Массивы расширены до {newSize} элементов");
            }

            todos[todoCount] = task;
            statuses[todoCount] = isCompleted;
            dates[todoCount] = DateTime.Now;
            todoCount++;

            string status = isCompleted ? "выполненная задача добавлена" : "задача добавлена";
            Console.WriteLine($"{status}!");
        }

        static void ViewTodos(string input)
        {
            string filter = "all";
            
            string args = input.Length > 4 ? input.Substring(4).Trim() : "";
            
            if (args.Contains("-d") || args.Contains("--done"))
            {
                filter = "done";
            }
            else if (args.Contains("-u") || args.Contains("--undone"))
            {
                filter = "undone";
            }
            else if (args.Contains("-a") || args.Contains("--all"))
            {
                filter = "all";
            }

            int displayedCount = 0;
            
            Console.WriteLine("Список задач:");
            Console.WriteLine(new string('-', 60));
            
            for (int i = 0; i < todoCount; i++)
            {
                if (filter == "done" && !statuses[i]) continue;
                if (filter == "undone" && statuses[i]) continue;
                
                string status = statuses[i] ? "Сделано" : "Не сделано";
                string date = dates[i].ToString("dd.MM.yyyy HH:mm");
                
                string taskText = todos[i];
                if (taskText.Length > 40)
                {
                    taskText = taskText.Substring(0, 37) + "...";
                }
                
                Console.WriteLine($"{i + 1}. {taskText}");
                Console.WriteLine($"   {status} | {date}");
                displayedCount++;
            }
            
            if (displayedCount == 0)
            {
                string message = filter switch
                {
                    "done" => "Нет выполненных задач",
                    "undone" => "Нет невыполненных задач",
                    _ => "Список задач пуст"
                };
                Console.WriteLine(message);
            }
            else
            {
                string filterText = filter switch
                {
                    "done" => "выполненных",
                    "undone" => "невыполненных",
                    _ => "всех"
                };
                Console.WriteLine($"Показано {displayedCount} {filterText} задач");
            }
            
            Console.WriteLine(new string('-', 60));
        }

        static void ReadTodo(string input)
        {
            var match = Regex.Match(input, @"read\s+(\d+)", RegexOptions.IgnoreCase);
            
            if (!match.Success)
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            int taskNumber = int.Parse(match.Groups[1].Value);
            int index = taskNumber - 1;
            
            if (index < 0 || index >= todoCount)
            {
                Console.WriteLine($"Ошибка: задача с номером {taskNumber} не найдена");
                return;
            }

            string mode = "full";
            
            string args = input.ToLower();
            if (args.Contains("-ft") || args.Contains("--fulltext"))
            {
                mode = "fulltext";
            }
            else if (args.Contains("-t") || args.Contains("--text"))
            {
                mode = "text";
            }
            else if (args.Contains("-s") || args.Contains("--status"))
            {
                mode = "status";
            }
            else if (args.Contains("-d") || args.Contains("--date"))
            {
                mode = "date";
            }
            else if (args.Contains("-f") || args.Contains("--full"))
            {
                mode = "full";
            }

            string taskText = todos[index];
            bool isCompleted = statuses[index];
            DateTime taskDate = dates[index];
            string status = isCompleted ? "Сделано" : "Не сделано";
            string date = taskDate.ToString("dd.MM.yyyy HH:mm");

            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"Задача #{taskNumber}");
            Console.WriteLine(new string('-', 50));
            
            switch (mode)
            {
                case "full":
                    Console.WriteLine($"Текст: {taskText}");
                    Console.WriteLine($"Статус: {status}");
                    Console.WriteLine($"Дата: {date}");
                    break;
                case "text":
                    Console.WriteLine($"Текст задачи:");
                    Console.WriteLine(taskText);
                    break;
                case "status":
                    Console.WriteLine($"Статус: {status}");
                    break;
                case "date":
                    Console.WriteLine($"Дата: {date}");
                    break;
                case "fulltext":
                    Console.WriteLine($"Полный текст задачи:");
                    Console.WriteLine(new string('-', 30));
                    Console.WriteLine(taskText);
                    Console.WriteLine(new string('-', 30));
                    break;
            }
            
            Console.WriteLine(new string('-', 50));
        }

        static void CompleteTodo(string input)
        {
            var match = Regex.Match(input, @"(done|complete)\s+(\d+)", RegexOptions.IgnoreCase);
            
            if (!match.Success)
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            int taskNumber = int.Parse(match.Groups[2].Value);
            int index = taskNumber - 1;
            
            if (index < 0 || index >= todoCount)
            {
                Console.WriteLine($"Ошибка: задача с номером {taskNumber} не найдена");
                return;
            }

            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача {taskNumber} отмечена как выполненная!");
        }

        static void DeleteTodo(string input)
        {
            var match = Regex.Match(input, @"delete\s+(\d+)", RegexOptions.IgnoreCase);
            
            if (!match.Success)
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            int taskNumber = int.Parse(match.Groups[1].Value);
            int index = taskNumber - 1;
            
            if (index < 0 || index >= todoCount)
            {
                Console.WriteLine($"Ошибка: задача с номером {taskNumber} не найдена");
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
            var match = Regex.Match(input, @"update\s+(\d+)\s+(.+)", RegexOptions.IgnoreCase);
            
            if (!match.Success)
            {
                Console.WriteLine("Ошибка: укажите номер задачи и новый текст");
                Console.WriteLine("Пример: update 1 \"Новый текст задачи\"");
                return;
            }

            int taskNumber = int.Parse(match.Groups[1].Value);
            string newTask = match.Groups[2].Value.Trim().Trim('"', '\'');

            int index = taskNumber - 1;
            if (index < 0 || index >= todoCount)
            {
                Console.WriteLine($"Ошибка: задача с номером {taskNumber} не найдена");
                return;
            }

            todos[index] = newTask;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача {taskNumber} обновлена!");
        }
    }
}