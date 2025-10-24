using System;

namespace TodoList
{

    class TodoItem
    {
        public string Text { get; private set; }
        public bool IsDone { get; private set; }
        public DateTime LastUpdate { get; private set; }

        public TodoItem(string text)
        {
            Text = text;
            IsDone = false;
            LastUpdate = DateTime.Now;
        }

        public void MarkDone()
        {
            IsDone = true;
            LastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            Text = newText;
            LastUpdate = DateTime.Now;
        }

        public string GetShortInfo()
        {
            string shortText = Text;
            if (shortText == null) shortText = "";
            shortText = shortText.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
            if (shortText.Length > 30)
            {
                shortText = shortText.Substring(0, 27) + "...";
            }
            string status = IsDone ? "Сделано" : "Не сделано";
            string formattedDate = LastUpdate.ToString("dd.MM.yyyy HH:mm");
            return $"{shortText} | {status} | {formattedDate}";
        }

        public string GetFullInfo()
        {
            string status = IsDone ? "выполнена" : "не выполнена";
            string formattedDate = LastUpdate.ToString("dd.MM.yyyy HH:mm");
            return $"Текст: {Text}\nСтатус: {status}\nДата изменения: {formattedDate}";
        }
    }

    class Program
    {

        private const int intialArraySize = 2;
        private const int arrayExpansionMultiplier = 2;

        static void Main(string[] args)
        {
            Console.WriteLine("Powered by Yar and Angelina");

            bool showHelpAtStart = false;
            bool skipProfile = false;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--help" || args[i] == "-h")
                {
                    showHelpAtStart = true;
                }
                else if (args[i] == "--skip-profile" || args[i] == "-s")
                {
                    skipProfile = true;
                }
                else if (args[i] == "--version" || args[i] == "-v")
                {
                    Console.WriteLine("TodoList v1.0");
                    return;
                }
            }

            if (showHelpAtStart)
            {
                ShowHelp();
            }

            Profile profile = new Profile();

            if (!skipProfile)
            {
                Console.Write("Введите имя: ");
                profile.FirstName = Console.ReadLine();

                Console.Write("Введите фамилию: ");
                profile.LastName = Console.ReadLine();

                Console.Write("Введите возраст: ");
                string yearInput = Console.ReadLine();

                int birthYear;
                while (!int.TryParse(yearInput, out birthYear))
                {
                    Console.Write("Ошибка! Введите корректный возраст: ");
                    yearInput = Console.ReadLine();
                }
                profile.BirthYear = birthYear;
                Console.WriteLine($"Добро пожаловать пользователь {profile.GetInfo()}");
            }
            
            TodoList todoList = new TodoList();

            while (true)
            {
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                string[] inputParts = input.Split(' ');
                if (inputParts == null || inputParts.Length == 0)
                    continue;

                string command = inputParts[0].ToLower();
                bool verbose = false;
                bool force = false;
                bool multiline = false;

                for (int i = 1; i < inputParts.Length; i++)
                {
                    if (inputParts[i] == "--verbose" || inputParts[i] == "-V")
                    {
                        verbose = true;
                    }
                    else if (inputParts[i] == "--force" || inputParts[i] == "-f")
                    {
                        force = true;
                    }
                    else if (inputParts[i] == "--multiline" || inputParts[i] == "-m")
                    {
                        multiline = true;
                    }
                }

                if (command == "help")
                    ShowHelp();
                else if (command == "profile")
                    ExecuteProfile(profile, skipProfile);
                else if (command == "view")
                    ExecuteView(todoList, input);
                else if (command == "link")
                    ExecuteLink();
                else if (command == "exit")
                {
                    Console.WriteLine("Выход из программы...");
                    return;
                }
                else if (command.StartsWith("add"))
                    AddTask(input, todoList, multiline);
                else if (command.StartsWith("done"))
                    MarkTaskAsDone(input, todoList);
                else if (command.StartsWith("delete"))
                    DeleteTask(input, todoList, force);
                else if (command.StartsWith("update"))
                    UpdateTask(input, todoList, force);
                else if (command.StartsWith("read"))
                    ReadTask(input, todoList);
                else
                    ExecuteUnknown();

            }
        }

