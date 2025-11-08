namespace TodoList
{
    class Program
    {
        static string userFirstName;
        static string userLastName;
        static int userBirthYear;

        static string[] tasks = new string[2];
        static bool[] statuses = new bool[2];
        static DateTime[] dates = new DateTime[2];
        static int taskCount = 0;

        const int indexWidth = 6;
        const int textWidth = 36;
        const int statusWidth = 14;
        const int dateWidth = 16;
        
        public static void Main()
        {
	        Console.WriteLine("Работу выполнили Поплевин и Музыка 3831");
            Console.Write("Введите ваше имя: ");
            userFirstName = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            userLastName = Console.ReadLine();
            Console.Write("Введите ваш год рождения: ");
            userBirthYear = int.Parse(Console.ReadLine());
            Console.WriteLine($"Добавлен пользователь {userFirstName} {userLastName}, возраст - {DateTime.Now.Year - userBirthYear}");

            while (true)
            {
                Console.Write("\nВведите команду: ");
                var command = Console.ReadLine();

                if (command == "help") ShowHelp();
                else if (command == "profile") ShowProfile();
                else if (command.StartsWith("add ")) AddTask(command);
                else if (command.StartsWith("view")) ViewTasks(command);
                else if (command.StartsWith("done ")) DoneTask(command);
                else if (command.StartsWith("delete ")) DeleteTask(command);
                else if (command.StartsWith("update ")) UpdateTask(command);
                else if (command == "exit")
                {
                    Console.WriteLine("Программа завершена.");
                    break;
                }
                else Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
            }
        }

        private static void UpdateTask(string command)
        {
            var parts = command.Split(' ', 3);
            int idx = int.Parse(parts[1]);

            string newText = parts[2];
            tasks[idx] = newText;
            dates[idx] = DateTime.Now;
            Console.WriteLine($"Задача {idx} обновлена.");
        }

        private static void DeleteTask(string command)
        {
            string[] parts = command.Split(' ');
            var idx = int.Parse(parts[1]);

            for (var i = idx; i < taskCount - 1; i++)
            {
                tasks[i] = tasks[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }

            taskCount--;
            Console.WriteLine($"Задача {idx} удалена.");

        }

        private static void ViewTasks(string command)
        {
	        var flags = ParseFlags(command);

	        bool showAll = flags.Contains("--all") || flags.Contains("-a");
	        bool showIndex = flags.Contains("--index") || flags.Contains("-i") ||  showAll;
	        bool showStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
	        bool showUpdateDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;

	        List<string> headers = ["Текст задачи".PadRight(textWidth)];
	        if (showIndex) headers.Add("Индекс".PadRight(indexWidth));
	        if (showStatus) headers.Add("Статус".PadRight(statusWidth));
	        if (showUpdateDate) headers.Add("Дата обновления".PadRight(dateWidth));

	        Console.WriteLine("--" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "--");
	        Console.WriteLine("| " + string.Join(" | ", headers) + " |");
	        Console.WriteLine("|-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-|");

	        for (int i = 0; i < taskCount; i++)
	        {
		        if (string.IsNullOrEmpty(tasks[i])) continue;

		        string text = tasks[i].Replace("\n", " ");
		        if (text.Length > 30) text = text.Substring(0, 30) + "...";

		        string status = statuses[i] ? "выполнена" : "не выполнена";
		        string date = dates[i].ToString("yyyy-MM-dd HH:mm");

		        List<string> rows = [text.PadRight(textWidth)];
		        if (showIndex) rows.Add((i + 1).ToString().PadRight(indexWidth));
		        if (showStatus) rows.Add(status.PadRight(statusWidth));
		        if (showUpdateDate) rows.Add(date.PadRight(dateWidth));

		        Console.WriteLine("| " + string.Join(" | ", rows) + " |");
	        }
	        Console.WriteLine("--" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "--");
        }

        private static void DoneTask(string command)
        {
            var parts = command.Split(' ', 2);
            int idx = int.Parse(parts[1]);

            statuses[idx] = true;
            dates[idx] = DateTime.Now;
            Console.WriteLine($"Задача {idx} отмечена как выполненная.");
        }

        private static void AddTask(string command)
        {
	        var flags = ParseFlags(command);
	        bool isMultiTask = flags.Contains("-m") || flags.Contains("--multi");
            
	        string text = "";
	        if (isMultiTask)
	        {
		        Console.WriteLine("Многострочный режим введите !q для выхода");

		        while (true)
		        {
			        string line = Console.ReadLine();
			        if (line == "!q") break;
			        text += line + "\n";
		        }
	        }
	        else text = command.Split("add ", 2)[1];
	        
            if (taskCount == tasks.Length)
            {
                ExpandArrays();
            }

            tasks[taskCount] = text;
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
            taskCount++;
            Console.WriteLine($"Задача добавлена: {text}");
        }

        private static string[] ParseFlags(string command)
        {
	        List<string> flags = new();

	        foreach (var part in command.Split(' '))
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
        
        private static void ShowProfile()
        {
            Console.WriteLine($"{userFirstName} {userLastName}, {userBirthYear}");
        }

        private static void ShowHelp()
        {
            Console.WriteLine("""
            Доступные команды:
            help — список команд
            profile — выводит данные профиля
            add "текст задачи" — добавляет задачу
            view — просмотр всех задач
            done <индекс> — отметить задачу выполненной
            delete <индекс> — удалить задачу
            update <индекс> "новый текст" — изменить текст задачи
            exit — завершить программу
            """);
        }
        private static void ExpandArrays()
        {
            var newSize = tasks.Length * 2;
            Array.Resize(ref tasks, newSize);
            Array.Resize(ref statuses, newSize);
            Array.Resize(ref dates, newSize);
        }
    }
}