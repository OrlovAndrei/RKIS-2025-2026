namespace TodoList
{
	class Program
	{
		static string name, surname;
		static int age;
		
		static string[] todos = new string[2];
		static bool[] statuses = new bool[2];
		static DateTime[] dates = new DateTime[2];
		static int taskCount = 0;
		
		static bool isRunning = true;

		public static void Main()
		{
			Console.WriteLine("Работу выполнили Николаенко Крошняк");
            
			Console.Write("Введите ваше имя: ");
			name = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			surname = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			int year = int.Parse(Console.ReadLine());
			age = DateTime.Now.Year - year;
            
			Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
			

			while (isRunning)
			{
				Console.Write("\nВведите команду: ");
				string command = Console.ReadLine();
				
				if (command == "help")
				{
					ShowHelp();
				}
				else if (command == "profile")
				{
					ShowProfile();
				}
				else if (command.StartsWith("add "))
				{
					AddTask(command);
				}
				else if (command.StartsWith("done "))
				{
					CompleteTask(command);
				}
				else if (command.StartsWith("delete "))
				{
					DeleteTask(command);
				}
				else if (command.StartsWith("update "))
				{
					UpdateTask(command);
				}
				else if (command.StartsWith("view"))
				{
					ViewTasks(command);
				}
				else if (command.StartsWith("read "))
				{
					ReadTask(command);
				}
				else if (command == "exit")
				{
					ExitProgram();
				}
				else
				{
					UnknownCommand();
				}
			}
		}
		
		private static void ShowHelp()
        {
            Console.WriteLine("""
            Доступные команды:
            help — список команд
            profile — выводит данные 
            add "текст" — добавляет задачу
            view — просмотр всех задач
            done "индекс" - отметить выполненным
            delete "индекс" - удалить задачу
            update "индекс" "текст" - изменить текст
            exit — завершить программу
            """);
        }

        private static void ShowProfile()
        {
            Console.WriteLine($"{name} {surname} {age}");
        }
        private static string[] ParseFlags(string command)
        {
	        var parts = command.Split(' ');
	        var flags = new List<string>();

	        foreach (var part in parts)
		        if (part.StartsWith("-"))
			        for (int i = 1; i < part.Length; i++) flags.Add("-" + part[i]);
		        else if (part.StartsWith("--")) flags.Add(part);

	        return flags.ToArray();
        }
        private static void AddTask(string command)
        {
            string[] flags = ParseFlags(command);
            bool isMulti = flags.Contains("--multi") ||  flags.Contains("-m") ;

            string text = command.Substring(4);
            if (isMulti)
            {
	            Console.WriteLine("Многострочный ввод, введите !end для завершения");
	            text = "";
	            while (true)
	            {
		            string line = Console.ReadLine();
		            if (line == "!end") break;
		            text += line + "\n";
	            }
            }
            
            if (taskCount == todos.Length)
	            ExpandArrays();

            todos[taskCount] = text;
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
            taskCount++;

            Console.WriteLine($"Задача добавлена: {text}");
        }

        private static void CompleteTask(string command)
        {
            var parts = command.Split(' ', 2);
            var index = int.Parse(parts[1]) - 1;

            statuses[index] = true;
            dates[index] = DateTime.Now;

            Console.WriteLine($"Задача {index + 1} выполнена.");
        }
		private static void ReadTask(string command)
        {
            var parts = command.Split(' ', 2);
            var index = int.Parse(parts[1]) - 1;
            
            Console.WriteLine($"Текст задачи: {todos[index]}");
			Console.WriteLine($"Статус: {(statuses[index] ? "Выполнено" : "Не выполнено")}");
			Console.WriteLine($"Дата изменения: {dates[index]:dd.MM.yyyy HH:mm:ss}");
        }
        private static void DeleteTask(string command)
        {
            var parts = command.Split(' ', 2);
            var index = int.Parse(parts[1]) - 1;

            for (var i = index; i < taskCount - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }

            taskCount--;
            Console.WriteLine($"Задача {index + 1} удалена.");
        }

        private static void UpdateTask(string command)
        {
            var parts = command.Split(' ', 3);
            var index = int.Parse(parts[1]);
            var task = parts[2];

            todos[index] = task;
            dates[index] = DateTime.Now;

            Console.WriteLine("Задача обновлена");
        }

        private static void ViewTasks(string command)
        {
	        var flags = ParseFlags(command);
	        var showAll = flags.Contains("--all") || flags.Contains("-a");
	        var showIndex = flags.Contains("--index") || flags.Contains("-i") || showAll;
	        var showStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
	        var showUpdateDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;

	        List<string> headers = ["Текст задачи".PadRight(36)];
	        if (showIndex) headers.Add("Индекс".PadRight(8));
	        if (showStatus) headers.Add("Статус".PadRight(16));
	        if (showUpdateDate) headers.Add("Дата обновления".PadRight(16));

	        Console.WriteLine("+-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-+");
	        Console.WriteLine("| " + string.Join(" | ", headers) + " |");
	        Console.WriteLine("|-" + string.Join("-+-", headers.Select(it => new string('-', it.Length))) + "-|");

	        for (var i = 0; i < todos.Length; i++)
	        {
		        if (string.IsNullOrEmpty(todos[i])) continue;

		        var text = todos[i].Replace("\n", " ");
		        if (text.Length > 30) text = text.Substring(0, 30) + "...";

		        var status = statuses[i] ? "выполнена" : "не выполнена";
		        var date = dates[i].ToString("yyyy-MM-dd HH:mm");

		        List<string> rows = [text.PadRight(36)];
		        if (showIndex) rows.Add(i.ToString().PadRight(8));
		        if (showStatus) rows.Add(status.PadRight(16));
		        if (showUpdateDate) rows.Add(date.PadRight(16));

		        Console.WriteLine("| " + string.Join(" | ", rows) + " |");
	        }

	        Console.WriteLine("+-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-+");
        }

        private static void ExitProgram()
        {
            Console.WriteLine("Программа завершена.");
            isRunning = false;
        }

        private static void UnknownCommand()
        {
            Console.WriteLine("Неизвестная команда");
        }

        private static void ExpandArrays()
        {
            var newSize = todos.Length * 2;
            Array.Resize(ref todos, newSize);
            Array.Resize(ref statuses, newSize);
            Array.Resize(ref dates, newSize);
        }
	}
}