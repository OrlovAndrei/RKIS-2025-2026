namespace TodoList
{
	class Program
	{
		static Profile profile = new("", "", 2000);
		static TodoList todoList = new();
		
		static bool isRunning = true;

		public static void Main()
		{
			Console.WriteLine("Работу выполнили Николаенко Крошняк");
            
			Console.Write("Введите ваше имя: ");
			var name = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			var surname = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			int year = int.Parse(Console.ReadLine());
			profile = new Profile(name, surname, year);
			Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");

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
            "add "текст" (--multiline/-m) — добавляет задачу
            "view (-index/-i, --status/-s, --update-date/-d, --all/-a) — просмотр всех задач
            done "индекс" - отметить выполненным
            delete "индекс" - удалить задачу
            update "индекс" "текст" - изменить текст
            exit — завершить программу
            """);
        }

        private static void ShowProfile() => Console.WriteLine(profile.GetInfo());
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
            
            todoList.Add(new TodoItem(text));
        }

        private static void CompleteTask(string command)
        {
            var parts = command.Split(' ', 2);
            var index = int.Parse(parts[1]) - 1;

            todoList.MarkDone(index);
        }
		private static void ReadTask(string command)
        {
            var parts = command.Split(' ', 2);
            var index = int.Parse(parts[1]) - 1;

            todoList.Read(index);
        }
        private static void DeleteTask(string command)
        {
            var parts = command.Split(' ', 2);
            var index = int.Parse(parts[1]) - 1;

            todoList.Delete(index);
        }

        private static void UpdateTask(string command)
        {
            var parts = command.Split(' ', 3);
            var index = int.Parse(parts[1]) - 1;
            var task = parts[2];

            todoList.Update(index, task);
        }

        private static void ViewTasks(string command)
        {
	        var flags = ParseFlags(command);
	        var showAll = flags.Contains("--all") || flags.Contains("-a");
	        var showIndex = flags.Contains("--index") || flags.Contains("-i") || showAll;
	        var showStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
	        var showUpdateDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;

	        todoList.View(showIndex, showStatus, showUpdateDate);
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
	}
}