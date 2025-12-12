public static class CommandParser
{
	private delegate ICommand CommandHandler(string input, TodoList todoList, Profile profile, string dataDir);
	private static readonly Dictionary<string, CommandHandler> _commandHandlers;
	static CommandParser()
	{
		_commandHandlers = new Dictionary<string, CommandHandler>();
		_commandHandlers["help"] = ParseHelp;
		_commandHandlers["profile"] = ParseProfile;
		_commandHandlers["out"] = ParseProfile;
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
		string[] parts = inputString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		string commandKey = parts[0].ToLower();
		if (_commandHandlers.TryGetValue(commandKey, out var handler))
		{
			return handler(inputString, todoList, profile, dataDir);
		}
		throw new ArgumentException($"Неизвестная команда: {commandKey}");
	}
	private static ICommand ParseHelp(string input, TodoList todoList, Profile profile, string dataDir)
	{
		return new HelpCommand();
	}
	private static ICommand ParseUndo(string input, TodoList todoList, Profile profile, string dataDir)
	{
		return new UndoCommand();
	}
	private static ICommand ParseRedo(string input, TodoList todoList, Profile profile, string dataDir)
	{
		return new RedoCommand();
	}
	private static ICommand ParseAdd(string input, TodoList todoList, Profile profile, string dataDir)
	{
		var command = new AddCommand { Todos = todoList, UserId = profile.Id, DataDir = dataDir };
		if (input.Contains("-m") || input.Contains("--multiline"))
		{
			command.Multiline = true;
		}
		else
		{
			string[] textParts = input.Split('\"');
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
	private static ICommand ParseView(string input, TodoList todoList, Profile profile, string dataDir)
	{
		var command = new ViewCommand { Todos = todoList };
		command.AllOutput = input.Contains("--all");
		command.ShowIndex = input.Contains("--index");
		command.ShowStatus = input.Contains("--status");
		command.ShowDate = input.Contains("--update-date");
		if (!input.Contains("--"))
		{
			int dashIndex = input.IndexOf('-');
			if (dashIndex >= 0)
			{
				string flags = input.Substring(dashIndex + 1);
				if (!flags.Contains(" "))
				{
					if (flags.Contains('a')) command.AllOutput = true;
					if (flags.Contains('i')) command.ShowIndex = true;
					if (flags.Contains('s')) command.ShowStatus = true;
					if (flags.Contains('d')) command.ShowDate = true;
				}
			}
		}
		return command;
	}
	private static ICommand ParseDelete(string input, TodoList todoList, Profile profile, string dataDir)
	{
		string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length < 2 || !int.TryParse(parts[1], out int taskId))
		{
			throw new ArgumentException("Неверный формат команды delete. Используйте: delete индекс");
		}
		return new DeleteCommand { Todos = todoList, TaskIndex = taskId, UserId = profile.Id, DataDir = dataDir };
	}
	private static ICommand ParseUpdate(string input, TodoList todoList, Profile profile, string dataDir)
	{
		string[] textParts = input.Split('\"');
		if (textParts.Length < 2)
		{
			throw new ArgumentException("Неверный формат команды update. Используйте: update индекс \"новый текст\"");
		}
		string[] idParts = input.Split(new[] { ' ', '\"' }, StringSplitOptions.RemoveEmptyEntries);
		if (idParts.Length < 2 || !int.TryParse(idParts[1], out int taskId))
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
	private static ICommand ParseRead(string input, TodoList todoList, Profile profile, string dataDir)
	{
		string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length < 2 || !int.TryParse(parts[1], out int taskId))
		{
			throw new ArgumentException("Неверный формат команды read. Используйте: read индекс");
		}
		return new ReadCommand { Todos = todoList, TaskIndex = taskId, UserId = profile.Id };
	}
	private static ICommand ParseStatus(string input, TodoList todoList, Profile profile, string dataDir)
	{
		string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length < 3)
		{
			throw new ArgumentException("Неверный формат команды status. Используйте: status индекс статус");
		}
		if (!int.TryParse(parts[1], out int taskId))
		{
			throw new ArgumentException("Неверный индекс задачи");
		}
		if (!System.Enum.TryParse<TodoStatus>(parts[2], true, out TodoStatus status))
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
	private static ICommand ParseProfile(string input, TodoList todoList, Profile profile, string dataDir)
	{
		var command = new ProfileCommand { UserProfile = profile };
		string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		string commandKey = parts[0].ToLower();
		command.LogoutFlag = commandKey == "out";
		return command;
	}
}