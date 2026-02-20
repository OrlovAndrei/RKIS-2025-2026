using TodoApp.Exceptions;
public static class CommandParser
{
	private static TodoList _todoList;
	private static Profile _profile;
	private static string _dataDir;
	private delegate ICommand CommandHandler(string args);
	private static readonly Dictionary<string, CommandHandler> _commandHandlers;
	static CommandParser()
	{
		_commandHandlers = new Dictionary<string, CommandHandler>
		{
			["help"] = ParseHelp,
			["profile"] = ParseProfile,
			["out"] = ParseLogout,
			["read"] = ParseRead,
			["add"] = ParseAdd,
			["view"] = ParseView,
			["status"] = ParseStatus,
			["delete"] = ParseDelete,
			["update"] = ParseUpdate,
			["undo"] = ParseUndo,
			["redo"] = ParseRedo,
			["search"] = ParseSearch
		};
	}
	public static ICommand Parse(string inputString, TodoList todoList, Profile profile, string profilesFilePath, string dataDir)
	{
		if (string.IsNullOrWhiteSpace(inputString)) return null;
		_todoList = todoList;
		_profile = profile;
		_dataDir = dataDir;
		var parts = inputString.Trim().Split(' ', 2);
		var commandKey = parts[0].ToLower();
		var args = parts.Length > 1 ? parts[1] : "";
		if (_commandHandlers.TryGetValue(commandKey, out var handler))
		{
			return handler(args);
		}
		throw new ArgumentException($"Неизвестная команда: {commandKey}");
	}
	private static ICommand ParseHelp(string args) => new HelpCommand();
	private static ICommand ParseUndo(string args) => new UndoCommand();
	private static ICommand ParseRedo(string args) => new RedoCommand();
	private static ICommand ParseAdd(string args)
	{
		var command = new AddCommand { Todos = _todoList, UserId = _profile.Id, DataDir = _dataDir };
		if (args.Contains("-m") || args.Contains("--multiline"))
		{
			command.Multiline = true;
		}
		else
		{
			string[] textParts = args.Split('\"');
			if (textParts.Length >= 2)
			{
				command.TaskText = textParts[1];
			}
			else
			{
				throw new ArgumentException("Неверный формат команды add. Используйте: add \"текст задачи\"");
			}
		}
		return command;
	}
	private static ICommand ParseSearch(string args)
	{
		var command = new SearchCommand { Todos = _todoList };
		if (string.IsNullOrWhiteSpace(args)) return command;
		var regex = new System.Text.RegularExpressions.Regex(@"(--\w+)\s+(""[^""]*""|\S+)");
		var matches = regex.Matches(args);

		foreach (System.Text.RegularExpressions.Match match in matches)
		{
			string flag = match.Groups[1].Value.ToLower();
			string value = match.Groups[2].Value.Trim('"');

			switch (flag)
			{
				case "--contains":
					command.Contains = value;
					break;
				case "--starts-with":
					command.StartsWith = value;
					break;
				case "--ends-with":
					command.EndsWith = value;
					break;
				case "--from":
					if (DateTime.TryParse(value, out var f)) command.FromDate = f;
					else Console.WriteLine($"Ошибка: '{value}' не является корректной датой для --from.");
					break;
				case "--to":
					if (DateTime.TryParse(value, out var t)) command.ToDate = t;
					else Console.WriteLine($"Ошибка: '{value}' не является корректной датой для --to.");
					break;
				case "--status":
					if (Enum.TryParse<TodoStatus>(value, true, out var s)) command.Status = s;
					else Console.WriteLine($"Ошибка: Статус '{value}' не найден.");
					break;
				case "--sort":
					command.SortBy = value.ToLower();
					break;
				case "--top":
					if (int.TryParse(value, out var n) && n > 0) command.Top = n;
					else Console.WriteLine("Ошибка: --top должен быть положительным числом.");
					break;
			}
		}
		if (args.Contains("--desc")) command.Descending = true;
		return command;
	}
	private static ICommand ParseView(string args)
	{
		var command = new ViewCommand { Todos = _todoList };
		command.AllOutput = args.Contains("--all") || args.Contains("-a");
		command.ShowIndex = args.Contains("--index") || args.Contains("-i");
		command.ShowStatus = args.Contains("--status") || args.Contains("-s");
		command.ShowDate = args.Contains("--update-date") || args.Contains("-d");
		return command;
	}
	private static ICommand ParseDelete(string args)
	{
		if (string.IsNullOrWhiteSpace(args) || !int.TryParse(args.Trim(), out int taskId))
		{
			Console.WriteLine("Ошибка: Индекс задачи должен быть целым числом.");
			return null;
		}
		return new DeleteCommand { Todos = _todoList, TaskIndex = taskId, UserId = _profile.Id, DataDir = _dataDir };
	}
	private static ICommand ParseUpdate(string args)
	{
		string[] textParts = args.Split('\"');
		if (textParts.Length < 2)
		{
			Console.WriteLine("Ошибка: Используйте формат: update [индекс] \"новый текст\"");
			return null;
		}
		if (!int.TryParse(textParts[0].Trim(), out int taskId))
		{
			Console.WriteLine("Ошибка: Некорректный индекс задачи.");
			return null;
		}
		return new UpdateCommand { Todos = _todoList, TaskIndex = taskId, NewText = textParts[1], UserId = _profile.Id, DataDir = _dataDir };
	}
	private static ICommand ParseRead(string args)
	{
		if (string.IsNullOrWhiteSpace(args) || !int.TryParse(args.Trim(), out int taskId))
		{
			throw new ArgumentException("Неверный формат команды read. Используйте: read индекс");
		}
		return new ReadCommand { Todos = _todoList, TaskIndex = taskId, UserId = _profile.Id };
	}
	private static ICommand ParseStatus(string args)
	{
		string[] parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length < 2)
		{
			throw new ArgumentException("Неверный формат команды status. Используйте: status индекс статус");
		}
		if (!int.TryParse(parts[0], out int taskId))
		{
			throw new ArgumentException("Неверный индекс задачи");
		}
		if (!Enum.TryParse<TodoStatus>(parts[1], true, out TodoStatus status))
		{
			throw new ArgumentException($"Неверный статус. Допустимые значения: {string.Join(", ", Enum.GetNames(typeof(TodoStatus)))}");
		}
		return new StatusCommand
		{
			Todos = _todoList,
			TaskIndex = taskId,
			NewStatus = status,
			UserId = _profile.Id,
			DataDir = _dataDir
		};
	}
	private static ICommand ParseProfile(string args)
	{
		return new ProfileCommand { UserProfile = _profile, LogoutFlag = false };
	}
	private static ICommand ParseLogout(string args)
	{
		return new ProfileCommand { UserProfile = _profile, LogoutFlag = true };
	}
}