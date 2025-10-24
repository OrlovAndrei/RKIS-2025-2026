namespace TodoList;

internal class Program
{
	static Profile profile;
	private static TodoList todos = new();

	public static void Main()
	{
		Console.WriteLine("Выполнили курсанты группы 3833 Кучеренко и Мурза");
		profile = AddUser();

		while (true)
		{
			Console.Write("Введите команду: ");
			var command = Console.ReadLine();

			if (command == "help") HelpCommand();
			else if (command == "profile") ShowProfile();
			else if (command == "exit") break;
			else if (command.StartsWith("add ")) AddTodo(command);
			else if (command.StartsWith("done ")) DoneTodo(command);
			else if (command.StartsWith("delete ")) DeleteTodo(command);
			else if (command.StartsWith("update ")) UpdateTodo(command);
			else if (command.StartsWith("view")) ViewTodo(command);
			else if (command.StartsWith("read ")) ReadTodo(command);
			else Console.WriteLine("Неизвестная команда.");
		}
	}

	private static Profile AddUser()
	{
		Console.Write("Введите ваше имя: ");
		var firstName = Console.ReadLine();
		Console.Write("Введите вашу фамилию: ");
		var lastName = Console.ReadLine();

		Console.Write("Введите ваш год рождения: ");
		var year = int.Parse(Console.ReadLine());

		var profile = new Profile(firstName, lastName, year);
		Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");
		return profile;
	}

	private static void HelpCommand()
	{
		Console.WriteLine(
			"Команды:\n" +
			"help — выводит список всех доступных команд с кратким описанием\n" +
			"profile — выводит данные пользователя\n" +
			"add \"текст задачи\" — добавляет новую задачу\n" +
			"    -m, --multi — добавить многострочную задачу\n" +
			"view — выводит все задачи\n" +
			"    -a, --all — показать все поля\n" +
			"    -i, --index — показать индекс\n" +
			"    -s, --status — показать статус\n" +
			"    -d, --update-date — показать дату\n" +
			"read \"idx\" — вывод задачи без редактирования\n" +
			"done \"idx\" — отметить задачу выполненной\n" +
			"delete \"idx\" — удалить задачу\n" +
			"update \"idx\" — изменить текст задачи\n" +
			"exit — выход из программы"
		);
	}

	private static void ShowProfile()
	{
		Console.WriteLine(profile.GetInfo());
	}

	private static void AddTodo(string command)
	{
		var flags = ParseFlags(command);
		bool isMulti = flags.Contains("--multi") || flags.Contains("-m");

		string task = "";
		if (isMulti)
		{
			Console.WriteLine("Введите строки задачи (введите !exit для завершения):");

			while (true)
			{
				Console.Write("> ");
				var line = Console.ReadLine();
				if (line == "!exit") break;
				task = line + "\n";
			}
		}
		else
		{
			task = command.Split("add", 2)[1].Trim();
		}

		todos.Add(new TodoItem(task));
	}
	private static string[] ParseFlags(string command)
	{
		return command.Split(' ')
		.Where(p => p.StartsWith("--") || p.StartsWith("-"))
		.SelectMany(p =>
		{
			if (p.StartsWith("--"))
				return new[] { p.Trim() };

			return p.Skip(1)
					.Select(ch => "-" + ch);
		})
		.ToArray();
	}

	private static void DoneTodo(string command)
	{
		var parts = command.Split(' ', 2);
		var index = int.Parse(parts[1]);
		todos.MarkDone(index);
	}

	private static void DeleteTodo(string command)
	{
		var parts = command.Split(' ', 2);
		var index = int.Parse(parts[1]);
		todos.Delete(index);
	}

	private static void UpdateTodo(string command)
	{
		var parts = command.Split(' ', 3);
		var index = int.Parse(parts[1]);
		var task = parts[2];

		todos.Update(index, task);
	}

	private static void ViewTodo(string command)
	{
		var flags = ParseFlags(command);

		bool showAll = flags.Contains("--all") || flags.Contains("-a");
		bool showIndex = flags.Contains("--index") || flags.Contains("-i") || showAll;
		bool showStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
		bool showDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;

		todos.View(showIndex, showStatus, showDate);
	}

	private static void ReadTodo(string command)
	{
		var parts = command.Split(' ', 2);
		int index = int.Parse(parts[1]);

		todos.Read(index);
	}
}