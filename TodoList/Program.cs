namespace TodoList;

internal class Program
{
	private static string name;
	private static string surname;
	private static int age;

	private static string[] taskList = new string[2];
	private static bool[] taskStatuses = new bool[2];
	private static DateTime[] taskDates = new DateTime[2];
	private static int taskCount;

	public static void Main()
	{
		Console.WriteLine("Работу выполнили Антонов и Мадойкин 3833");
		Console.Write("Введите имя: ");
		name = Console.ReadLine();
		Console.Write("Введите фамилию: ");
		surname = Console.ReadLine();

		Console.Write("Введите год рождения: ");
		var year = int.Parse(Console.ReadLine());
		age = DateTime.Now.Year - year;

		Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");

		while (true)
		{
			Console.WriteLine("Введите команду: ");
			var command = Console.ReadLine();

			if (command == "help")
				Help();
			else if (command == "profile")
				Profile();
			else if (command.StartsWith("add "))
				AddTask(command);
			else if (command.StartsWith("view"))
				ViewTasks(command);
			else if (command.StartsWith("done "))
				DoneTask(command);
			else if (command.StartsWith("delete "))
				DeleteTask(command);
			else if (command.StartsWith("update "))
				UpdateTask(command);
			else if (command.StartsWith("read"))
				ReadTask(command);
			else if (command == "exit")
			{
				Console.WriteLine("Программа завершена.");
				break;
			}
			else
				Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
		}
	}

	private static void ReadTask(string input)
	{
		var parts = input.Split(' ', 2);
		var taskIndex = int.Parse(parts[1]) - 1;

		Console.WriteLine($"Полная информация о задаче {taskIndex}");
		Console.WriteLine($"Текст: {taskList[taskIndex]}");
		Console.WriteLine($"Статус: {(taskStatuses[taskIndex] ? "Выполнено" : "Не выполнено")}");
		Console.WriteLine($"Изменено: {taskDates[taskIndex]:dd.MM.yyyy HH:mm:ss}");
	}

	private static void UpdateTask(string input)
	{
		var parts = input.Split(' ', 3);
		var taskIndex = int.Parse(parts[1]) - 1;

		var newText = parts[2];
		taskList[taskIndex] = newText;
		taskDates[taskIndex] = DateTime.Now;
		Console.WriteLine($"Задача {taskIndex} обновлена.");
	}

	private static void DeleteTask(string input)
	{
		var parts = input.Split(' ', 2);
		var taskIndex = int.Parse(parts[1]) - 1;

		for (var i = taskIndex; i < taskCount - 1; i++)
		{
			taskList[i] = taskList[i + 1];
			taskStatuses[i] = taskStatuses[i + 1];
			taskDates[i] = taskDates[i + 1];
		}

		taskCount--;
		Console.WriteLine($"Задача {taskIndex + 1} удалена.");
	}

	private static void DoneTask(string input)
	{
		var parts = input.Split(' ', 2);
		var taskIndex = int.Parse(parts[1]) - 1;

		taskStatuses[taskIndex] = true;
		taskDates[taskIndex] = DateTime.Now;

		Console.WriteLine($"Задача {taskIndex + 1} выполнена.");
	}

	private static void ViewTasks(string input)
	{
		var flags = ParseFlags(input);

		var hasAll = flags.Contains("--all") || flags.Contains("-a");
		var hasIndex = flags.Contains("--index") || flags.Contains("-i");
		var hasStatus = flags.Contains("--status") || flags.Contains("-s");
		var hasDate = flags.Contains("--update-date") || flags.Contains("-d");

		var header = "|";
		if (hasIndex || hasAll) header += " Индекс".PadRight(8) + " |";
		header += " Задача".PadRight(36) + " |";
		if (hasStatus || hasAll) header += " Статус".PadRight(18) + " |";
		if (hasDate || hasAll) header += " Изменено".PadRight(18) + " |";

		Console.WriteLine(header);
		Console.WriteLine(new string('-', header.Length));

		for (var i = 0; i < taskCount; i++)
		{
			var title = taskList[i].Replace("\n", " ");
			if (title.Length > 30) title = title.Substring(0, 30) + "...";

			var rows = "|";
			if (hasIndex || hasAll) rows += " " + (i + 1).ToString().PadRight(8) + "|";
			rows += " " + title.PadRight(36) + "|";
			if (hasStatus || hasAll) rows += " " + (taskStatuses[i] ? "Выполнено" : "Не выполнено").PadRight(18) + "|";
			if (hasDate || hasAll) rows += " " + taskDates[i].ToString("yyyy-MM-dd HH:mm").PadRight(18) + "|";

			Console.WriteLine(rows);
		}
	}

	private static void AddTask(string input)
	{
		var flags = ParseFlags(input);

		if (flags.Contains("-m") || flags.Contains("--multi"))
		{
			Console.WriteLine("Для выхода из многострочного режима введите !end");
			var lines = new List<string>();
			while (true)
			{
				var line = Console.ReadLine();
				if (line == "!end") break;
				lines.Add(line);
			}

			AddTaskToArray(string.Join("\n", lines));
		}
		else
			AddTaskToArray(input.Split(" ", 2)[1]);
	}

	private static void AddTaskToArray(string task)
	{
		if (taskCount == taskList.Length) ExpandArrays();

		taskList[taskCount] = task;
		taskStatuses[taskCount] = false;
		taskDates[taskCount] = DateTime.Now;

		taskCount++;
		Console.WriteLine($"Задача добавлена: {task}");
	}

	private static string[] ParseFlags(string command)
	{
		var flags = new List<string>();
		foreach (var text in command.Split(' '))
			if (text.StartsWith("-"))
				for (var i = 1; i < text.Length; i++)
					flags.Add("-" + text[i]);
			else if (text.StartsWith("--")) flags.Add(text);

		return flags.ToArray();
	}

	private static void Profile()
	{
		Console.WriteLine($"{name} {surname}, {age}");
	}

	private static void Help()
	{
		Console.WriteLine("""
		                  Доступные команды:
		                  help — список команд
		                  profile — выводит данные профиля
		                  add "текст задачи" — добавляет задачу
		                    Флаги: --multiline -m
		                  done - отметить выполненным
		                  delete - удалить задачу
		                  view — просмотр всех задач
		                    Флаги: --index -i, --status -s, --update-date -d, --all -a
		                  exit — завершить программу
		                  read - посмотреть полный текст задачи
		                  """);
	}

	private static void ExpandArrays()
	{
		var newSize = taskList.Length * 2;
		Array.Resize(ref taskList, newSize);
		Array.Resize(ref taskStatuses, newSize);
		Array.Resize(ref taskDates, newSize);
	}
}