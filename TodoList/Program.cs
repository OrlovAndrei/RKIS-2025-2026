namespace TodoList
{
    class Program
    {
        static TodoList todoList = new ();
		static Profile profile;
        public static void Main()
        {
	        Console.WriteLine("Работу выполнили Поплевин и Музыка 3831");
	        Console.Write("Введите ваше имя: ");
	        var userFirstName = Console.ReadLine();
	        Console.Write("Введите вашу фамилию: ");
	        var userLastName = Console.ReadLine();
	        Console.Write("Введите ваш год рождения: ");
	        var userBirthYear = int.Parse(Console.ReadLine());
	        profile = new Profile(userFirstName, userLastName, userBirthYear);
	        Console.WriteLine(profile.GetInfo());;

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
                else if (command.StartsWith("read ")) ReadTask(command);

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

			todoList.Update(idx, parts[2]);

        }

        private static void DeleteTask(string command)
        {
            string[] parts = command.Split(' ');
            var idx = int.Parse(parts[1]);

			todoList.Delete(idx);
        }

        private static void ViewTasks(string command)
        {
	        var flags = ParseFlags(command);

	        bool showAll = flags.Contains("--all") || flags.Contains("-a");
	        bool showIndex = flags.Contains("--index") || flags.Contains("-i") ||  showAll;
	        bool showStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
	        bool showUpdateDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;

			todoList.View(showIndex, showStatus, showUpdateDate);
        }

		private static void ReadTask(string command)
		{
			string[] parts = command.Split(' ');
			var idx = int.Parse(parts[1]);

			todoList.Read(idx);
		}

		private static void DoneTask(string command)
		{
			string[] parts = command.Split(' ');
			var idx = int.Parse(parts[1]);

			todoList.MarkDone(idx);
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
	        todoList.Add(new TodoItem(text));
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
	        Console.WriteLine(profile.GetInfo());
		}

        private static void ShowHelp()
        {
            Console.WriteLine("""
            Доступные команды:
            help — список команд
            profile — выводит данные профиля
            add "текст задачи" — добавляет задачу
              -m, --multi — добавление задачи в несколько строк
            view — просмотр всех задач
              -a, --all — добавить все поля таблицы
              -i, --index — добавить поле индекса
              -s, --status — добавить поле статуса
              -d, --update-date — добавить поле даты
            done <индекс> — отметить задачу выполненной
            delete <индекс> — удалить задачу
            update <индекс> "новый текст" — изменить текст задачи
            read <индекс> — просмотр задачи
            exit — завершить программу
            """);
        }
       
    }
}