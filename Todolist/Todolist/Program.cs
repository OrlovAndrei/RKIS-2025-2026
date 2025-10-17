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
                    AddMultiTask(input);
                }
                else if (command.StartsWith("done"))
                {
                    MarkDoneTask(input);
                }
                else if (command.StartsWith("delete"))
                {
                    DeleteTask(input);
                }
                else if (command.StartsWith("update"))
                {
                    UpdateTask(input);
                }
                else if (command == "view")
                {
                    ViewTasks(input);
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
            Console.WriteLine("done [id] - отметить задачу как выполненную");
            Console.WriteLine("delete [id] - удалить задачу");
            Console.WriteLine("update [id] \"новый текст\" - обновить текст задачи");
            Console.WriteLine("view - показать все задачи");
            Console.WriteLine("exit - выйти из программы");
        }

        static void ShowProfile()
        {
            Console.WriteLine($"{firstName} {lastName}, {age}");
        }

        static void AddMultiTask(string input)
        {
            var flags = ParseFlags(input);
            bool isMultiTask = flags.Contains("-m") || flags.Contains("--multi");
            
            if (isMultiTask)
            {
                string taskText = "";
                Console.WriteLine("Многострочный режим, введите !end для отправки");

                while (true)
                {
                    string line = Console.ReadLine();
                    if (line == "!end") break;
                    taskText += line + "\n";
                }

                AddTask(taskText);
            }
            else
            {
                string taskText = ExtractTaskText(input);
                AddTask(taskText);
            }
        }
        static void AddTask(string taskText)
        {
            if (string.IsNullOrWhiteSpace(taskText))
            {
                Console.WriteLine("Ошибка: текст задачи не может быть пустым");
                Console.WriteLine("Формат: add \"текст задачи\"");
                return;
            }

            if (taskCount >= todos.Length)
            {
                ResizeArray();
            }

            todos[taskCount] = taskText;
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
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

        static void ResizeArray()
        {
            int newSize = todos.Length * 2;
            string[] newTodos = new string[newSize];
            bool[] newStatuses = new bool[newSize];
            DateTime[] newDates = new DateTime[newSize];

            for (int i = 0; i < todos.Length; i++)
            {
                newTodos[i] = todos[i];
                newStatuses[i] = statuses[i];
                newDates[i] = dates[i];
            }

            todos = newTodos;
            statuses = newStatuses;
            dates = newDates;
            
            Console.WriteLine($"Массив расширен до {newSize} элементов");
        }
        private static string[] ParseFlags(string command)
        {
            var parts = command.Split(' ');
            var flags = new List<string>();

            foreach (var part in parts)
            {
                if (part.StartsWith("--"))
                {
                    flags.Add(part);
                }
                else if (part.StartsWith("-"))
                {
                    for (int i = 1; i < part.Length; i++)
                        flags.Add("-" + part[i]);
                }
            }

            return flags.ToArray();
        }

        static void ViewTasks(string input)
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Задач нет");
                return;
            }

            int indexWidth = 6;
            int textWidth = 36;
            int statusWidth = 14;
            int updateDateWidth = 16;
      
            var flags = ParseFlags(input);

            bool showIndex = flags.Contains("-i") || flags.Contains("--index");
            bool showStatus = flags.Contains("-s") || flags.Contains("--status");
            bool showUpdateDate = flags.Contains("-d") || flags.Contains("--update-date");
            bool showAll = flags.Contains("-a") || flags.Contains("--all");

            List<string> headers = ["Текст задачи".PadRight(textWidth)];
            if (showIndex || showAll) headers.Add("Индекс".PadRight(indexWidth));
            if (showStatus || showAll) headers.Add("Статус".PadRight(statusWidth));
            if (showUpdateDate || showAll) headers.Add("Дата обновления".PadRight(updateDateWidth));

            Console.WriteLine("| " + string.Join(" | ", headers) + " |");
            Console.WriteLine("|-" + string.Join("-|-", headers.Select(it => new string('-',it.Length))) + "-|");

            for (int i = 0; i < taskCount; i++)
            {
                if (string.IsNullOrEmpty(todos[i])) continue;

                string text = todos[i].Replace("\n", " ");
                if (text.Length > 30) text = text.Substring(0, 30) + "...";

                string status = statuses[i] ? "выполнена" : "не выполнена";
                string date = dates[i].ToString("yyyy-MM-dd HH:mm");

                List<string> rows = [text.PadRight(textWidth)];
                if (showIndex || showAll) rows.Add(i.ToString().PadRight(indexWidth));
                if (showStatus || showAll) rows.Add(status.PadRight(statusWidth));
                if (showUpdateDate || showAll) rows.Add(date.PadRight(updateDateWidth));

                Console.WriteLine("| " + string.Join(" | ", rows) + " |");
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
        
        static void DeleteTask(string input)
        {
            var parts = input.Split(' ', 2);
            if (parts.Length < 2 || !int.TryParse(parts[1], out int index) || index < 1 || index > taskCount)
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи.");
                return;
            }
            Console.WriteLine($"Удалена задача: '{todos[index - 1]}'");
            
            string[] newTodos = new string[todos.Length];
            bool[] newStatuses = new bool[todos.Length];
            DateTime[] newDates = new DateTime[todos.Length];
            for (int i = 0; i < todos.Length; i++)
            {
                if (i == index - 1) continue;
                newTodos[i] = todos[i];
                newStatuses[i] = statuses[i];
                newDates[i] = dates[i];
            }

            todos = newTodos;
            statuses = newStatuses;
            dates = newDates;
        }
        
        static void UpdateTask(string input)
        {
            var parts = input.Split(' ', 3);
            if (parts.Length < 2 || !int.TryParse(parts[1], out int index) || index < 1 || index > taskCount)
            {
                Console.WriteLine("Ошибка: формат - update <номер> \"новый текст\"");
                return;
            }

            string newText = parts[2].Trim('"');
            todos[index - 1] = newText;
            dates[index - 1] = DateTime.Now;
            
            Console.WriteLine($"Задача №{index} обновлена: {newText}");
        }
    }
}
