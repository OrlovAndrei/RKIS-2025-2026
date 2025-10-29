
namespace TodoList1;

public static class CommandParser
{
	public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
	{
		if (string.IsNullOrWhiteSpace(inputString))
		{
			return new HelpCommand { todoList = todoList, Profile = profile };
		}

		string[] parts = inputString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length == 0)
		{
			return new HelpCommand { todoList = todoList, Profile = profile };
		}

		string commandName = parts[0].ToLower();

		switch (commandName)
		{
			case "exit":
				return new ExitCommand { todoList = todoList, Profile = profile };

			case "help":
				return new HelpCommand { todoList = todoList, Profile = profile };

			case "profile":
				return new ProfileCommand { todoList = todoList, Profile = profile };

			case "add":
				return ParseAddCommand(inputString, todoList, profile);

			case "view":
				return ParseViewCommand(inputString, todoList, profile);

			case "read":
				return ParseReadCommand(inputString, todoList, profile);

			case "done":
				return ParseDoneCommand(inputString, todoList, profile);

			case "delete":
				return ParseDeleteCommand(inputString, todoList, profile);

			case "update":
				return ParseUpdateCommand(inputString, todoList, profile);

			default:
				Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
				return new HelpCommand { todoList = todoList, Profile = profile };
		}
	}

	private static ICommand ParseAddCommand(string input, TodoList todoList, Profile profile)
	{
		var command = new AddCommand { todoList = todoList, Profile = profile };

		if (input.Contains("--multiline") || input.Contains("-m"))
		{
			command.Multiline = true;
		}
		else
		{
			int startIndex = input.IndexOf("add") + 3;
			if (startIndex < input.Length)
			{
				command.TaskText = input.Substring(startIndex).Trim(' ', '"');
			}
		}

		return command;
	}

	private static ICommand ParseViewCommand(string input, TodoList todoList, Profile profile)
	{
		var command = new ViewCommand { todoList = todoList, Profile = profile };

		command.ShowIndex = input.Contains("--index") || input.Contains("-i");
		command.ShowStatus = input.Contains("--status") || input.Contains("-s");
		command.ShowDate = input.Contains("--date") || input.Contains("-d");
		command.ShowAll = input.Contains("--all") || input.Contains("-a");

		if (!command.ShowIndex && !command.ShowStatus && !command.ShowDate && !command.ShowAll)
		{
			Console.WriteLine("Список задач:");
			for (int i = 0; i < todoList._count; i++)
			{
				var item = todoList.GetItem(i);
				if (item != null)
				{
					Console.WriteLine(item.GetShortInfo());
				}
			}
			return new EmptyCommand();
		}

		return command;
	}

	private static ICommand ParseReadCommand(string input, TodoList todoList, Profile profile)
	{
		var command = new ReadCommand { todoList = todoList, Profile = profile };
		command.Index = GetIndexFromCommand(input);
		return command;
	}

	private static ICommand ParseDoneCommand(string input, TodoList todoList, Profile profile)
	{
		var command = new DoneCommand { todoList = todoList, Profile = profile };
		command.Index = GetIndexFromCommand(input);
		return command;
	}

	private static ICommand ParseDeleteCommand(string input, TodoList todoList, Profile profile)
	{
		var command = new DeleteCommand { todoList = todoList, Profile = profile };
		command.Index = GetIndexFromCommand(input);
		return command;
	}

	private static ICommand ParseUpdateCommand(string input, TodoList todoList, Profile profile)
	{
		var command = new UpdateCommand { todoList = todoList, Profile = profile };

		string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length >= 2 && int.TryParse(parts[1], out int index))
		{
			command.Index = index - 1;

			int firstSpaceIndex = input.IndexOf(' ');
			if (firstSpaceIndex != -1)
			{
				int secondSpaceIndex = input.IndexOf(' ', firstSpaceIndex + 1);
				if (secondSpaceIndex != -1)
				{
					command.NewText = input.Substring(secondSpaceIndex + 1).Trim(' ', '"');
				}
			}
		}

		return command;
	}

	public static int GetIndexFromCommand(string command)
	{
		string[] parts = command.Split(' ');
		if (parts.Length > 1 && int.TryParse(parts[1], out int index))
		{
			return index - 1;
		}
		return -1;
	}
}


