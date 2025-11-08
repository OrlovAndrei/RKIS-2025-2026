using System;

public static class CommandParser
{
    public static ICommand Parse(string inputString, TodoList todoList, Profile profile, string todoFilePath, string profileFilePath)
    {
        if (string.IsNullOrWhiteSpace(inputString))
        {
            return new HelpCommand();
        }

        string[] parts = inputString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            return new HelpCommand();
        }

        string commandName = parts[0].ToLower();

        switch (commandName)
        {
            case "add":
                return ParseAddCommand(inputString, todoList, todoFilePath);
            case "view":
                return ParseViewCommand(inputString, todoList);
            case "done":
                return ParseDoneCommand(inputString, todoList, todoFilePath);
            case "delete":
                return ParseDeleteCommand(inputString, todoList, todoFilePath);
            case "update":
                return ParseUpdateCommand(inputString, todoList, todoFilePath);
            case "read":
                return ParseReadCommand(inputString, todoList);
            case "profile":
                return ParseProfileCommand(inputString, profile, profileFilePath);
            case "help":
                return new HelpCommand();
            case "exit":
                return new ExitCommand();
            default:
                return new HelpCommand();
        }
    }

    private static ICommand ParseAddCommand(string input, TodoList todoList, string todoFilePath)
    {
        var command = new AddCommand
        {
            TodoList = todoList,
            TodoFilePath = todoFilePath
        };

        if (input.Contains("--multiline") || input.Contains("-m"))
        {
            command.IsMultiline = true;
            return command;
        }

        string[] parts = input.Split('"');
        if (parts.Length >= 2)
        {
            command.Text = parts[1].Trim();
        }

        return command;
    }

    private static ICommand ParseViewCommand(string input, TodoList todoList)
    {
        var command = new ViewCommand { TodoList = todoList };

        string flags = input.Length > 4 ? input.Substring(4).Trim() : "";

        bool showAll = flags.Contains("-a") || flags.Contains("--all");
        command.ShowIndex = flags.Contains("--index") || flags.Contains("-i") || showAll;
        command.ShowStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
        command.ShowDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;

        if (flags.Contains("-") && flags.Length > 1 && !flags.Contains("--"))
        {
            string shortFlags = flags.Replace("-", "").Replace(" ", "");
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

    private static ICommand ParseDoneCommand(string input, TodoList todoList, string todoFilePath)
    {
        var command = new DoneCommand
        {
            TodoList = todoList,
            TodoFilePath = todoFilePath
        };
        string[] parts = input.Split(' ');

        if (parts.Length >= 2 && int.TryParse(parts[1], out int taskNumber))
        {
            command.TaskNumber = taskNumber;
        }

        return command;
    }

    private static ICommand ParseDeleteCommand(string input, TodoList todoList, string todoFilePath)
    {
        var command = new DeleteCommand
        {
            TodoList = todoList,
            TodoFilePath = todoFilePath
        };
        string[] parts = input.Split(' ');

        if (parts.Length >= 2 && int.TryParse(parts[1], out int taskNumber))
        {
            command.TaskNumber = taskNumber;
        }

        return command;
    }

    private static ICommand ParseUpdateCommand(string input, TodoList todoList, string todoFilePath)
    {
        var command = new UpdateCommand
        {
            TodoList = todoList,
            TodoFilePath = todoFilePath
        };
        string[] parts = input.Split('"');

        if (parts.Length >= 2)
        {
            command.NewText = parts[1].Trim();

            string indexPart = parts[0].Replace("update", "").Trim();
            if (int.TryParse(indexPart, out int taskNumber))
            {
                command.TaskNumber = taskNumber;
            }
        }

        return command;
    }

    private static ICommand ParseReadCommand(string input, TodoList todoList)
    {
        var command = new ReadCommand { TodoList = todoList };
        string[] parts = input.Split(' ');

        if (parts.Length >= 2 && int.TryParse(parts[1], out int taskNumber))
        {
            command.TaskNumber = taskNumber;
        }

        return command;
    }
    private static ICommand ParseProfileCommand(string input, Profile profile, string profileFilePath)
    {
        var command = new ProfileCommand
        {
            Profile = profile,
            ProfileFilePath = profileFilePath
        };
        return command;
    }
}
