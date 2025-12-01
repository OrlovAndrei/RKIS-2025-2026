namespace TodoList;

internal class Program
{
	private const int InitialArraySize = 2;

	private static string[] _todos = new string[InitialArraySize];
	private static bool[] _statuses = new bool[InitialArraySize];
	private static DateTime[] _dates = new DateTime[InitialArraySize];

	private static int _nextTodoIndex;

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

		var age = DateTime.Now.Year - year;
		var text = "Добавлен пользователь " + firstName + " " + lastName + ", возраст - " + age;
		Console.WriteLine(text);

		while (true)
		{
			Console.Write("Введите команду: ");
			var command = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(command)) continue;

			if (command == "help") HelpCommand();
			else if (command == "profile") ShowProfile(firstName, lastName, year);
			else if (command == "exit") break;
			else if (command == "view") ViewTodo("");
			else if (command.StartsWith("view ")) ViewTodo(command.Substring(4).Trim());
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
		Console.WriteLine("help — выводит список всех доступных команд с кратким описанием");
		Console.WriteLine("profile — выводит данные пользователя");
		Console.WriteLine("add \"текст\" — добавляет новую задачу");
		Console.WriteLine("add --multiline (-m) — добавить задачу в многострочном режиме");
		Console.WriteLine("done index — отметить задачу выполненной");
		Console.WriteLine("delete index — удалить задачу");
		Console.WriteLine("update index — изменить текст задачи");
		Console.WriteLine("view — выводит все задачи");
		Console.WriteLine("exit — выход из программы");
	}

	private static void ShowProfile(string firstName, string lastName, int birthYear)
	{
		var age = DateTime.Now.Year - birthYear;
		Console.WriteLine(firstName + " " + lastName + ", " + birthYear + " (возраст: " + age + ")");
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

		if (_nextTodoIndex >= _todos.Length) ExpandArrays();

		_todos[_nextTodoIndex] = todoText;
		_statuses[_nextTodoIndex] = false;
		_dates[_nextTodoIndex] = DateTime.Now;

		Console.WriteLine("Задача добавлена: " + todoText + " (всего задач: " + (_nextTodoIndex + 1) + ")");
		_nextTodoIndex++;
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

		if (_nextTodoIndex >= _todos.Length) ExpandArrays();

		_todos[_nextTodoIndex] = multilineText;
		_statuses[_nextTodoIndex] = false;
		_dates[_nextTodoIndex] = DateTime.Now;

		Console.WriteLine("Многострочная задача добавлена (всего задач: " + (_nextTodoIndex + 1) + ")");
		_nextTodoIndex++;
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
		if (taskIndex < 0 || taskIndex >= _nextTodoIndex)
		{
			Console.WriteLine("Задачи с номером " + taskNumber + " не существует.");
			return;
		}

		_statuses[taskIndex] = true;
		_dates[taskIndex] = DateTime.Now;
		Console.WriteLine("Задача '" + _todos[taskIndex] + "' отмечена как выполненная");
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
		if (taskIndex < 0 || taskIndex >= _nextTodoIndex)
		{
			Console.WriteLine("Задачи с номером " + taskNumber + " не существует.");
			return;
		}

		var newText = parts[1].Trim();
		if (string.IsNullOrWhiteSpace(newText))
		{
			Console.WriteLine("Текст задачи не может быть пустым.");
			return;
		}

		_todos[taskIndex] = newText;
		_dates[taskIndex] = DateTime.Now;
		Console.WriteLine("Задача обновлена");
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
		if (taskIndex < 0 || taskIndex >= _nextTodoIndex)
		{
			Console.WriteLine("Задачи с номером " + taskNumber + " не существует.");
			return;
		}

		for (var i = taskIndex; i < _nextTodoIndex - 1; i++)
		{
			_todos[i] = _todos[i + 1];
			_statuses[i] = _statuses[i + 1];
			_dates[i] = _dates[i + 1];
		}

		_nextTodoIndex--;
		Console.WriteLine("Задача удалена");
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
		if (taskIndex < 0 || taskIndex >= _nextTodoIndex)
		{
			Console.WriteLine("Задачи с номером " + taskNumber + " не существует.");
			return;
		}

		Console.WriteLine("=== Задача #" + taskNumber + " ===");
		Console.WriteLine("Текст: " + _todos[taskIndex]);
		Console.WriteLine("Статус: " + (_statuses[taskIndex] ? "Выполнена" : "Не выполнена"));
		Console.WriteLine("Дата изменения: " + _dates[taskIndex].ToString("dd.MM.yyyy HH:mm"));
	}

	private static void ViewTodo(string flags)
	{
		if (_nextTodoIndex == 0)
		{
			Console.WriteLine("Задач нет.");
			return;
		}

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

		if (!showIndex && !showStatus && !showDate)
		{
			Console.WriteLine("Список задач:");
			for (var i = 0; i < _nextTodoIndex; i++)
			{
				var shortText = GetShortText(_todos[i], 30);
				Console.WriteLine(i + 1 + ". " + shortText);
			}

			return;
		}

		Console.WriteLine("Список задач:");

		string header = "";
		if (showIndex) header += "№    ";
		header += "Текст задачи                   ";
		if (showStatus) header += "Статус      ";
		if (showDate) header += "Дата изменения    ";
    
		Console.WriteLine(header);
		Console.WriteLine(new string('-', header.Length));
    
		for (int i = 0; i < _nextTodoIndex; i++)
		{
			string line = "";
        
			if (showIndex)
				line += $"{i + 1,-4} ";
            
			string shortText = GetShortText(_todos[i], 30);
			line += $"{shortText,-30}";
			
			if (showStatus)
			{
				string status = _statuses[i] ? "Сделано" : "Не сделано";
				line += $" {status,-10} ";
			}
        
			if (showDate)
			{
				string date = _dates[i].ToString("dd.MM.yyyy HH:mm");
				line += $" {date}";
			}
        
			Console.WriteLine(line);
		}
	}

	private static string GetShortText(string text, int maxLength)
	{
		if (string.IsNullOrEmpty(text)) return "";
		if (text.Length <= maxLength) return text;
		return text.Substring(0, maxLength - 3) + "...";
	}

	private static void ExpandArrays()
	{
		var newSize = _todos.Length * 2;
		Array.Resize(ref _todos, newSize);
		Array.Resize(ref _statuses, newSize);
		Array.Resize(ref _dates, newSize);
	}
}