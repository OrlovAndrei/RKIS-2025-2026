﻿using System;

namespace TodoList
{
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

            string firstName = "";
            string lastName = "";
            int birthYear = 0;

            if (!skipProfile)
            {
                Console.Write("Введите имя: ");
                firstName = Console.ReadLine();

                Console.Write("Введите фамилию: ");
                lastName = Console.ReadLine();

                Console.Write("Введите возраст: ");
                string yearInput = Console.ReadLine();

                while (!int.TryParse(yearInput, out birthYear))
                {
                    Console.Write("Ошибка! Введите корректный возраст: ");
                    yearInput = Console.ReadLine();
                }
                // Результаты 
                Console.WriteLine($"Добро пожаловать пользователь {firstName} {lastName}, возраст - {birthYear}");
            }
            // Это массивы для задач
            string[] taskDescriptions = new string[intialArraySize];
            bool[] taskStatuses = new bool[intialArraySize];
            DateTime[] taskDates = new DateTime[intialArraySize];
            int taskCount = 0;

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
                    ExecuteProfile(firstName, lastName, birthYear, skipProfile);
                else if (command == "view")
                    ExecuteView(taskDescriptions, taskStatuses, taskDates, taskCount, input);
                //Ссылочка :3
                else if (command == "link")
                    ExecuteLink();
                else if (command == "exit")
                {
                    Console.WriteLine("Выход из программы...");
                    return;
                }
                else if (command.StartsWith("add"))
                    taskCount = AddTask(input, taskDescriptions, taskStatuses, taskDates, taskCount, multiline);
                else if (command.StartsWith("done"))
                    taskCount = MarkTaskAsDone(input, taskDescriptions, taskStatuses, taskDates, taskCount);
                else if (command.StartsWith("delete"))
                    taskCount = DeleteTask(input, taskDescriptions, taskStatuses, taskDates, taskCount, force);
                else if (command.StartsWith("update"))
                    taskCount = UpdateTask(input, taskDescriptions, taskStatuses, taskDates, taskCount, force);
                else if (command.StartsWith("read"))
                    ReadTask(input, taskDescriptions, taskStatuses, taskDates, taskCount);
                else
                    ExecuteUnknown();

            }
        }

        static void ExecuteProfile(string firstName, string lastName, int birthYear, bool skipProfile)
        {
            if (skipProfile)
            {
                Console.WriteLine("Профиль отключен флагом --skip-profile");
            }
            else
            {
                ShowProfile(firstName, lastName, birthYear);
            }
        }

        static void ExecuteView(string[] descriptions, bool[] statuses, DateTime[] dates, int taskCount, string input)
        {
            ViewTasks(descriptions, statuses, dates, taskCount, input);
        }

        static void ExecuteLink()
        {
            Console.WriteLine("https://youtu.be/dQw4w9WgXcQ?si=RqvXF3hYQQogSgMs");
        }
        static void ExecuteUnknown()
        {
            Console.WriteLine("Неизвестная команда. Введите 'help' для просмотра доступных команд.");
        }

        // Единственное, что может помочь (нет)
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
        // Выводит данные пользователя
        static void ShowProfile(string firstName, string lastName, int birthYear)
        {
            if (firstName == null) firstName = "";
            if (lastName == null) lastName = "";
            Console.WriteLine($"\n=== Профиль пользователя ===");
            Console.WriteLine($"{firstName} {lastName}, {birthYear}\n");
        }
        // Эт чтобы добавить задачу с мультиками
        static int AddTask(string input, string[] descriptions, bool[] statuses, DateTime[] dates, int taskCount, bool multiline = false)
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
                return taskCount;
            }
            // Проверку добавил

            if (taskCount >= descriptions.Length)
            {
                ExpandArrays(descriptions, statuses, dates);
                Console.WriteLine($"Массивы расширены до {descriptions.Length} элементов");
            }

            descriptions[taskCount] = task;
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
            taskCount++;

            Console.WriteLine($"Задача добавлена: \"{task}\"");
            Console.WriteLine($"Всего задач: {taskCount}/{descriptions.Length}");

            return taskCount;
        }

        static void ViewTasks(string[] descriptions, bool[] statuses, DateTime[] dates, int taskCount, string input)
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            if (descriptions == null || statuses == null || dates == null)
            {
                Console.WriteLine("Ошибка: Массивы задач не инициализированы.");
                return;
            }

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

            if (!showIndex && !showStatus && !showDate)
            {
                Console.WriteLine("\n=== Список задач ===");
                for (int i = 0; i < taskCount; i++)
                {
                    string shortDescription = descriptions[i];
                    if (shortDescription == null) shortDescription = "";

                    // ЗАМЕНА ПЕРЕНОСОВ СТРОК НА ПРОБЕЛЫ И ОБРЕЗКА ДО 30 СИМВОЛОВ
                    shortDescription = shortDescription.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                    if (shortDescription.Length > 30)
                    {
                        shortDescription = shortDescription.Substring(0, 27) + "...";
                    }
                    Console.WriteLine($"{shortDescription}");
                }
                Console.WriteLine($"Всего задач: {taskCount}\n");
                return;
            }

            int indexWidth = 6;
            int descriptionWidth = 33;
            int statusWidth = 10;
            int dateWidth = 16;

            Console.WriteLine("\n=== Список задач ===");

            string header = "";
            if (showIndex) header += "Индекс".PadRight(indexWidth);
            header += "Описание".PadRight(descriptionWidth);
            if (showStatus) header += "Статус".PadRight(statusWidth);
            if (showDate) header += "Дата изменения".PadRight(dateWidth);
            Console.WriteLine(header);

            string separator = "";
            if (showIndex) separator += new string('-', indexWidth);
            separator += new string('-', descriptionWidth);
            if (showStatus) separator += new string('-', statusWidth);
            if (showDate) separator += new string('-', dateWidth);
            Console.WriteLine(separator);

            for (int i = 0; i < taskCount; i++)
            {
                string line = "";

                if (showIndex)
                {
                    line += i.ToString().PadRight(indexWidth);
                }

                string shortDescription = descriptions[i];
                if (shortDescription == null) shortDescription = "";

                // ТОЖЕ ЗАМЕНА ПЕРЕНОСОВ СТРОК НА ПРОБЕЛЫ И ОБРЕЗКА ДО 30 СИМВОЛОВ
                shortDescription = shortDescription.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                if (shortDescription.Length > 30)
                {
                    shortDescription = shortDescription.Substring(0, 27) + "...";
                }
                line += shortDescription.PadRight(descriptionWidth);

                if (showStatus)
                {
                    string statusText = statuses[i] ? "Сделано" : "Не сделано";
                    line += statusText.PadRight(statusWidth);
                }

                if (showDate)
                {
                    string formattedDate = dates[i].ToString("dd.MM.yyyy HH:mm");
                    line += formattedDate.PadRight(dateWidth);
                }

                Console.WriteLine(line);
            }
            Console.WriteLine($"Всего задач: {taskCount}\n");
        }
        // Покажет задачи

        static int MarkTaskAsDone(string input, string[] descriptions, bool[] statuses, DateTime[] dates, int taskCount)
        {
            if (descriptions == null || statuses == null || dates == null)
                return taskCount;

            int taskIndex = ExtractTaskIndex(input, "done", taskCount);
            if (taskIndex == -1) return taskCount;

            statuses[taskIndex] = true;
            dates[taskIndex] = DateTime.Now;

            Console.WriteLine($"Задача \"{descriptions[taskIndex]}\" отмечена как выполненная");
            return taskCount;
        }
        // Это удалит задачу

        static int DeleteTask(string input, string[] descriptions, bool[] statuses, DateTime[] dates, int taskCount, bool force = false)
        {
            if (descriptions == null || statuses == null || dates == null)
                return taskCount;

            int taskIndex = ExtractTaskIndex(input, "delete", taskCount);
            if (taskIndex == -1) return taskCount;

            string deletedTask = descriptions[taskIndex];
            if (deletedTask == null) deletedTask = "";

            if (!force && taskIndex < taskCount - 1)
            {
                Console.WriteLine($"Используйте --force для удаления задачи не с конца списка");
                return taskCount;
            }

            for (int i = taskIndex; i < taskCount - 1; i++)
            {
                descriptions[i] = descriptions[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }

            descriptions[taskCount - 1] = null;
            statuses[taskCount - 1] = false;
            dates[taskCount - 1] = DateTime.MinValue;

            taskCount--;
            Console.WriteLine($"Задача \"{deletedTask}\" удалена");
            Console.WriteLine($"Осталось задач: {taskCount}");

            return taskCount;
        }

        static int UpdateTask(string input, string[] descriptions, bool[] statuses, DateTime[] dates, int taskCount, bool force = false)
        {
            if (descriptions == null || statuses == null || dates == null)
                return taskCount;

            string[] parts = input.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (parts == null || parts.Length < 3)
            {
                Console.WriteLine("Ошибка: Неверный формат команды. Используйте: update <индекс> \"новый текст\"");
                return taskCount;
            }

            if (parts.Length < 3 || !parts[2].StartsWith("\"") || !parts[2].EndsWith("\""))
            {
                Console.WriteLine("Ошибка: Неверный формат команды. Используйте: update <индекс> \"новый текст\"");
                return taskCount;
            }

            if (!int.TryParse(parts[1], out int taskIndex) || taskIndex < 0 || taskIndex >= taskCount)
            {
                Console.WriteLine($"Ошибка: Неверный индекс задачи. Допустимые значения: 0-{taskCount - 1}");
                return taskCount;
            }

            string newText = parts[2].Substring(1, parts[2].Length - 2).Trim();

            if (string.IsNullOrWhiteSpace(newText))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return taskCount;
            }

            string oldText = descriptions[taskIndex];
            if (oldText == null) oldText = "";
            descriptions[taskIndex] = newText;
            dates[taskIndex] = DateTime.Now;

            Console.WriteLine($"Задача обновлена: \"{oldText}\" -> \"{newText}\"");
            return taskCount;
        }

        static void ReadTask(string input, string[] descriptions, bool[] statuses, DateTime[] dates, int taskCount)
        {
            if (descriptions == null || statuses == null || dates == null)
                return;

            int taskIndex = ExtractTaskIndex(input, "read", taskCount);
            if (taskIndex == -1) return;

            string taskText = descriptions[taskIndex];
            if (taskText == null) taskText = "";
            Console.WriteLine($"\n=== Задача {taskIndex} ===");
            Console.WriteLine($"Текст: {taskText}");
            string statusText = statuses[taskIndex] ? "выполнена" : "не выполнена";
            Console.WriteLine($"Статус: {statusText}");
            string formattedDate = dates[taskIndex].ToString("dd.MM.yyyy HH:mm");
            Console.WriteLine($"Дата изменения: {formattedDate}\n");
        }

        static void ExpandArrays(string[] descriptions, bool[] statuses, DateTime[] dates)
        {
            if (descriptions == null || statuses == null || dates == null)
                return;

            int newSize = descriptions.Length * arrayExpansionMultiplier;

            string[] newDescriptions = new string[newSize];
            bool[] newStatuses = new bool[newSize];
            DateTime[] newDates = new DateTime[newSize];

            for (int i = 0; i < descriptions.Length; i++)
            {
                newDescriptions[i] = descriptions[i];
                newStatuses[i] = statuses[i];
                newDates[i] = dates[i];
            }

            descriptions = newDescriptions;
            statuses = newStatuses;
            dates = newDates;
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

        static int ExtractTaskIndex(string input, string commandName, int taskCount)
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

            if (taskIndex < 0 || taskIndex >= taskCount)
            {
                Console.WriteLine($"Ошибка: Неверный индекс задачи. Допустимые значения: 0-{taskCount - 1}");
                return -1;
            }

            return taskIndex;
        }
    }
}