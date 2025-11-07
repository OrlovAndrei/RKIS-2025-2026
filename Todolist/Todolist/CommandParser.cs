using TodoList.Commands;

namespace TodoList;

public class CommandParser
{
    public static Profile profile = FileManager.LoadProfile(Program.profileFilePath);
    public static TodoList todoList = FileManager.LoadTodos(Program.todoFilePath);

    public static ICommand Parse(string input)
    {
        var parts = input.Trim().Split(' ', 2);
        var commandName = parts[0].ToLower();
        var args = parts.Length > 1 ? parts[1] : "";

        var updateParts = input.Split(' ', 3);

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
                    TaskIndex = int.Parse(args) - 1
                };

            case "update":
                return new UpdateCommand
                {
                    todoList = todoList,
                    TaskIndex = int.Parse(updateParts[1]) - 1,
                    NewText = updateParts[2]
                };

            case "profile":
                return new ProfileCommand
                {
                    profile = profile
                };

            case "setprofile":
                return new SetProfileCommand();

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
            if (part.StartsWith("--"))
                flags.Add(part);
            else if (part.StartsWith("-"))
                for (var i = 1; i < part.Length; i++)
                    flags.Add("-" + part[i]);

        return flags.ToArray();
    }
}