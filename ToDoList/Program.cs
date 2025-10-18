using System.Data;

namespace TodoList
{
    class Program
    {
        static string userName;
        static string userLastName;
        static int userAge;
        static string[] todos = new string[2];
        static int taskCount = 0;
        static bool[] statuses = new bool[2];
        static DateTime[] dates = new DateTime[2];

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Фучаджи и Клюев");

            CreateUser();

            Console.WriteLine("Введите help для вывода доступных команд");


            while (true)
            {

                Console.Write("Введите команду:");
                string CommandUser = Console.ReadLine();

                if (CommandUser == null || CommandUser.ToLower() == "exit") break;

                if (CommandUser == "help")
                {
                    CommandHelp();
                }
                else if (CommandUser == "profile")
                {
                    CommandProfile();
                }
                else if (CommandUser == "add")
                {
                    CommandAdd(CommandUser);
                }
                else if (CommandUser.StartsWith("done "))
                {
                    CommandDone(CommandUser);
                }
                else if (CommandUser.StartsWith("delete "))
                {
                    CommandDelete(CommandUser);
                }
                else if (CommandUser.StartsWith("update "))
                {
                    CommandUpdate(CommandUser);
                }
                else if (CommandUser.StartsWith("view"))
                {
                    CommandView();
                }
            }
        }

        static void CreateUser()
        {
            Console.Write("Введите ваше имя: ");
            userName = Console.ReadLine();
            Console.Write("Введите вашу Фамилию: ");
            userLastName = Console.ReadLine();

            Console.Write("Введите вашу дату рождения: ");
            string BirthYearInput = Console.ReadLine();
            int BirthYear = int.Parse(BirthYearInput);
            if (BirthYear <= 0)
            {
                Console.WriteLine("Введите реальный возраст");
                return;
            }
            int CurrentYear = DateTime.Now.Year;
            userAge = CurrentYear - BirthYear;

            Console.WriteLine($"Добавлен пользователь {userName} {userLastName}, Возвраст - {userAge}");
        }

