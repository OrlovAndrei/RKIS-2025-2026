namespace TodoList
{
	class Program
	{
		private static string firstName;
		private static string lastName;
		private static int age;

		
		private static string[] todos = new string[2];
		private static bool[] statuses = new bool[2];
		private static DateTime[] dates = new DateTime[2];
		private static int index;

		public static void Main()
		{
			Console.WriteLine("Выполнил Герасимов Егор");
			Console.Write("Введите имя: "); 
			firstName = Console.ReadLine();
			Console.Write("Введите фамилию: ");
			lastName = Console.ReadLine();

			Console.Write("Введите год рождения: ");
			int year = int.Parse(Console.ReadLine());
			age = (DateTime.Now.Year - year);
			
			string output = "Добавлен пользователь " + firstName + " " + lastName + ", возраст - " + age;
			Console.WriteLine(output);
			
			while (true)
            {
                Console.Write("Введите команду: ");
                string command = Console.ReadLine();

	            if (command == "profile") ShowProfile();
                else if (command.StartsWith("add ")) AddTodo(command);
                else if (command.StartsWith("done ")) DoneTodo(command);
                else if (command.StartsWith("delete ")) DeleteTodo(command);
                else if (command.StartsWith("update ")) UpdateTodo(command);
                else if (command == "view") ViewTodo();
                else if (command == "exit") 
				{
					Console.WriteLine("Выход из программы.");
					break;
				}
                else if (command == "help") HelpCommand();
            }
		}
		private static void ShowProfile()
		{
			Console.WriteLine(firstName + " " + lastName + ", - " + age);
		}
		private static void ExpandArrays()
		{
			var newSize = todos.Length * 2;
			Array.Resize(ref todos, newSize);
			Array.Resize(ref statuses, newSize);
			Array.Resize(ref dates, newSize);
		}
		private static void AddTodo(string command)
		{
			string[] flags = ParseFlags(command);
			bool isMultiline = flags.Contains("--multi") ||  flags.Contains("-m") ;

			string text = command.Substring(4);
			if (isMultiline)
			{
				Console.WriteLine("Многострочный режим, введите !end для отправки");
				text = "";
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
		private static void DoneTodo(string command)
		{
			var parts = command.Split(' ', 2);
			var idx = int.Parse(parts[1]);
			statuses[index] = true;
			dates[index] = DateTime.Now;

			Console.WriteLine("Задача " + todos[idx] + " отмечена выполненной");
		}
		private static void DeleteTodo(string command)
		{
			var idx = int.Parse(command.Split(' ')[1]);

			for (var i = idx; i < index - 1; i++)
			{
				todos[i] = todos[i + 1];
				statuses[i] = statuses[i + 1];
				dates[i] = dates[i + 1];
			}

			index--;
			todos[index] = string.Empty;
			statuses[index] = false;
			dates[index] = default;
			Console.WriteLine($"Задача {index} удалена.");
		}
		private static void UpdateTodo(string command)
		{
			string[] parts = command.Split(' ', 3);
			int idx = int.Parse(parts[1]);
            
			todos[idx] = parts[2];
			dates[idx] = DateTime.Now;
			Console.WriteLine($"Задача под номером {idx} была обновлена.");
		}
		
		private static void ViewTodo()
		{
			Console.WriteLine("Задачи:");
			for (var i = 0; i < todos.Length; i++)
			{
				var todo = todos[i];
				var status = statuses[i];
				var date = dates[i];

				if (!string.IsNullOrEmpty(todo))
					Console.WriteLine(i + ") " + date + " - " + todo + " выполнена: " + status);
			}
		}
		
		private static void HelpCommand()
		{
			Console.WriteLine("Команды:");
			Console.WriteLine("help — выводит список всех доступных команд с кратким описанием");
			Console.WriteLine("profile — выводит данные пользователя");
			Console.WriteLine("add \"текст задачи\" — добавляет новую задачу");
			Console.WriteLine("done idx - отмечает задачу выполненной");
			Console.WriteLine("delete idx - удаляет задачу");
			Console.WriteLine("update idx \"текст задачи\" - обновляет задачу");
			Console.WriteLine("view — выводит все задачи");
			Console.WriteLine("exit — выход из программы");
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
	}
}