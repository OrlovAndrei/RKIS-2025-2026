namespace TodoList;

internal class Program
{
	private static readonly TodoList _todoList = new();
	private static Profile _userProfile;

	public static void Main()
	{
		Console.WriteLine("Работу  выполнили Лютов и Легатов 3832");
		Console.Write("Введите ваше имя: ");
		var firstName = Console.ReadLine();
		Console.Write("Введите вашу фамилию: ");
		var lastName = Console.ReadLine();

		Console.Write("Введите ваш год рождения: ");
		var yearInput = Console.ReadLine();
		int year;
		if (!int.TryParse(yearInput, out year))
		{
			Console.WriteLine("Неверный формат года. Установлен 2000 год по умолчанию.");
			year = 2000;
		}

		_userProfile = new Profile(firstName, lastName, year);

		var text = "Добавлен пользователь " + _userProfile.GetInfo();
		Console.WriteLine(text);

		while (true)
		{
			Console.Write("Введите команду: ");
			var command = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(command)) continue;

			if (command == "help") HelpCommand();
			else if (command == "profile") ShowProfile();
			else if (command == "exit") break;
			else if (command.StartsWith("view")) ViewTodo(command.Substring(4).Trim());
			else if (command.StartsWith("add")) AddTodo(command);
			else if (command.StartsWith("done ")) DoneTodo(command);
			else if (command.StartsWith("update")) UpdateTodo(command);
			else if (command.StartsWith("delete ")) DeleteTodo(command);
			else if (command.StartsWith("read ")) ReadTodo(command);
			else Console.WriteLine("Неизвестная команда.");
		}
	}

	private static void HelpCommand()
	{
		Console.WriteLine("СПРАВКА ПО КОМАНДАМ:");
		Console.WriteLine("help                    - вывести список команд");
		Console.WriteLine("profile                 - показать данные пользователя");
		Console.WriteLine("add \"текст\"            - добавить задачу");
		Console.WriteLine("add --multiline (-m)    - добавить задачу в многострочном режиме");
		Console.WriteLine("view                    - показать только текст задач");
		Console.WriteLine("view --index (-i)       - показать с индексами");
		Console.WriteLine("view --status (-s)      - показать со статусами");
		Console.WriteLine("view --update-date (-d) - показать с датами");
		Console.WriteLine("view --all (-a)         - показать всю информацию");
		Console.WriteLine("read <номер>            - просмотреть полный текст задачи");
		Console.WriteLine("done <номер>            - отметить задачу выполненной");
		Console.WriteLine("delete <номер>          - удалить задачу");
		Console.WriteLine("update <номер> \"текст\" - обновить текст задачи");
		Console.WriteLine("exit                    - выйти из программы");
	}

	private static void ShowProfile()
	{
		Console.WriteLine(_userProfile.GetInfo());
	}

	private static void AddTodo(string command)
	{
		if (string.IsNullOrWhiteSpace(command))
		{
			Console.WriteLine("Команда не может быть пустой.");
			return;
		}

		if (command.Contains("--multiline") || command.Contains("-m"))
		{
			AddTodoMultiline();
			return;
		}

		var parts = command.Split('"');
		if (parts.Length < 2)
		{
			Console.WriteLine("Неверный формат. Используйте: add \"текст задачи\"");
			return;
		}

		var todoText = parts[1].Trim();
		if (string.IsNullOrWhiteSpace(todoText))
		{
			Console.WriteLine("Текст задачи не может быть пустым.");
			return;
		}

		var newItem = new TodoItem(todoText);
		_todoList.Add(newItem);

		Console.WriteLine("Задача добавлена: " + todoText + " (всего задач: " + _todoList.Count + ")");
	}

	private static void AddTodoMultiline()
	{
		Console.WriteLine("Введите текст задачи (для завершения введите !end):");

		var multilineText = "";
		while (true)
		{
			Console.Write("> ");
			var line = Console.ReadLine();

			if (line == null) continue;
			if (line == "!end") break;

			if (!string.IsNullOrEmpty(multilineText)) multilineText += "\n";

			multilineText += line;
		}

		if (string.IsNullOrWhiteSpace(multilineText))
		{
			Console.WriteLine("Текст задачи не может быть пустым.");
			return;
		}

		var newItem = new TodoItem(multilineText);
		_todoList.Add(newItem);

		Console.WriteLine("Многострочная задача добавлена (всего задач: " + _todoList.Count + ")");
	}

	private static void DoneTodo(string command)
	{
		var parts = command.Split(' ');
		if (parts.Length < 2 || !int.TryParse(parts[1], out var taskNumber))
		{
			Console.WriteLine("Неверный формат. Используйте: done <номер_задачи>");
			return;
		}

		var taskIndex = taskNumber - 1;
		try
		{
			var item = _todoList.GetItem(taskIndex);
			item.MarkDone();
			Console.WriteLine($"Задача '{item.Text}' отмечена как выполненная");
		}
		catch (ArgumentOutOfRangeException)
		{
			Console.WriteLine($"Задачи с номером {taskNumber} не существует.");
		}
	}

	private static void DeleteTodo(string command)
	{
		var parts = command.Split(' ');
		if (parts.Length < 2 || !int.TryParse(parts[1], out var taskNumber))
		{
			Console.WriteLine("Неверный формат. Используйте: delete <номер_задачи>");
			return;
		}

		var taskIndex = taskNumber - 1;
		try
		{
			_todoList.Delete(taskIndex);
			Console.WriteLine("Задача удалена");
		}
		catch (ArgumentOutOfRangeException)
		{
			Console.WriteLine($"Задачи с номером {taskNumber} не существует.");
		}
	}

	private static void UpdateTodo(string command)
	{
		if (string.IsNullOrWhiteSpace(command))
		{
			Console.WriteLine("Команда не может быть пустой.");
			return;
		}

		var parts = command.Split('"');
		if (parts.Length < 2)
		{
			Console.WriteLine("Неверный формат. Используйте: update <номер> \"новый текст\"");
			return;
		}

		var indexPart = parts[0].Replace("update", "").Trim();
		if (!int.TryParse(indexPart, out var taskNumber))
		{
			Console.WriteLine("Неверный номер задачи.");
			return;
		}

		var taskIndex = taskNumber - 1;
		var newText = parts[1].Trim();
		if (string.IsNullOrWhiteSpace(newText))
		{
			Console.WriteLine("Текст задачи не может быть пустым.");
			return;
		}

		try
		{
			var item = _todoList.GetItem(taskIndex);
			item.UpdateText(newText);
			Console.WriteLine("Задача обновлена");
		}
		catch (ArgumentOutOfRangeException)
		{
			Console.WriteLine($"Задачи с номером {taskNumber} не существует.");
		}
	}

	private static void ReadTodo(string command)
	{
		var parts = command.Split(' ');
		if (parts.Length < 2 || !int.TryParse(parts[1], out var taskNumber))
		{
			Console.WriteLine("Неверный формат. Используйте: read <номер_задачи>");
			return;
		}

		var taskIndex = taskNumber - 1;
		try
		{
			var item = _todoList.GetItem(taskIndex);
			Console.WriteLine($"=== Задача #{taskNumber} ===");
			Console.WriteLine(item.GetFullInfo());
		}
		catch (ArgumentOutOfRangeException)
		{
			Console.WriteLine($"Задачи с номером {taskNumber} не существует.");
		}
	}

	private static void ViewTodo(string flags)
	{
		var showAll = flags.Contains("-a") || flags.Contains("--all");
		var showIndex = flags.Contains("--index") || flags.Contains("-i") || showAll;
		var showStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
		var showDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;

		if (flags.Contains("-") && flags.Length > 1 && !flags.Contains("--"))
		{
			var shortFlags = flags.Replace("-", "").Replace(" ", "");
			showIndex = showIndex || shortFlags.Contains("i");
			showStatus = showStatus || shortFlags.Contains("s");
			showDate = showDate || shortFlags.Contains("d");
			if (shortFlags.Contains("a"))
			{
				showIndex = true;
				showStatus = true;
				showDate = true;
			}
		}

		_todoList.View(showIndex, showStatus, showDate);
	}
}