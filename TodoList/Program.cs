namespace TodoList
{
    class Program
    {
        static string name;
        static string surname;
        static int age;
        public static void Main()
        {
            Console.WriteLine("Работу выполнили: Галстян и Дзуцев");
            CreateUser();

            string[] todos = new string[2];
            bool[] statuses = new bool[2];
            DateTime[] dates = new DateTime[2];
            int index = 0;

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
                {
                    HelpCommand();
                }
                else if (input == "profile")
                {
                    ProfileCommand();
                }
                else if (input.StartsWith("add"))
                {
                    AddCommand(input, todos, statuses, dates, ref index);
                }
                else if (input.StartsWith("done "))
                {
                    DoneCommand(input, statuses, dates);
                }
                else if (input.StartsWith("update "))
                {
                    UpdateCommand(input, todos, dates);
                }
                else if (input.StartsWith("delete "))
                {
                    DeleteCommand(input, todos, statuses, dates, ref index);
                }
                else if (input.StartsWith("view"))
                {
                    ViewCommand(input, todos, statuses, dates, index);
                }
                else if (input.StartsWith("read"))
                {
	                ReadCommand(input, todos, statuses, dates);
                }
                else
                {
                    Console.WriteLine("Неизвестная команда.");
                }
            }
        }

        private static void DeleteCommand(string command, string[] todos, bool[] statuses, DateTime[] dates, ref int index)
        {
            int idx = int.Parse(command.Split(' ')[1]) - 1;
            
            for (int i = idx; i < index - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }
            
            index--;
            todos[index] = string.Empty;
            statuses[index] = false;
            dates[index] = default;
            Console.WriteLine($"Задача под номером {idx + 1} была удалена.");
        }

        private static void UpdateCommand(string command, string[] todos, DateTime[] dates)
        {
            string[] parts = command.Split(' ', 3);
            int index = int.Parse(parts[1]) - 1;
            
            todos[index] = parts[2];
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача под номером {index + 1} была обновлена.");
        }

        private static void DoneCommand(string command, bool[] statuses, DateTime[] dates)
        {
            int index = int.Parse(command.Split(' ')[1]) - 1;
            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача под номером {index + 1} отмечена выполненной.");
        }

        private static void ViewCommand(string command, string[] todos, bool[] statuses, DateTime[] dates, int index)
		{
			string[] flags = ParseFlags(command);

			bool hasAll = flags.Contains("--all") || flags.Contains("-a");
		    bool hasIndex = flags.Contains("--index") || flags.Contains("-i");
		    bool hasStatus = flags.Contains("--status") || flags.Contains("-s");
		    bool hasDate = flags.Contains("--update-date") || flags.Contains("-d");

		    int indexWidth = 7;
		    int textWidth = 33;
		    int statusWidth = 12;
		    int dateWidth = 19;

		    string headerRow = "";
		    if (hasAll || hasIndex) headerRow += string.Format("{0,-" + indexWidth + "}|", "Индекс");
		    headerRow += string.Format("{0,-" + textWidth + "}|", "Текст задачи");
		    if (hasAll || hasStatus) headerRow += string.Format("{0,-" + statusWidth + "}|", "Статус");
		    if (hasAll || hasDate) headerRow += string.Format("{0,-" + dateWidth + "}|", "Дата обновления");

		    Console.WriteLine(headerRow);
		    Console.WriteLine(new string('-', headerRow.Length));

		    for (int i = 0; i < index; i++)
		    {
		        if (string.IsNullOrEmpty(todos[i])) continue;

		        string text = todos[i];
		        if (text.Length > 30) text = text.Substring(0, 30) + "...";

		        string status = statuses[i] ? "выполнена" : "не выполнена";
		        string date = dates[i].ToString("yyyy-MM-dd HH:mm");

		        string row = "";
		        if (hasAll || hasIndex) row += string.Format("{0,-" + indexWidth + "}|", i + 1);
		        row += string.Format("{0,-" + textWidth + "}|", text);
		        if (hasAll || hasStatus) row += string.Format("{0,-" + statusWidth + "}|", status);
		        if (hasAll || hasDate) row += string.Format("{0,-" + dateWidth + "}|", date);

		        Console.WriteLine(row);
		    }
		}
        
		private static void ReadCommand(string command, string[] todos, bool[] statuses, DateTime[] dates)
		{
			string[] parts = command.Split(' ', 3);
			int index = int.Parse(parts[1]) - 1;
			
			Console.WriteLine($"{index + 1}) {todos[index]}, сделано:{statuses[index]}, {dates[index]}");
		}

        private static void AddCommand(string command, string[] todos, bool[] statuses, DateTime[] dates, ref int index)
        {
	        string[] flags = ParseFlags(command);
	        bool isMultiTask = flags.Contains("--multi") ||  flags.Contains("-m") ;

	        string text = "";
            if (!isMultiTask)
            {
	            text = command.Substring(4);
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
            
            if (index == todos.Length)
            {
                int newSize = todos.Length * 2;
                Array.Resize(ref todos, newSize);
                Array.Resize(ref statuses, newSize);
                Array.Resize(ref dates, newSize);
            }

            todos[index] = text;
            statuses[index] = false;
            dates[index] = DateTime.Now;
            index++;

            Console.WriteLine($"Добавлена задача: \"{text}\"");
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
            Console.WriteLine($"{name} {surname}, {age}");
        }

        private static void HelpCommand()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - вывести список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add текст задачи - добавить новую задачу (флаги: --multiline -m)");
            Console.WriteLine("view - показать все задачи (флаги: --all -a, --index -i, --status -s, --update-date -d)");
            Console.WriteLine("read idx - просмотр задачи");
            Console.WriteLine("done idx - отмечает задачу выполненной");
            Console.WriteLine("delete idx - удаляет задачу");
            Console.WriteLine("update idx - обновляет задачу");
            Console.WriteLine("exit - выйти из программы");
        }

        private static void CreateUser()
        {
            Console.Write("Введите ваше имя: ");
            name = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            surname = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            var year = int.Parse(Console.ReadLine());
            age = DateTime.Now.Year - year;
            
            Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
        }
    }
}