        static void ExecuteProfile(Profile profile, bool skipProfile)
        {
            if (skipProfile)
            {
                Console.WriteLine("Профиль отключен флагом --skip-profile");
            }
            else
            {
                Console.WriteLine(profile.GetInfo());
            }
        }

        static void ExecuteView(TodoList todoList, string input)
        {
            string[] inputParts = input.Split(' ');
            if (inputParts == null)
                return;

            bool showIndex = false;
            bool showStatus = false;
            bool showDate = false;
            bool showAll = false;

            for (int i = 1; i < inputParts.Length; i++)
            {
                if (inputParts[i] == "--index" || inputParts[i] == "-i" || inputParts[i] == "-is" || inputParts[i] == "-dis")
                {
                    showIndex = true;
                }
                else if (inputParts[i] == "--status" || inputParts[i] == "-s" || inputParts[i] == "-is" || inputParts[i] == "-ds" || inputParts[i] == "-dis")
                {
                    showStatus = true;
                }
                else if (inputParts[i] == "--update-date" || inputParts[i] == "-d" || inputParts[i] == "-ds" || inputParts[i] == "-dis")
                {
                    showDate = true;
                }
                else if (inputParts[i] == "--all" || inputParts[i] == "-a")
                {
                    showAll = true;
                }
            }

            if (showAll)
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }

