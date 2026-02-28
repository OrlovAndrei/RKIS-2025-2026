using TodoList.commands;

namespace TodoList.classes;

public class CommandParser
{
	public static Profile profile = FileManager.LoadProfile();
	public static TodoList todoList = FileManager.LoadTodos();
	public static ICommand Parse(string input)
	{
		var parts = input.Trim().Split(' ', 2);
		var commandName = parts[0].ToLower();
		var flags = ParseFlags(input);

		switch (commandName)
		{
			case "add":
				return new AddCommand
				{
					TodoList = todoList,
					IsMultiline = flags.Contains("--multi") || flags.Contains("-m"),
					TaskText = parts[1]
				};

			case "view":
				var showAll = flags.Contains("--all") || flags.Contains("-a");
				return new ViewCommand
				{
					TodoList = todoList,
					ShowIndex = flags.Contains("--index") || flags.Contains("-i") || showAll,
					ShowStatus = flags.Contains("--status") || flags.Contains("-s") || showAll,
					ShowDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll
				};

			case "status":
			{
				var newParts = parts[1].Trim().Split(' ');
				return new SetStausCommand
				{
					TodoList = todoList,
					TaskIndex = int.Parse(newParts[0]),
					EnumValue = newParts[1]
				};
			}
				

			case "read":
				return new ReadCommand
				{
					TodoList = todoList,
					TaskIndex = int.Parse(parts[1])
				};

			case "delete":
				return new DeleteCommand
				{
					TodoList = todoList,
					TaskIndex = int.Parse(parts[1])
				};

			case "update":
			{
				var newParts = parts[1].Trim().Split(' ');
				return new UpdateCommand
				{
					TodoList = todoList,
					TaskIndex = int.Parse(newParts[0]),
					NewText = newParts[1]
				};
			}

			case "profile":
				return new ProfileCommand
				{
					Profile = profile
				};
			
			case "set":
				return new SetProfileCommand
				{
					Parts = parts[1].Split()
				};

			case "help":
				return new HelpCommand();

			case "exit":
				return new ExitCommand();

			default:
				return new UnknownCommand();
		}
	}

	private static string[] ParseFlags(string command)
	{
		List<string> flags = new();

		foreach (var part in command.Split(' '))
			if (part.StartsWith("--"))
				flags.Add(part);
			else if (part.StartsWith("-"))
				for (var i = 1; i < part.Length; i++)
					flags.Add("-" + part[i]);

		return flags.ToArray();
	}
}