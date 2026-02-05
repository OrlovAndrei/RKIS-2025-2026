public static class CommandParser
{
	private delegate ICommand CommandHandler(string args, TodoList todoList, Profile profile, string dataDir);
	private static readonly Dictionary<string, CommandHandler> _commandHandlers;
	static CommandParser()
	{
		_commandHandlers["search"] = ParseSearch;
		_commandHandlers = new Dictionary<string, CommandHandler>();
		_commandHandlers["help"] = ParseHelp;
		_commandHandlers["profile"] = ParseProfile;
		_commandHandlers["out"] = ParseLogout;
		_commandHandlers["read"] = ParseRead;
		_commandHandlers["add"] = ParseAdd;
		_commandHandlers["view"] = ParseView;
		_commandHandlers["status"] = ParseStatus;
		_commandHandlers["delete"] = ParseDelete;
		_commandHandlers["update"] = ParseUpdate;
		_commandHandlers["undo"] = ParseUndo;
		_commandHandlers["redo"] = ParseRedo;
	}
	public static ICommand Parse(string inputString, TodoList todoList, Profile profile, string profilesFilePath, string dataDir)
	{
		if (string.IsNullOrWhiteSpace(inputString))
			return null;
		var parts = inputString.Trim().Split(' ', 2);
		var commandKey = parts[0].ToLower();
		var args = parts.Length > 1 ? parts[1] : "";
		if (_commandHandlers.TryGetValue(commandKey, out var handler))
		{
			return handler(args, todoList, profile, dataDir);
		}
		throw new ArgumentException($"Неизвестная команда: {commandKey}");
	}
	private static ICommand ParseHelp(string args, TodoList todoList, Profile profile, string dataDir)
	{
		return new HelpCommand();
	}
	private static ICommand ParseUndo(string args, TodoList todoList, Profile profile, string dataDir)
	{
		return new UndoCommand();
	}
	private static ICommand ParseRedo(string args, TodoList todoList, Profile profile, string dataDir)
	{
		return new RedoCommand();
	}
	private static ICommand ParseAdd(string args, TodoList todoList, Profile profile, string dataDir)
	{
		var command = new AddCommand { Todos = todoList, UserId = profile.Id, DataDir = dataDir };
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
	private static ICommand ParseSearch(string args, TodoList todoList, Profile profile, string dataDir)
	{
		var command = new SearchCommand { Todos = todoList };
		var regex = new System.Text.RegularExpressions.Regex(@"(--\w+)\s+(""[^""]*""|\S+)");
		var matches = regex.Matches(args);

		foreach (System.Text.RegularExpressions.Match match in matches)
		{
			string flag = match.Groups[1].Value.ToLower();
			string value = match.Groups[2].Value.Trim('"');

			switch (flag)
			{
				case "--contains": command.Contains = value; break;
				case "--starts-with": command.StartsWith = value; break;
				case "--ends-with": command.EndsWith = value; break;
				case "--from": if (DateTime.TryParse(value, out var f)) command.FromDate = f; break;
				case "--to": if (DateTime.TryParse(value, out var t)) command.ToDate = t; break;
				case "--status": if (Enum.TryParse<TodoStatus>(value, true, out var s)) command.Status = s; break;
				case "--sort": command.SortBy = value.ToLower(); break;
				case "--top": if (int.TryParse(value, out var n)) command.Top = n; break;
			}
		}
		if (args.Contains("--desc")) command.Descending = true;
		return command;
	}
	private static ICommand ParseView(string args, TodoList todoList, Profile profile, string dataDir)
	{
		var command = new ViewCommand { Todos = todoList };
		command.AllOutput = args.Contains("--all") || args.Contains("-a");
		command.ShowIndex = args.Contains("--index") || args.Contains("-i");
		command.ShowStatus = args.Contains("--status") || args.Contains("-s");
		command.ShowDate = args.Contains("--update-date") || args.Contains("-d");
		return command;
	}
	private static ICommand ParseDelete(string args, TodoList todoList, Profile profile, string dataDir)
	{
		if (string.IsNullOrWhiteSpace(args) || !int.TryParse(args.Trim(), out int taskId))
		{
			throw new ArgumentException("Неверный формат команды delete. Используйте: delete индекс");
		}
		return new DeleteCommand { Todos = todoList, TaskIndex = taskId, UserId = profile.Id, DataDir = dataDir };
	}
	private static ICommand ParseUpdate(string args, TodoList todoList, Profile profile, string dataDir)
	{
		string[] textParts = args.Split('\"');
		if (textParts.Length < 2)
		{
			throw new ArgumentException("Неверный формат команды update. Используйте: update индекс \"новый текст\"");
		}
		string[] idParts = textParts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (idParts.Length < 1 || !int.TryParse(idParts[0], out int taskId))
		{
			throw new ArgumentException("Неверный индекс задачи");
		}
		return new UpdateCommand
		{
			Todos = todoList,
			TaskIndex = taskId,
			NewText = textParts[1],
			UserId = profile.Id,
			DataDir = dataDir
		};
	}
	private static ICommand ParseRead(string args, TodoList todoList, Profile profile, string dataDir)
	{
		if (string.IsNullOrWhiteSpace(args) || !int.TryParse(args.Trim(), out int taskId))
		{
			throw new ArgumentException("Неверный формат команды read. Используйте: read индекс");
		}
		return new ReadCommand { Todos = todoList, TaskIndex = taskId, UserId = profile.Id };
	}
	private static ICommand ParseStatus(string args, TodoList todoList, Profile profile, string dataDir)
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
		if (!System.Enum.TryParse<TodoStatus>(parts[1], true, out TodoStatus status))
		{
			throw new ArgumentException($"Неверный статус. Допустимые значения: {string.Join(", ", System.Enum.GetNames(typeof(TodoStatus)))}");
		}
		return new StatusCommand
		{
			Todos = todoList,
			TaskIndex = taskId,
			NewStatus = status,
			UserId = profile.Id,
			DataDir = dataDir
		};
	}
	private static ICommand ParseProfile(string args, TodoList todoList, Profile profile, string dataDir)
	{
		return new ProfileCommand { UserProfile = profile, LogoutFlag = false };
	}

	private static ICommand ParseLogout(string args, TodoList todoList, Profile profile, string dataDir)
	{
		return new ProfileCommand { UserProfile = profile, LogoutFlag = true };
	}
}