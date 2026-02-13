using System;
using System.Collections.Generic;

public static class CommandParser
{
    private static Dictionary<string, Func<string, TodoList, Profile, string, string, ICommand>> _commandHandlers = new();
    static CommandParser()
    {
        RegisterCommandHandlers();
    }

    private static void RegisterCommandHandlers()
    {
        _commandHandlers["add"] = ParseAddCommand;
        _commandHandlers["view"] = ParseViewCommand;
        _commandHandlers["delete"] = ParseDeleteCommand;
        _commandHandlers["update"] = ParseUpdateCommand;
        _commandHandlers["read"] = ParseReadCommand;
        _commandHandlers["profile"] = ParseProfileCommand;
        _commandHandlers["status"] = ParseStatusCommand;
        _commandHandlers["undo"] = (args, todoList, profile, todoFilePath, profileFilePath) => new UndoCommand();
        _commandHandlers["redo"] = (args, todoList, profile, todoFilePath, profileFilePath) => new RedoCommand();
        _commandHandlers["help"] = (args, todoList, profile, todoFilePath, profileFilePath) => new HelpCommand();
        _commandHandlers["exit"] = (args, todoList, profile, todoFilePath, profileFilePath) => new ExitCommand();
        _commandHandlers["search"] = ParseSearchCommand;
    }
    public static ICommand Parse(string inputString, TodoList todoList, Profile profile, string todoFilePath, string profileFilePath)
    {
        if (string.IsNullOrWhiteSpace(inputString))
        {
            return new HelpCommand();
        }

        string[] parts = inputString.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            return new HelpCommand();
        }

        string commandName = parts[0].ToLower();
        string args = parts.Length > 1 ? parts[1] : "";

        if (_commandHandlers.TryGetValue(commandName, out var handler))
        {
            return handler(args, todoList, profile, todoFilePath, profileFilePath);
        }
        else
        {
            return new HelpCommand();
        }
    }

    private static ICommand ParseAddCommand(string args, TodoList todoList, Profile profile, string todoFilePath, string profileFilePath)
    {
        var command = new AddCommand
        {
            TodoList = todoList,
            TodoFilePath = todoFilePath
        };

        if (args.Contains("--multiline") || args.Contains("-m"))
        {
            command.IsMultiline = true;
            return command;
        }

        string[] parts = args.Split('"');
        if (parts.Length >= 2)
        {
            command.Text = parts[1].Trim();
        }
        return command;
    }
    private static ICommand ParseViewCommand(string args, TodoList todoList, Profile profile, string todoFilePath, string profileFilePath)
    {
        var command = new ViewCommand
        {
            TodoList = todoList
        };

        string flags = args.Trim();

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
    private static ICommand ParseDeleteCommand(string args, TodoList todoList, Profile profile, string todoFilePath, string profileFilePath)
    {
        var command = new DeleteCommand
        {
            TodoList = todoList,
            TodoFilePath = todoFilePath
        };

        string[] parts = args.Split(' ');
        if (parts.Length >= 1 && int.TryParse(parts[0], out int taskNumber))
        {
            command.TaskNumber = taskNumber;
        }
        return command;
    }

    private static ICommand ParseUpdateCommand(string args, TodoList todoList, Profile profile, string todoFilePath, string profileFilePath)
    {
        var command = new UpdateCommand
        {
            TodoList = todoList,
            TodoFilePath = todoFilePath
        };

        string[] parts = args.Split('"');
        if (parts.Length >= 2)
        {
            command.NewText = parts[1].Trim();

            string indexPart = parts[0].Trim();
            if (int.TryParse(indexPart, out int taskNumber))
            {
                command.TaskNumber = taskNumber;
            }
        }
        return command;
    }

    private static ICommand ParseReadCommand(string args, TodoList todoList, Profile profile, string todoFilePath, string profileFilePath)
    {
        var command = new ReadCommand
        {
            TodoList = todoList
        };

        string[] parts = args.Split(' ');
        if (parts.Length >= 1 && int.TryParse(parts[0], out int taskNumber))
        {
            command.TaskNumber = taskNumber;
        }
        return command;
    }

    private static ICommand ParseProfileCommand(string args, TodoList todoList, Profile profile, string todoFilePath, string profileFilePath)
    {
        var command = new ProfileCommand
        {
            Profile = profile,
            ProfileFilePath = profileFilePath
        };

        string flags = args.Trim();
        command.ShouldLogout = flags.Contains("--out") || flags.Contains("-o");

        return command;
    }
    private static ICommand ParseStatusCommand(string args, TodoList todoList, Profile profile, string todoFilePath, string profileFilePath)
    {
        var command = new StatusCommand
        {
            TodoList = todoList,
            TodoFilePath = todoFilePath
        };

        string[] parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length >= 2)
        {
            if (int.TryParse(parts[0], out int taskNumber))
            {
                command.TaskNumber = taskNumber;
            }

            string statusString = parts[1].ToLower();
            command.Status = statusString switch
            {
                "notstarted" => TodoStatus.NotStarted,
                "inprogress" => TodoStatus.InProgress,
                "completed" => TodoStatus.Completed,
                "postponed" => TodoStatus.Postponed,
                "failed" => TodoStatus.Failed,
                _ => TodoStatus.NotStarted
            };
        }
        return command;
    }

    private static ICommand ParseSearchCommand(string args, TodoList todoList, Profile profile, string todoFilePath, string profileFilePath)
    {
    var command = new SearchCommand
    {
        TodoList = todoList
    };
    
    return command;
    }
}