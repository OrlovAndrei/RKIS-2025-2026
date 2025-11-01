namespace TodoList;

public class CommandParser
{
    public static ICommand Parse(string input, TodoList todoList, Profile profile)
    {
        string[] parts = input.Trim().Split(' ', 2);
        string commandName = parts[0].ToLower();
        string args = parts.Length > 1 ? parts[1] : "";

        switch (commandName)
        {
            case "add":
                return new AddCommand
                {
                    todos = todoList,
                    input = input
                };

            case "view":
                return new ViewCommand
                {
                    todoList = todoList,
                    ShowIndex = args.Contains("--index") || args.Contains("-i"),
                    ShowStatus = args.Contains("--status") || args.Contains("-s"),
                    ShowDate = args.Contains("--update-date") || args.Contains("-d"),
                    ShowAll = args.Contains("--all") || args.Contains("-a")
                };

            case "done":
                return new DoneCommand
                {
                    todoList = todoList,
                    TaskIndex = int.Parse(args) - 1
                };

            case "delete":
                return new DeleteCommand
                {
                    todoList = todoList,
                    TaskIndex = int.Parse(args) - 1
                };
            
            case "read":
                return new ReadCommand
                {
                    todoList = todoList,
                    TaskIndex = int.Parse(args) -1
                };
            
            case "update":
                return new UpdateCommand
                {
                    todoList = todoList,
                    TaskIndex = int.Parse(args.Split(" ", 2)[0]),
                    NewText = args
                };

            case "profile":
                return new ProfileCommand
                {
                    profile = profile
                };

            case "help":
                return new HelpCommand();

            case "exit":
                return new ExitCommand();

            default:
                return new UnknownCommand();
        }
    }
    
    public static string[] ParseFlags(string command)
    {
        var parts = command.Split(' ');
        var flags = new List<string>();

        foreach (var part in parts)
        {
            if (part.StartsWith("--"))
            {
                flags.Add(part);
            }
            else if (part.StartsWith("-"))
            {
                for (int i = 1; i < part.Length; i++)
                    flags.Add("-" + part[i]);
            }
        }

        return flags.ToArray();
    }
}