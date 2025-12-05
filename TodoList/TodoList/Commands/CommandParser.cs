public static class CommandParser
{
	public static ICommand Parse(string inputString, TodoList todoList, Profile profile, string profilesFilePath, string dataDir)
	{
		if (string.IsNullOrWhiteSpace(inputString))
			return null;

		string[] parts = inputString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		string command = parts[0].ToLower();
		switch (command)
		{
			case "help":
				return new HelpCommand();
			case "profile":
				return new ProfileCommand {UserProfile = profile};
			case "add":
				return ParseAddCommand(inputString, todoList);
			case "view":
				return ParseViewCommand(inputString, todoList);
			case "status":
				return ParseStatusCommand(inputString, todoList);
			case "delete":
				return ParseDeleteCommand(inputString, todoList);
			case "update":
				return ParseUpdateCommand(inputString, todoList);
			case "out":
				return ParseReadCommand(inputString, todoList);
			case "undo":
				return new UndoCommand();
			case "redo":
				return new RedoCommand();
			default:
				throw new ArgumentException($"Неизвестная команда: {command}");
		}
	}
	private static ICommand ParseAddCommand(string input, TodoList todoList)
	{
		var command = new AddCommand { Todos = todoList };
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
	private static ICommand ParseViewCommand(string input, TodoList todoList)
	{
		var command = new ViewCommand { Todos = todoList };
		command.AllOutput = input.Contains("--all");
		command.ShowIndex = input.Contains("--index");
		command.ShowStatus = input.Contains("--status");
		command.ShowDate = input.Contains("--update-date");
		if (!input.Contains("--"))
		{
			int dashIndex = input.IndexOf("-");
			if (dashIndex >= 0)
			{
				for (int i = dashIndex + 1; i < input.Length; i++)
				{
					char flag = input[i];
					switch (flag)
					{
						case 'a': command.AllOutput = true; break;
						case 'i': command.ShowIndex = true; break;
						case 's': command.ShowStatus = true; break;
						case 'd': command.ShowDate = true; break;
					}
				}
			}
		}
		return command;
	}
	private static ICommand ParseDeleteCommand(string input, TodoList todoList)
	{
		string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length < 2 || !int.TryParse(parts[1], out int taskId))
		{
			throw new ArgumentException("Неверный формат команды delete. Используйте: delete индекс");
		}
		return new DeleteCommand { Todos = todoList, TaskIndex = taskId };
	}
	private static ICommand ParseUpdateCommand(string input, TodoList todoList)
	{
		string[] textParts = input.Split('\"');
		if (textParts.Length < 2)
		{
			throw new ArgumentException("Неверный формат команды update. Используйте: update индекс \"новый текст\"");
		}
		string[] idParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (idParts.Length < 2 || !int.TryParse(idParts[1], out int taskId))
		{
			throw new ArgumentException("Неверный индекс задачи");
		}
		return new UpdateCommand
		{
			Todos = todoList,
			TaskIndex = taskId,
			NewText = textParts[1]
		};
	}
	private static ICommand ParseReadCommand(string input, TodoList todoList)
	{
		string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length < 2 || !int.TryParse(parts[1], out int taskId))
		{
			throw new ArgumentException("Неверный формат команды read. Используйте: read индекс");
		}
		return new ReadCommand { Todos = todoList, TaskIndex = taskId };
	}
	private static ICommand ParseStatusCommand(string input, TodoList todoList)
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
			NewStatus = status
		};
	}
}
