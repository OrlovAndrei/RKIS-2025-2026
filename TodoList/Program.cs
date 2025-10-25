namespace TodoList
{
	
    class Program
    {
	    static Profile profile;
	    static TodoList todos = new();
        public static void Main()
        {
            Console.WriteLine("Работу выполнили: Галстян и Дзуцев");
            CreateUser();

            while (true)
            {
                Console.Write("Введите команду: ");
                string input = Console.ReadLine();
                
                if (input == "exit")
                {
                    Console.WriteLine("Выход из программы.");
                    break;
                }
                
                if (input == "help")
	                HelpCommand();
                else if (input == "profile")
	                ProfileCommand();
                else if (input.StartsWith("add"))
	                AddCommand(input);
                else if (input.StartsWith("done "))
	                DoneCommand(input);
                else if (input.StartsWith("update "))
	                UpdateCommand(input);
                else if (input.StartsWith("delete "))
	                DeleteCommand(input);
                else if (input.StartsWith("view"))
	                ViewCommand(input);
                else if (input.StartsWith("read"))
	                ReadCommand(input);
                else
	                Console.WriteLine("Неизвестная команда.");
            }
        }

        private static void DeleteCommand(string command)
        {
            int idx = int.Parse(command.Split(' ')[1]) - 1;
            todos.Delete(idx);
        }

        private static void UpdateCommand(string command)
        {
            string[] parts = command.Split(' ', 3);
            
            int idx = int.Parse(parts[1]) - 1;
            string newText = parts[2].Trim('"');
            todos.Update(idx, newText);
        }

        private static void DoneCommand(string command)
        {
            int idx = int.Parse(command.Split(' ')[1]) - 1;
            todos.MarkDone(idx);
        }

        private static void ViewCommand(string command)
		{
			string[] flags = ParseFlags(command);

			bool hasAll = flags.Contains("--all") || flags.Contains("-a");
		    bool hasIndex = flags.Contains("--index") || flags.Contains("-i");
		    bool hasStatus = flags.Contains("--status") || flags.Contains("-s");
		    bool hasDate = flags.Contains("--update-date") || flags.Contains("-d");

		    todos.View(hasIndex, hasStatus, hasDate, hasAll);
		}
        
		private static void ReadCommand(string command)
		{
			int idx = int.Parse(command.Split(' ')[1]) - 1;
			todos.Read(idx);
		}

        private static void AddCommand(string command)
        {
	        string[] flags = ParseFlags(command);
	        bool isMultiTask = flags.Contains("--multi") ||  flags.Contains("-m") ;

	        string text = "";
            if (!isMultiTask)
            {
	            text = ExtractTaskText(command);
            }
            else
            {
	            Console.WriteLine("Многострочный режим, введите !end для отправки");

	            while (true)
	            {
		            string line = Console.ReadLine();
		            if (line == "!end") break;
		            text += line + "\n";
	            }
            }
            
            todos.Add(new TodoItem(text));
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
        
        private static string[] ParseFlags(string command)
        {
	        var parts = command.Split(' ');
	        var flags = new List<string>();

	        foreach (var part in parts)
	        {
		        if (part.StartsWith("-"))
		        {
			        for (int i = 1; i < part.Length; i++)
			        {
				        flags.Add("-" + part[i]);
			        }
		        }
		        else if (part.StartsWith("--"))
		        {
			        flags.Add(part);
		        }
	        }

	        return flags.ToArray();
        }

        private static void ProfileCommand()
        {
	        Console.WriteLine(profile.GetInfo());
        }

        private static void HelpCommand()
        {
            Console.WriteLine(
	        @"Доступные команды:
            help - вывести список команд
            profile - показать данные пользователя
            add текст задачи - добавить новую задачу (флаги: --multiline -m)
            view - показать все задачи (флаги: --all -a, --index -i, --status -s, --update-date -d)
            read idx - просмотр задачи
	            done idx - отмечает задачу выполненной
            delete idx - удаляет задачу
	            update idx - обновляет задачу
            exit - выйти из программы"
            );
        }

        private static void CreateUser()
        {
            Console.Write("Введите ваше имя: ");
            var name = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            var surname = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            var year = int.Parse(Console.ReadLine());
            
            profile = new Profile(name, surname, year);
            Console.WriteLine(profile.GetInfo());
        }
    }
}