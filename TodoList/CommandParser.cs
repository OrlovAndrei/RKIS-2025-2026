namespace TodoList.Commands;

public static class CommandParser
{
	public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
	{
		if (string.IsNullOrWhiteSpace(inputString)) return new UnknownCommand();

		var parts = inputString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length == 0) return new UnknownCommand();

		var commandName = parts[0].ToLower();

		switch (commandName)
		{
			case "add":
				return ParseAddCommand(inputString, todoList);
			case "view":
				return ParseViewCommand(inputString, todoList);
			case "done":
				return ParseDoneCommand(inputString, todoList);
			case "delete":
				return ParseDeleteCommand(inputString, todoList);
			case "update":
				return ParseUpdateCommand(inputString, todoList);
			case "read":
				return ParseReadCommand(inputString, todoList);
			case "profile":
				return ParseProfileCommand(profile);
			case "help":
				return new HelpCommand();
			case "exit":
				return new ExitCommand { TodoList = todoList, UserProfile = profile };
			default:
				return new UnknownCommand();
		}
	}

	private static ICommand ParseAddCommand(string input, TodoList todoList)
	{
		var command = new AddCommand
		{
			TodoList = todoList
		};

		if (input.Contains("--multiline") || input.Contains("-m"))
		{
			command.IsMultiline = true;
			return command;
		}

		var parts = input.Split('"');
		if (parts.Length >= 2) command.Text = parts[1].Trim();

		return command;
	}

	private static ICommand ParseViewCommand(string input, TodoList todoList)
	{
		var command = new ViewCommand { TodoList = todoList };

		var flags = input.Length > 4 ? input.Substring(4).Trim() : "";

		var showAll = flags.Contains("-a") || flags.Contains("--all");
		command.ShowIndex = flags.Contains("--index") || flags.Contains("-i") || showAll;
		command.ShowStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
		command.ShowDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;

		if (flags.Contains("-") && flags.Length > 1 && !flags.Contains("--"))
		{
			var shortFlags = flags.Replace("-", "").Replace(" ", "");
			command.ShowIndex = command.ShowIndex || shortFlags.Contains("i");
			command.ShowStatus = command.ShowStatus || shortFlags.Contains("s");
			command.ShowDate = command.ShowDate || shortFlags.Contains("d");
			if (shortFlags.Contains("a"))
			{
				command.ShowIndex = true;
				command.ShowStatus = true;
				command.ShowDate = true;
			}
		}

		return command;
	}

	private static ICommand ParseDoneCommand(string input, TodoList todoList)
	{
		var command = new DoneCommand
		{
			TodoList = todoList
		};
		var parts = input.Split(' ');

		if (parts.Length >= 2 && int.TryParse(parts[1], out var taskNumber)) command.TaskNumber = taskNumber;

		return command;
	}

	private static ICommand ParseDeleteCommand(string input, TodoList todoList)
	{
		var command = new DeleteCommand
		{
			TodoList = todoList
		};
		var parts = input.Split(' ');

		if (parts.Length >= 2 && int.TryParse(parts[1], out var taskNumber)) command.TaskNumber = taskNumber;

		return command;
	}

	private static ICommand ParseUpdateCommand(string input, TodoList todoList)
	{
		var command = new UpdateCommand
		{
			TodoList = todoList
		};
		var parts = input.Split('"');

		if (parts.Length >= 2)
		{
			command.NewText = parts[1].Trim();

			var indexPart = parts[0].Replace("update", "").Trim();
			if (int.TryParse(indexPart, out var taskNumber)) command.TaskNumber = taskNumber;
		}

		return command;
	}

	private static ICommand ParseReadCommand(string input, TodoList todoList)
	{
		var command = new ReadCommand { TodoList = todoList };
		var parts = input.Split(' ');

		if (parts.Length >= 2 && int.TryParse(parts[1], out var taskNumber)) command.TaskNumber = taskNumber;

		return command;
	}

	private static ICommand ParseProfileCommand(Profile profile)
	{
		var command = new ProfileCommand
		{
			Profile = profile
		};
		return command;
	}
}