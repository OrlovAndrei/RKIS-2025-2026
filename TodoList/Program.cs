using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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

        static string userName = "";
        static string userLastName = "";
        static int userBirthYear = 0;
        static int userAge = 0;

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
            userName = Console.ReadLine() ?? "Неизвестно";

            Console.Write("Введите вашу фамилию: ");
            userLastName = Console.ReadLine() ?? "Неизвестно";

            Console.Write("Введите ваш год рождения: ");
            string birthYearInput = Console.ReadLine();
            
            if (!int.TryParse(birthYearInput, out userBirthYear))
            {
                userBirthYear = DateTime.Now.Year - 25;
            }
            
            userAge = DateTime.Now.Year - userBirthYear;
            Console.WriteLine($"Добавлен пользователь {userName} {userLastName}, возраст – {userAge}");
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
            else if (command.StartsWith("profile"))
                ExecuteProfile();
            else if (command == "exit")
                ExecuteExit();
            else
                Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд");
        }

        static void ExecuteHelp()
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ ЗАДАЧАМИ");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("help                     - справка");
            Console.WriteLine("profile                  - профиль пользователя");
            Console.WriteLine("add <задача>             - добавить задачу");
            Console.WriteLine("add -d <задача>          - добавить выполненную задачу");
            Console.WriteLine("add --multiline          - многострочный ввод задачи");
            Console.WriteLine("add -m                   - многострочный ввод задачи");
            Console.WriteLine("view                     - показать задачи (только текст)");
            Console.WriteLine("view --index             - показать с индексами");
            Console.WriteLine("view --status            - показать со статусами");
            Console.WriteLine("view --update-date       - показать с датами");
            Console.WriteLine("view -a                  - показать все данные");
            Console.WriteLine("read <idx>               - информация о задаче");
            Console.WriteLine("update <idx> <текст>     - изменить задачу");
            Console.WriteLine("done <idx>               - отметить выполненной");
            Console.WriteLine("delete <idx>             - удалить задачу");
            Console.WriteLine("exit                     - выход");
            Console.WriteLine("=".PadRight(60, '='));
        }

        static void ExecuteProfile()
        {
            Console.WriteLine("ПРОФИЛЬ ПОЛЬЗОВАТЕЛЯ");
            Console.WriteLine(new string('-', 30));
            Console.WriteLine($"Имя: {userName}");
            Console.WriteLine($"Фамилия: {userLastName}");
            Console.WriteLine($"Год рождения: {userBirthYear}");
            Console.WriteLine($"Возраст: {userAge} лет");
            Console.WriteLine($"Всего задач: {todoCount}");
            Console.WriteLine(new string('-', 30));
        }

        static void ExecuteAdd(string input)
        {
            string taskPart = input.Substring(3).Trim();
            
            // Проверяем многострочный режим
            if (taskPart.StartsWith("--multiline") || taskPart.StartsWith("-m"))
            {
                AddTodoMultiline();
                return;
            }

            // Стандартный однострочный режим
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

            AddTodoToArray(task, isCompleted);
            Console.WriteLine(isCompleted ? "Выполненная задача добавлена!" : "Задача добавлена!");
        }

        static void AddTodoMultiline()
        {
            Console.WriteLine("Многострочный ввод задачи. Вводите строки, для завершения введите !end");
            Console.WriteLine("Начало ввода:");
            
            List<string> lines = new List<string>();
            while (true)
            {
                string line = Console.ReadLine() ?? "";
                if (line.Trim() == "!end")
                    break;
                lines.Add(line);
            }

            if (lines.Count == 0)
            {
                Console.WriteLine("Ошибка: задача не может быть пустой");
                return;
            }

            string multilineTask = string.Join("\n", lines);
            AddTodoToArray(multilineTask, false);
            Console.WriteLine("Многострочная задача добавлена!");
        }

        static void AddTodoToArray(string task, bool isCompleted)
        {
            if (todoCount >= todos.Length)
            {
                ResizeTodoArrays();
            }

            todos[todoCount] = task;
            statuses[todoCount] = isCompleted;
            dates[todoCount] = DateTime.Now;
            todoCount++;
        }

        static void ExecuteView(string input)
        {
            ViewConfig config = GetViewConfig(input);
            DisplayTodosTable(config);
        }

        static ViewConfig GetViewConfig(string input)
        {
            ViewConfig config = new ViewConfig();
            string args = input.Substring(4).Trim().ToLower();
            
            if (args.Contains("-a") || args.Contains("--all"))
            {
                config.ShowIndex = true;
                config.ShowStatus = true;
                config.ShowDate = true;
                return config;
            }

            config.ShowIndex = args.Contains("-i") || args.Contains("--index");
            config.ShowStatus = args.Contains("-s") || args.Contains("--status");
            config.ShowDate = args.Contains("-d") || args.Contains("--update-date");

            // Если нет флагов, показываем только текст
            if (!config.ShowIndex && !config.ShowStatus && !config.ShowDate)
            {
                config.ShowIndex = false;
                config.ShowStatus = false;
                config.ShowDate = false;
            }

            return config;
        }

        static void DisplayTodosTable(ViewConfig config)
        {
            if (todoCount == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            // Определяем ширину колонок
            int indexWidth = config.ShowIndex ? 8 : 0;
            int statusWidth = config.ShowStatus ? 12 : 0;
            int dateWidth = config.ShowDate ? 20 : 0;
            int textWidth = 30;

            // Вычисляем общую ширину таблицы
            int tableWidth = textWidth + indexWidth + statusWidth + dateWidth + 6; // +6 для разделителей

            // Заголовок таблицы
            Console.WriteLine(new string('-', tableWidth));
            Console.Write("|");
            
            if (config.ShowIndex)
                Console.Write(" Индекс  |");
            
            Console.Write(" Текст задачи".PadRight(textWidth + 1) + "|");
            
            if (config.ShowStatus)
                Console.Write(" Статус".PadRight(statusWidth) + "|");
            
            if (config.ShowDate)
                Console.Write(" Дата изменения".PadRight(dateWidth) + "|");
            
            Console.WriteLine();
            Console.WriteLine(new string('-', tableWidth));

            // Данные задач
            for (int i = 0; i < todoCount; i++)
            {
                Console.Write("|");
                
                // Колонка индекса
                if (config.ShowIndex)
                    Console.Write($" {i + 1,-6} |");
                
                // Колонка текста (обрезаем до 30 символов)
                string taskText = todos[i] ?? "[Пустая задача]";
                if (taskText.Length > 30)
                    taskText = taskText.Substring(0, 27) + "...";
                else if (taskText.Contains("\n"))
                    taskText = taskText.Split('\n')[0] + "...";
                
                Console.Write($" {taskText.PadRight(29)} |");
                
                // Колонка статуса
                if (config.ShowStatus)
                {
                    string status = statuses[i] ? "Сделано" : "Не сделано";
                    Console.Write($" {status.PadRight(10)} |");
                }
                
                // Колонка даты
                if (config.ShowDate)
                {
                    string date = dates[i].ToString("dd.MM.yyyy HH:mm");
                    Console.Write($" {date.PadRight(18)} |");
                }
                
                Console.WriteLine();
            }

            Console.WriteLine(new string('-', tableWidth));
            Console.WriteLine($"Всего задач: {todoCount}");
        }

        // Остальные методы остаются без изменений
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
            {
                Console.WriteLine($"Текст:");
                Console.WriteLine(taskText);
            }
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

    // Класс для конфигурации отображения
    class ViewConfig
    {
        public bool ShowIndex { get; set; }
        public bool ShowStatus { get; set; }
        public bool ShowDate { get; set; }
    }
}