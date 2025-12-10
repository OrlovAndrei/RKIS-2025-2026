namespace TodoList
{
	class Program
	{
		private const string CommandHelp = "help";
		private const string CommandProfile = "profile";
		private const string CommandAdd = "add";
		private const string CommandView = "view";
		private const string CommandExit = "exit";
		private const string FlagMultiline = "--multiline";
		private const string FlagShortMultiline = "-m";
		private const string FlagIncomplete = "-i";
		private const string FlagShortIncomplete = "-s";

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
            add "текст задачи" [--multiline | -m] — добавляет задачу. Флаг --multiline (-m) позволяет вводить задачу в несколько строк до пустой строки.
            view [-i | -s] — просмотр всех задач. Флаг -i (-s) показывает только незавершенные задачи.
            exit — завершить программу
            """);
		}

		private static void ShowProfile()
		{
			Console.WriteLine($"{firstName} {lastName}, {birthYear}");
		}

		private static void AddTask(string command)
		{
			string[] parts = command.Split(' ', 3);
			string task = "";
			bool isMultiline = false;

			if (parts.Length > 1)
			{
				if (parts[1].Equals(FlagMultiline) || parts[1].Equals(FlagShortMultiline))
				{
					isMultiline = true;
				}
				else if (parts.Length > 2 && (parts[2].Equals(FlagMultiline) || parts[2].Equals(FlagShortMultiline)))
				{
					isMultiline = true;
					task = parts[1];
				}
				else
				{
					task = parts[1];
				}
			}

			if (isMultiline)
			{
				Console.WriteLine("Введите задачу (завершите пустой строкой):");
				string line;
				while (!string.IsNullOrEmpty(line = Console.ReadLine()))
				{
					if (task.Length > 0)
					{
						task += Environment.NewLine;
					}
					task += line;
				}
			}
			else if (task.Length == 0)
			{
				task = command.Split(" ", 2)[1];
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
			bool incompleteOnly = command.Contains(FlagIncomplete) || command.Contains(FlagShortIncomplete);

			Console.WriteLine("Список задач:");
			for (int i = 0; i < index; i++)
			{
				if (!incompleteOnly || !statuses[i])
				{
					Console.WriteLine($"{i + 1} {todos[i]} {(statuses[i] ? "сделано" : "не сделано")} {dates[i]}");
				}
			}
		}
	}
}