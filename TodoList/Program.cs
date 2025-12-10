namespace TodoList
{
	class Program
	{
		private const string CommandHelp = "help";
		private const string CommandProfile = "profile";
		private const string CommandAdd = "add";
		private const string CommandView = "view";
		private const string CommandRead = "read";
		private const string CommandExit = "exit";

		private const string FlagMultiline = "--multiline";
		private const string FlagShortMultiline = "-m";

		private const string FlagIndex = "--index";
		private const string FlagShortIndex = "-i";
		private const string FlagStatus = "--status";
		private const string FlagShortStatus = "-s";
		private const string FlagDate = "--update-date";
		private const string FlagShortDate = "-d";
		private const string FlagAll = "--all";
		private const string FlagShortAll = "-a";

		private static string firstName;
		private static string lastName;
		private static int birthYear;

		private static string[] todos = new string[2];
		private static int index = 0;

		private static bool[] statuses = new bool[2];
		private static DateTime[] dates = new DateTime[2];

		public static void Main()
		{
			Console.WriteLine("Работу выполнил: Измайлов");

			Console.Write("Введите ваше имя: ");
			firstName = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			lastName = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			birthYear = int.Parse(Console.ReadLine());
			int age = DateTime.Now.Year - birthYear;

			Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

			while (true)
			{
				Console.Write("Введите команду: ");
				string command = Console.ReadLine();

				if (command == CommandHelp)
				{
					ShowHelp();
				}
				else if (command == CommandProfile)
				{
					ShowProfile();
				}
				else if (command.StartsWith(CommandAdd))
				{
					AddTask(command);
				}
				else if (command.StartsWith(CommandView))
				{
					ViewTasks(command);
				}
				else if (command.StartsWith(CommandRead))
				{
					ReadTask(command);
				}
				else if (command == CommandExit)
				{
					Console.WriteLine("Программа завершена.");
					break;
				}
				else
				{
					Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
				}
			}
		}

		private static void ShowHelp()
		{
			Console.WriteLine("""
            Доступные команды:
            help — список команд
            profile — выводит данные профиля
            add "текст задачи" [--multiline | -m] — добавляет задачу. Флаг --multiline (-m) позволяет вводить задачу в несколько строк до команды !end.
            view [флаги] — просмотр всех задач. Показывает только текст задачи по умолчанию.
                --index, -i — показать индекс задачи
                --status, -s — показать статус задачи (сделано/не сделано)
                --update-date, -d — показать дату последнего изменения
                --all, -a — показать все данные
            read <idx> — просмотр полного текста задачи, статуса и даты по индексу
            exit — завершить программу
            """);
		}

		private static void ShowProfile()
		{
			Console.WriteLine($"{firstName} {lastName}, {birthYear}");
		}

		private static void AddTask(string command)
		{
			string task = "";
			bool isMultiline = false;
			string args = command.Length > CommandAdd.Length ? command.Substring(CommandAdd.Length).Trim() : string.Empty;

			if (args.EndsWith(FlagMultiline) || args.EndsWith(FlagShortMultiline))
			{
				isMultiline = true;
				// Удаляем флаг из аргументов, чтобы оставшийся текст (если есть) стал первой строкой.
				if (args.EndsWith(FlagMultiline))
				{
					task = args.Substring(0, args.Length - FlagMultiline.Length).Trim();
				}
				else
				{
					task = args.Substring(0, args.Length - FlagShortMultiline.Length).Trim();
				}
			}
			else
			{
				// Однострочный режим
				task = args;
			}

			if (isMultiline)
			{
				Console.WriteLine("Введите задачу (для завершения введите !end):");
				string line;
				System.Text.StringBuilder taskBuilder = new System.Text.StringBuilder();

				if (!string.IsNullOrEmpty(task))
				{
					taskBuilder.AppendLine(task);
				}

				while (true)
				{
					Console.Write("> ");
					line = Console.ReadLine();
					if (line.Trim().Equals("!end", StringComparison.OrdinalIgnoreCase))
					{
						break;
					}
					taskBuilder.AppendLine(line);
				}
				task = taskBuilder.ToString().TrimEnd();

				if (string.IsNullOrEmpty(task))
				{
					Console.WriteLine("Задача не добавлена (пустой текст).");
					return;
				}
			}
			else if (string.IsNullOrEmpty(task))
			{
				Console.WriteLine("Неверный формат команды. Используйте: add \"текст задачи\"");
				return;
			}

			if (index >= todos.Length)
			{
				ExpandArray();
			}

			todos[index] = task;
			statuses[index] = false;
			dates[index] = DateTime.Now;

			index++;
			Console.WriteLine($"Задача добавлена: {task}");
		}

		private static void ExpandArray()
		{
			string[] newTodos = new string[todos.Length * 2];
			bool[] newStatuses = new bool[statuses.Length * 2];
			DateTime[] newDates = new DateTime[dates.Length * 2];

			for (int i = 0; i < todos.Length; i++)
				newTodos[i] = todos[i];

			for (int i = 0; i < statuses.Length; i++)
				newStatuses[i] = statuses[i];

			for (int i = 0; i < dates.Length; i++)
				newDates[i] = dates[i];

			todos = newTodos;
			statuses = newStatuses;
			dates = newDates;
		}

		private static void ViewTasks(string command)
		{
			string flags = command.Length > CommandView.Length ? command.Substring(CommandView.Length).Trim() : string.Empty;

			bool showIndex = flags.Contains(FlagIndex) || flags.Contains(FlagShortIndex) || flags.Contains(FlagAll) || flags.Contains(FlagShortAll);
			bool showStatus = flags.Contains(FlagStatus) || flags.Contains(FlagShortStatus) || flags.Contains(FlagAll) || flags.Contains(FlagShortAll);
			bool showDate = flags.Contains(FlagDate) || flags.Contains(FlagShortDate) || flags.Contains(FlagAll) || flags.Contains(FlagShortAll);

			int indexWidth = index.ToString().Length;
			if (indexWidth < 5) indexWidth = 5;
			int taskWidth = 30;
			int statusWidth = 10;
			int dateWidth = 19;

			string header = "";
			if (showIndex) header += $"{"Инд",-indexWidth} ";
			header += $"{"Задача",-taskWidth} ";
			if (showStatus) header += $"{"Статус",-statusWidth} ";
			if (showDate) header += $"{"Дата",-dateWidth}";

			Console.WriteLine("Список задач:");

			if (header.Length > 0)
			{
				Console.WriteLine(header.TrimEnd());
				Console.WriteLine(new string('-', header.Length));
			}

			for (int i = 0; i < index; i++)
			{
				string output = "";

				if (showIndex)
				{
					output += $"{(i + 1),-indexWidth} ";
				}

				string taskText = todos[i];
				if (taskText.Length > taskWidth)
				{
					taskText = taskText.Substring(0, taskWidth - 3) + "...";
				}
				output += $"{taskText,-taskWidth} ";

				if (showStatus)
				{
					string statusText = statuses[i] ? "сделано" : "не сделано";
					output += $"{statusText,-statusWidth} ";
				}

				if (showDate)
				{
					string dateText = dates[i].ToString("yyyy-MM-dd HH:mm:ss");
					output += $"{dateText,-dateWidth}";
				}

				Console.WriteLine(output.TrimEnd());
			}
		}

		private static void ReadTask(string command)
		{
			string[] parts = command.Split(" ", 2);
			if (parts.Length != 2)
			{
				Console.WriteLine("Неверный формат команды. Используйте: read <idx>");
				return;
			}

			if (!int.TryParse(parts[1], out int taskIndex) || taskIndex <= 0 || taskIndex > index)
			{
				Console.WriteLine($"Неверный индекс задачи. Допустимые значения от 1 до {index}.");
				return;
			}

			int i = taskIndex - 1;

			Console.WriteLine("Полный текст задачи:");
			Console.WriteLine($"\t{todos[i]}");
			Console.WriteLine($"Статус: {(statuses[i] ? "Выполнена" : "Не выполнена")}");
			Console.WriteLine($"Дата последнего изменения: {dates[i].ToString("yyyy-MM-dd HH:mm:ss")}");
		}
	}
}