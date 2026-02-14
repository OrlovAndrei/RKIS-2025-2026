namespace TodoList.commands;

public class CommandParser
{
    public static ICommand Parse(string input)
    {
        var flags = ParseFlags(input);
        var parts = input.Split(' ');
        var command = parts[0].ToLower();
        
        switch (command)
        {
            case "help": return new HelpCommand();
            case "add":
                return new AddCommand
                {
                    multiline = flags.Contains("-m") || flags.Contains("--multi"),
                    parts = parts
                };
            case "view":
                return new ViewCommand
                {
                    ShowIndex = flags.Contains("--index") || flags.Contains("-i"),
                    ShowStatus = flags.Contains("--status") || flags.Contains("-s"),
                    ShowDate = flags.Contains("--update-date") || flags.Contains("-d"),
                    ShowAll = flags.Contains("--all") || flags.Contains("-a")
                };
            case "read":
                return new ReadCommand
                {
                    parts = parts
                };
            case "setstatus":
                return new SetStatusCommand
                {
                    parts = parts
                };
            case "delete":
                return new DeleteCommand
                {
                    parts = parts
                };
            case "update":
                return new UpdateCommand
                {
                    parts = parts
                };
            case "profile":
                return new ProfileCommand
                {
                    parts = parts
                };
            case "undo": return new UndoCommand();
            case "redo": return new RedoCommand();
            case "exit": return new ExitCommand();
            default: return new UnknownCommand();
        }
    }

    private static string[] ParseFlags(string command)
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