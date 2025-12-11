namespace TodoList
{
	class Program
	{
		private static string name;
		private static string surname;
		private static int age;
		
		private static string[] taskList = new string[2];
		private static bool[] statuses = new bool[2];
		private static DateTime[] dates = new DateTime[2];
		private static int count;

		public static void Main()
		{
			Console.WriteLine("Работу выполнил Турчин 3833.9");
			Console.Write("Введите имя: "); 
			name = Console.ReadLine();
			Console.Write("Введите фамилию: ");
			surname = Console.ReadLine();

			Console.Write("Введите год рождения: ");
			int year = int.Parse(Console.ReadLine());
			age = DateTime.Now.Year - year;

			Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
			
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

			var header = "|";
			if (hasIndex || hasAll) header += $"{" Индекс",-8} |";
			header += $"{" Задача",-36} |";
			if (hasStatus || hasAll) header += $"{" Статус",-18} |";
			if (hasDate || hasAll) header += $"{" Изменено",-20} |";

			Console.WriteLine(header);
			Console.WriteLine(new string('-', header.Length));

			for (var i = 0; i < count; i++)
			{
				var title = taskList[i].Replace("\n", " ");
				if (title.Length > 27) title = title.Substring(0, 27) + "...";

				var rows = "|";
				if (hasIndex || hasAll) rows += $" {(i + 1).ToString(),-8}|";
				rows += $" {title,-36}|";
				if (hasStatus || hasAll) rows += $" {(statuses[i] ? "Выполнено" : "Не выполнено"),-18}|";
				if (hasDate || hasAll) rows += $" {dates[i],-20:yyyy-MM-dd HH:mm}|";

				Console.WriteLine(rows);
			}
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
            
			if (count == taskList.Length) ExpandArrays();

			taskList[count] = text;
			statuses[count] = false;
			dates[count] = DateTime.Now;
			count++;

			Console.WriteLine($"Задача добавлена: {text}");
		}
		private static void UpdateTask(string input)
		{
			var parts = input.Split(' ', 3);
			var index = int.Parse(parts[1]) - 1;

			var newText = parts[2];
			taskList[index] = newText;
			dates[index] = DateTime.Now;
			Console.WriteLine($"Задача {index} обновлена.");
		}
		private static void DoneTask(string input)
		{
			var parts = input.Split(' ', 2);
			var index = int.Parse(parts[1]) - 1;

			statuses[index] = true;
			dates[index] = DateTime.Now;

			Console.WriteLine($"Задача {index + 1} выполнена.");
		}
		private static void DeleteTask(string input)
		{
			var parts = input.Split(' ', 2);
			var index = int.Parse(parts[1]) - 1;

			for (var i = index; i < count - 1; i++)
			{
				taskList[i] = taskList[i + 1];
				statuses[i] = statuses[i + 1];
				dates[i] = dates[i + 1];
			}

			count--;
			Console.WriteLine($"Задача {index + 1} удалена.");
		}
		private static void ExpandArrays()
		{
			var newSize = taskList.Length * 2;
			Array.Resize(ref taskList, newSize);
			Array.Resize(ref statuses, newSize);
			Array.Resize(ref dates, newSize);
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
			Console.WriteLine($"{name} {surname}, возраст - {age}");
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