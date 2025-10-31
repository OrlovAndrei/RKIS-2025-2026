namespace TodoList;

public static class CommandParser
{
	public static ICommand Parse(string input, TodoList todoList, Profile profile)
	{
		string[] parts = input.Trim().Split(' ', 2);
		string commandName = parts[0].ToLower();
		string args = parts.Length > 1 ? parts[1] : "";

		switch (commandName)
		{
			case "add":
				var addCmd = new AddCommand
				{
					TodoList = todoList,
					IsMultiline = args.Contains("--multi") || args.Contains("-m"),
					TaskText = args.Replace("--multi", "").Replace("-m", "").Trim()
				};
				return addCmd;

			case "view":
				var viewCmd = new ViewCommand
				{
					TodoList = todoList,
					HasAll = args.Contains("--all") || args.Contains("-a"),
					HasIndex = args.Contains("--index") || args.Contains("-i"),
					HasStatus = args.Contains("--status") || args.Contains("-s"),
					HasDate = args.Contains("--update-date") || args.Contains("-d")
				};
				return viewCmd;

			case "done":
				return new DoneCommand
				{
					TodoList = todoList,
					TaskIndex = int.Parse(args) - 1
				};

			case "delete":
				return new DeleteCommand
				{
					TodoList = todoList,
					TaskIndex = int.Parse(args) - 1
				};

			case "profile":
				return new ProfileCommand { Profile = profile };

			case "help":
				return new HelpCommand();

			case "exit":
				return new ExitCommand();

			default:
				return new UnknownCommand();
		}
	}
}