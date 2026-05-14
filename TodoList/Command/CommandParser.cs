using System;
using System.Collections.Generic;
using TodoList.Models;
using TodoList.Services.Repositories;

public static class CommandParser
{
    private static IProfileRepository _profileRepo = null!;
    private static ITodoRepository _todoRepo = null!;
    private static TodoList _currentTodoList = null!;
    private static Profile _currentProfile = null!;

    private static Dictionary<string, Func<string, ICommand>> _commandHandlers = new();

    static CommandParser()
    {
        RegisterCommandHandlers();
    }

    public static void Initialize(IProfileRepository profileRepo, ITodoRepository todoRepo, TodoList currentTodoList, Profile currentProfile)
    {
        _profileRepo = profileRepo;
        _todoRepo = todoRepo;
        _currentTodoList = currentTodoList;
        _currentProfile = currentProfile;
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
        _commandHandlers["undo"] = _ => new UndoCommand();
        _commandHandlers["redo"] = _ => new RedoCommand();
        _commandHandlers["help"] = _ => new HelpCommand();
        _commandHandlers["exit"] = _ => new ExitCommand();
        _commandHandlers["search"] = ParseSearchCommand;
        _commandHandlers["load"] = ParseLoadCommand;
        _commandHandlers["sync"] = ParseSyncCommand;
    }

    public static ICommand Parse(string inputString)
    {
        if (string.IsNullOrWhiteSpace(inputString))
            throw new InvalidCommandException("Введена пустая строка");

        string[] parts = inputString.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            throw new InvalidCommandException("Не удалось разобрать команду");

        string commandName = parts[0].ToLower();
        string args = parts.Length > 1 ? parts[1] : "";

        if (_commandHandlers.TryGetValue(commandName, out var handler))
            return handler(args);
        else
            throw new InvalidCommandException(commandName, "команда не зарегистрирована в словаре");
    }

    private static ICommand ParseAddCommand(string args)
    {
        var command = new AddCommand
        {
            TodoList = _currentTodoList,
            TodoRepo = _todoRepo,
            ProfileId = _currentProfile.Id
        };
        if (args.Contains("--multiline") || args.Contains("-m"))
            command.IsMultiline = true;
        else
        {
            string[] parts = args.Split('"');
            if (parts.Length >= 2)
                command.Text = parts[1].Trim();
        }
        return command;
    }

    private static ICommand ParseViewCommand(string args) => new ViewCommand { TodoList = _currentTodoList };
    private static ICommand ParseDeleteCommand(string args)
    {
        var command = new DeleteCommand { TodoList = _currentTodoList, TodoRepo = _todoRepo };
        if (int.TryParse(args.Trim(), out int num)) command.TaskNumber = num;
        return command;
    }
    private static ICommand ParseUpdateCommand(string args)
    {
        var command = new UpdateCommand { TodoList = _currentTodoList, TodoRepo = _todoRepo, NewText = "" };
        string[] parts = args.Split('"');
        if (parts.Length >= 2)
        {
            command.NewText = parts[1].Trim();
            if (int.TryParse(parts[0].Trim(), out int num)) command.TaskNumber = num;
        }
        return command;
    }
    private static ICommand ParseReadCommand(string args)
    {
        var command = new ReadCommand { TodoList = _currentTodoList };
        if (int.TryParse(args.Trim(), out int num)) command.TaskNumber = num;
        return command;
    }
    private static ICommand ParseProfileCommand(string args)
    {
        var command = new ProfileCommand { Profile = _currentProfile };
        command.ShouldLogout = args.Contains("--out") || args.Contains("-o");
        return command;
    }
    private static ICommand ParseStatusCommand(string args)
    {
        var command = new StatusCommand { TodoList = _currentTodoList, TodoRepo = _todoRepo };
        string[] parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2)
        {
            if (int.TryParse(parts[0], out int num)) command.TaskNumber = num;
            command.Status = StatusParser.ParseStatusWithDefault(parts[1]);
        }
        return command;
    }
    private static ICommand ParseSearchCommand(string args)
    {
        var command = new SearchCommand { TodoList = _currentTodoList };
        return command;
    }
    private static ICommand ParseLoadCommand(string args)
    {
        var command = new LoadCommand();
        string[] parts = args.Split(' ');
        if (parts.Length == 2 && int.TryParse(parts[0], out int c) && int.TryParse(parts[1], out int s))
        {
            command.DownloadsCount = c;
            command.DownloadSize = s;
        }
        else throw new InvalidCommandException("load <количество> <размер>");
        return command;
    }
    private static ICommand ParseSyncCommand(string args)
    {
        var command = new SyncCommand();
        command.IsPull = args.Contains("--pull");
        command.IsPush = args.Contains("--push");
        return command;
    }
}