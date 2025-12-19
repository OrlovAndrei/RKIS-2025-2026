namespace TodoList.Commands;

public class CommandParser
{
	public static Profile Profile = FileManager.LoadProfile();
	public static ICommand Parse(string input, TodoList todoList)
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
				return new ViewCommand
				{
					TodoList = todoList,
					ShowIndex = flags.Contains("--index") || flags.Contains("-i"),
					ShowStatus = flags.Contains("--status") || flags.Contains("-s"),
					ShowDate = flags.Contains("--update-date") || flags.Contains("-d"),
					HasAll = flags.Contains("--all") || flags.Contains("-a")
				};

			case "done":
				return new DoneCommand
				{
					TodoList = todoList,
					TaskIndex = int.Parse(parts[1])
				};

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
				var newParts = parts[1].Trim().Split(' ');
				return new UpdateCommand
				{
					TodoList = todoList,
					TaskIndex = int.Parse(newParts[0]),
					NewText = newParts[1]
				};

			case "profile":
				return new ProfileCommand
				{
					Text = input
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
		var flags = new List<string>();
		foreach (var text in command.Split(' '))
			if (text.StartsWith("-"))
				for (var i = 1; i < text.Length; i++)
					flags.Add("-" + text[i]);
			else if (text.StartsWith("--")) flags.Add(text);

		return flags.ToArray();
	}
}