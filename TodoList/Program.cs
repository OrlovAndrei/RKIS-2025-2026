namespace TodoList;

class Program
{
	static Profile profile;
	private static TodoList todos = new();

	public static void Main()
	{
		Console.WriteLine("Работу выполнили Кулаков и Рублёв 3833");
		Console.Write("Введите имя: ");
		var name = Console.ReadLine();
		Console.Write("Введите фамилию: ");
		var surname = Console.ReadLine();

		Console.Write("Введите год рождения: ");
		var year = int.Parse(Console.ReadLine());

		profile = new Profile(name, surname, year);
		Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");

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
			else if (command.StartsWith("read"))
				ReadTask(command);
			else if (command.StartsWith("done "))
				DoneTask(command);
			else if (command.StartsWith("delete "))
				DeleteTask(command);
			else if (command.StartsWith("update "))
				UpdateTask(command);
			else if (command == "exit")
			{
				Console.WriteLine("Программа завершена.");
				break;
			}
			else
				Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
		}
	}

	private static void UpdateTask(string input)
	{
		string[] parts = input.Split(' ', 3);
		var taskIndex = int.Parse(parts[1]) - 1;

		var newText = parts[2];
		todos.Update(taskIndex, newText);
	}

	private static void DeleteTask(string input)
	{
		string[] parts = input.Split(' ', 2);
		var taskIndex = int.Parse(parts[1]) - 1;

		todos.Delete(taskIndex);
	}

	private static void DoneTask(string input)
	{
		string[] parts = input.Split(' ', 2);
		var taskIndex = int.Parse(parts[1]) - 1;

		todos.MarkDone(taskIndex);
	}

	private static void ReadTask(string input)
	{
		string[] parts = input.Split(' ', 2);
		var taskIndex = int.Parse(parts[1]) - 1;

		todos.Read(taskIndex);
	}
	private static void ViewTasks(string input)
	{
		var flags = ParseFlags(input);

		bool hasAll = flags.Contains("--all") || flags.Contains("-a");
		bool hasIndex = flags.Contains("--index") || flags.Contains("-i");
		bool hasStatus = flags.Contains("--status") || flags.Contains("-s");
		bool hasDate = flags.Contains("--update-date") || flags.Contains("-d");

		todos.View(hasIndex, hasStatus, hasDate, hasAll);
	}

	private static void AddTask(string input)
	{
		var flags = ParseFlags(input);

		string task = "";
		if (flags.Contains("-m") || flags.Contains("--multi"))
		{
			Console.WriteLine("Для выхода из многострочного режима введите !end");
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
			task = input.Split("add", 2)[1].Trim();
		}

		todos.Add(new TodoItem(task));
	}

	private static string[] ParseFlags(string command)
	{
		List<string> flags = new List<string>();
		foreach (var text in command.Split(' '))
		{
			if (text.StartsWith("-"))
			{
				for (int i = 1; i < text.Length; i++)
				{
					flags.Add("-" + text[i]);
				}
			}
			else if (text.StartsWith("--"))
			{
				flags.Add(text);
			}
		}
		return flags.ToArray();
	}

	private static void Profile()
	{
		Console.WriteLine(profile.GetInfo());
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
}