        static void CommandHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help — выводит список всех доступных команд с кратким описанием.");
            Console.WriteLine("profile - выводит данные пользователя в формате: <Имя> <Фамилия>, <Год рождения>.");
            Console.WriteLine("add — добавляет новую задачу. Формат ввода: add *Задача*");
            Console.WriteLine("view — выводит все задачи из массива (только непустые элементы).");
            Console.WriteLine("done — отмечает задачу выполненной.");
            Console.WriteLine("delete — удаляет задачу по индексу");
            Console.WriteLine("update — обновляет текст задачи");
            Console.WriteLine("exit — завершает цикл и останавливает выполнение программы.");
        }
        static void CommandProfile()
        {
            Console.WriteLine($" Пользователь:{userName} {userLastName}, Год рождения: {userAge}");
        }
        static void CommandAdd(string CommandUser)
        {
            bool isMultiline = CommandUser.Contains("--multiline") || CommandUser.Contains("-m");
            bool isUrgent = CommandUser.Contains("--urgent") || CommandUser.Contains("-u");

            if (CommandUser.Contains("-") && CommandUser.Length >= 3)
            {
                int flagIndex = CommandUser.IndexOf('-');
                if (flagIndex + 2 < CommandUser.Length)
                {
                    string flags = CommandUser.Substring(flagIndex + 1, 2);
                    if (flags.Contains("m") && flags.Contains("u"))
                    {
                        isMultiline = true;
                        isUrgent = true;
                    }
                }
            }

            string task = "";
            if (isMultiline)
            {
                Console.WriteLine("Введите задачу (для завершения введите пустую строку):");
                string line;
                while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
                {
                    Console.Write("> ");
                    task += line + Environment.NewLine;
                }
                task = task.TrimEnd();
            }
            else
            {
                string[] parts = CommandUser.Split('"');
                if (parts.Length >= 2)
                {
                    task = parts[1];
                }
                else
                {
                    Console.WriteLine("Ошибка: используйте формат add *текст задачи* или add --multiline");
                    return;
                }
            }
            if (taskCount >= todos.Length)
            {
                ResizeArrays();
            }
            todos[taskCount] = task;
            statuses[taskCount] = true;
            dates[taskCount] = DateTime.Now;
            if (isUrgent)
            {
                todos[taskCount] = "[Срочно] " + todos[taskCount];
            }
            taskCount++;
            Console.WriteLine($"Задача добавлена: {task}");
        }

        static void CommandView(string CommandUser = "")
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Задач нет");
                return;
            }

            bool showIndex = CommandUser.Contains("--index") || CommandUser.Contains("-i");
            bool showStatus = CommandUser.Contains("--status") || CommandUser.Contains("-s");
            bool showUpdateDate = CommandUser.Contains("--update-date") || CommandUser.Contains("-d");
            bool showAll = CommandUser.Contains("--all") || CommandUser.Contains("-a");

            if (CommandUser.Contains("-") && CommandUser.Length >= 2)
            {
                int flagIndex = CommandUser.IndexOf('-');
                if (flagIndex + 1 < CommandUser.Length)
                {
                    string flagsPart = CommandUser.Substring(flagIndex + 1);
                    string[] words = flagsPart.Split(' ');
                    if (words.Length > 0 && words[0].Length > 0)
                    {
                        string flags = words[0];
                        if (flags.Length > 0 && flags[0] != '-')
                        {
                            foreach (char flag in flags)
                            {
                                if (flag == 'i') showIndex = true;
                                else if (flag == 's') showStatus = true;
                                else if (flag == 'd') showUpdateDate = true;
                                else if (flag == 'a') showAll = true;
                            }
                        }
                    }
                }
            }

            if (showAll)
            {
                showIndex = true;
                showStatus = true;
                showUpdateDate = true;
            }

            if (!showIndex && !showStatus && !showUpdateDate && !showAll)
            {
                Console.WriteLine("Ваши задачи:");
                for (int i = 0; i < taskCount; i++)
                {
                    string shortTask = todos[i].Length > 30 ? todos[i].Substring(0, 30) + "..." : todos[i];
                    Console.WriteLine($"  {shortTask}");
                }
                return;
            }

            Console.WriteLine("Ваши задачи:");

            string header = "|";
            string separator = "|";

            if (showIndex) { header += " Индекс  |"; separator += "---------|"; }
            if (showStatus) { header += " Статус       |"; separator += "--------------|"; }
            if (showUpdateDate) { header += " Дата изменения |"; separator += "-----------------|"; }
            header += " Задача                             |";
            separator += "------------------------------------|";
            Console.WriteLine(header);
            Console.WriteLine(separator);

            for (int i = 0; i < taskCount; i++)
            {
                string row = "|";

                if (showIndex)
                    row += $" {(i + 1),-7}|";

                if (showStatus)
                    row += $" {(statuses[i] ? "Сделано" : "Не сделано"),-12}|";

                if (showUpdateDate)
                    row += $" {dates[i]:dd.MM.yyyy HH:mm} |";

                string shortTask = todos[i].Replace("\n", " ");
                if (shortTask.Length > 30)
                    shortTask = shortTask.Substring(0, 30) + "...";
                row += $" {shortTask,-33}|";

                Console.WriteLine(row);
            }
        }      

        static void CommandDone(string CommandUser)
        {
            string[] parts = CommandUser.Split(' ');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int index))
            {
                index--;
                if (index >= 0 && index < taskCount)
                {
                    statuses[index] = true;
                    dates[index] = DateTime.Now;
                    Console.WriteLine($"Задача {index + 1} отмечена как выполненная");
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный индекс задачи");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат done <номер_задачи>");
            }
        }

        static void CommandDelete(string CommandUser)
        {
            string[] parts = CommandUser.Split(' ');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int index))
            {
                index--;
                if (index >= 0 && index < taskCount)
                {
                    for (int i = index; i < taskCount - 1; i++)
                    {
                        todos[i] = todos[i + 1];
                        statuses[i] = statuses[i + 1];
                        dates[i] = dates[i + 1];
                    }
                    taskCount--;
                    Console.WriteLine($"Задача {index + 1} удалена");
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный индекс задачи");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат delete *номер_задачи*");
            }
        }

        static void CommandUpdate(string CommandUser)
        {
            string[] parts = CommandUser.Split(' ', 3);
            if (parts.Length >= 3)
            {
                if (int.TryParse(parts[1], out int index))
                {
                    index--;
                    if (index >= 0 && index < taskCount)
                    {
                        string newText = parts[2].Trim('"');
                        todos[index] = newText;
                        dates[index] = DateTime.Now;
                        Console.WriteLine($"Задача {index + 1} обновлена: {newText}");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверный индекс задачи");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: используйте формат update *номер_задачи* \"новый текст\"");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат update *номер_задачи* \"новый текст\"");
            }
        }


        static void ResizeArrays()
        {
            string[] newTodos = new string[todos.Length * 2];
            bool[] newStatuses = new bool[statuses.Length * 2];
            DateTime[] newDates = new DateTime[dates.Length * 2];

            for (int i = 0; i < taskCount; i++)
            {
                newTodos[i] = todos[i];
                newStatuses[i] = statuses[i];
                newDates[i] = dates[i];
            }

            todos = newTodos;
            statuses = newStatuses;
            dates = newDates;
        }
    }
 }