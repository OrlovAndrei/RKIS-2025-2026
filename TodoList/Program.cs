namespace TodoList
{
	class Program
	{
		static Profile profile;
		static TodoList todos = new();
		
		public static void Main()
		{
			Console.WriteLine("Работу выполнил Турчин 3833.9");
			Console.Write("Введите имя: "); 
			var name = Console.ReadLine();
			Console.Write("Введите фамилию: ");
			var surname = Console.ReadLine();
			Console.Write("Введите год рождения: ");
			int year = int.Parse(Console.ReadLine());
			
			profile = new Profile(name, surname, year);
			Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");

			while (true)
			{
				Console.WriteLine("Введите команду:");
				var command = Console.ReadLine();

				if (command == "help") Help();
				else if (command == "profile") Profile();
				else if (command.StartsWith("add ")) AddTask(command);
				else if (command.StartsWith("update ")) UpdateTask(command);
				else if (command.StartsWith("done ")) DoneTask(command);
				else if (command.StartsWith("delete ")) DeleteTask(command);
				else if  (command.StartsWith("view")) ViewTasks(command);
				else if  (command.StartsWith("read ")) ReadTask(command);
				else if (command == "exit")
				{
					Console.WriteLine("Программа завершена");
					break;
				}
				else Console.WriteLine("Введите help для списка команд");
			}
		}
		
		private static void ViewTasks(string command)
		{
			var flags = ParseFlags(command);

			var hasAll = flags.Contains("--all") || flags.Contains("-a");
			var hasIndex = flags.Contains("--index") || flags.Contains("-i");
			var hasStatus = flags.Contains("--status") || flags.Contains("-s");
			var hasDate = flags.Contains("--update-date") || flags.Contains("-d");

			todos.View(hasIndex, hasStatus, hasDate, hasAll);
		}
		private static void ReadTask(string input)
		{
			var parts = input.Split(' ', 2);
			var taskIndex = int.Parse(parts[1]) - 1;

			todos.Read(taskIndex);
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
            
			todos.Add(new TodoItem(text));
		}
		private static void UpdateTask(string input)
		{
			var parts = input.Split(' ', 3);
			var index = int.Parse(parts[1]) - 1;

			var newText = parts[2];
			todos.Update(index, newText);
		}
		private static void DoneTask(string input)
		{
			var parts = input.Split(' ', 2);
			var index = int.Parse(parts[1]) - 1;

			todos.MarkDone(index);
		}
		private static void DeleteTask(string input)
		{
			var parts = input.Split(' ', 2);
			var index = int.Parse(parts[1]) - 1;

			todos.Delete(index);
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
		private static void Profile()
		{
			Console.WriteLine(profile.GetInfo());
		}

		private static void Help()
		{
			Console.WriteLine(
				"""
				Доступные команды:
				help — список команд
				profile — выводит данные профиля
				add "текст" — добавляет задачу
				done "индекс" - отметить выполненным
				delete "индекс" - удалить задачу
				update "индекс" "текст" - изменить текст выбранной задачи
				view — просмотр всех задач
				exit — завершить программу
				"""
			);
		}
	}
}