            todoList.View(showIndex, showStatus, showDate);
        }

        static void ExecuteLink()
        {
            Console.WriteLine("https://youtu.be/dQw4w9WgXcQ?si=RqvXF3hYQQogSgMs");
        }
        static void ExecuteUnknown()
        {
            Console.WriteLine("Неизвестная команда. Введите 'help' для просмотра доступных команд.");
        }

        static void ShowHelp()
        {
            Console.WriteLine(@"
            === Доступные команды ===
            help - показать этот список команд
            profile - показать данные пользователя
            add ""текст задачи"" - добавить новую задачу
            view - показать все задачи
            read <индекс> - показать полный текст задачи
            done <индекс> - отметить задачу выполненной
            delete <индекс> - удалить задачу
            update <индекс> ""текст"" - обновить текст задачи
            exit - выйти из программы

            link - показывает ссылочку

            === Флаги ===
            --help, -h - показать справку
            --skip-profile, -s - пропустить создание профиля
            --version, -v - показать версию
            --verbose, -V - подробный вывод
            --force, -f - принудительное выполнение
            --multiline, -m - многострочный ввод для add
            --index, -i - показывать индекс задачи (view)
            --status, -s - показывать статус задачи (view)
            --update-date, -d - показывать дату изменения (view)
            --all, -a - показывать все данные (view)
            ");
        }

        static void AddTask(string input, TodoList todoList, bool multiline = false)
        {
            string task = "";

            if (multiline)
            {
                Console.WriteLine("Многострочный режим. Вводите строки задачи. Для завершения введите !end");
                string multilineTask = "";
                while (true)
                {
                    Console.Write("> ");
                    string line = Console.ReadLine();
                    if (line == null) continue;
                    if (line == "!end")
                    {
                        break;
                    }
                    if (!string.IsNullOrEmpty(multilineTask))
                    {
                        multilineTask += "\n";
                    }
                    multilineTask += line;
                }
                task = multilineTask.Trim();
            }
            else
            {
                task = ExtractTaskText(input);
            }

            if (string.IsNullOrWhiteSpace(task))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return;
            }

            TodoItem newItem = new TodoItem(task);
            todoList.Add(newItem);

            Console.WriteLine($"Задача добавлена: \"{task}\"");
        }

        static void MarkTaskAsDone(string input, TodoList todoList)
        {
            int taskIndex = ExtractTaskIndex(input, "done");
            if (taskIndex == -1) return;

            TodoItem item = todoList.GetItem(taskIndex);
            if (item == null)
            {
                Console.WriteLine($"Ошибка: Задача с индексом {taskIndex} не найдена.");
                return;
            }

            item.MarkDone();

            Console.WriteLine($"Задача \"{item.Text}\" отмечена как выполненная");
        }

        static void DeleteTask(string input, TodoList todoList, bool force = false)
        {
            int taskIndex = ExtractTaskIndex(input, "delete");
            if (taskIndex == -1) return;

            TodoItem item = todoList.GetItem(taskIndex);
            if (item == null)
            {
                Console.WriteLine($"Ошибка: Задача с индексом {taskIndex} не найдена.");
                return;
            }

            string deletedTask = item.Text;
            if (deletedTask == null) deletedTask = "";

            if (!force)
            {
                Console.WriteLine($"Используйте --force для удаления задачи не с конца списка");
                return;
            }

            todoList.Delete(taskIndex);
            Console.WriteLine($"Задача \"{deletedTask}\" удалена");
        }

        static void UpdateTask(string input, TodoList todoList, bool force = false)
        {
            string[] parts = input.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts == null || parts.Length < 3)
            {
                Console.WriteLine("Ошибка: Неверный формат команды. Используйте: update <индекс> \"новый текст\"");
                return;
            }

            if (parts.Length < 3 || !parts[2].StartsWith("\"") || !parts[2].EndsWith("\""))
            {
                Console.WriteLine("Ошибка: Неверный формат команды. Используйте: update <индекс> \"новый текст\"");
                return;
            }

            if (!int.TryParse(parts[1], out int taskIndex))
            {
                Console.WriteLine($"Ошибка: Неверный индекс задачи.");
                return;
            }

            TodoItem item = todoList.GetItem(taskIndex);
            if (item == null)
            {
                Console.WriteLine($"Ошибка: Задача с индексом {taskIndex} не найдена.");
                return;
            }

            string newText = parts[2].Substring(1, parts[2].Length - 2).Trim();

            if (string.IsNullOrWhiteSpace(newText))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return;
            }

            string oldText = item.Text;
            if (oldText == null) oldText = "";
            item.UpdateText(newText);

            Console.WriteLine($"Задача обновлена: \"{oldText}\" -> \"{newText}\"");
        }

        static void ReadTask(string input, TodoList todoList)
        {
            int taskIndex = ExtractTaskIndex(input, "read");
            if (taskIndex == -1) return;

            TodoItem item = todoList.GetItem(taskIndex);
            if (item == null)
            {
                Console.WriteLine($"Ошибка: Задача с индексом {taskIndex} не найдена.");
                return;
            }

            Console.WriteLine(item.GetFullInfo());
        }

        static string ExtractTaskText(string input)
        {
            if (input == null)
                return null;

            string[] parts = input.Split('"');
            if (parts == null || parts.Length < 2)
            {
                Console.WriteLine("Ошибка: Неверный формат команды. Используйте: add \"текст задачи\"");
                return null;
            }

            if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
            {
                Console.WriteLine("Ошибка: Неверный формат команды. Используйте: add \"текст задачи\"");
                return null;
            }

            return parts[1].Trim();
        }

        static int ExtractTaskIndex(string input, string commandName)
        {
            if (input == null)
                return -1;

            string[] parts = input.Split(' ');
            if (parts == null || parts.Length < 2)
            {
                Console.WriteLine($"Ошибка: Неверный формат команды. Используйте: {commandName} <индекс>");
                return -1;
            }

            if (parts.Length < 2 || !int.TryParse(parts[1], out int taskIndex))
            {
                Console.WriteLine($"Ошибка: Неверный формат команды. Используйте: {commandName} <индекс>");
                return -1;
            }

            if (taskIndex < 0)
            {
                Console.WriteLine($"Ошибка: Неверный индекс задачи.");
                return -1;
            }

            return taskIndex;
        }
    }
}