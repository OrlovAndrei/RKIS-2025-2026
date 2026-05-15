using System;
using System.Collections.Generic;

public static class CommandParser
{
    private static IProfileRepository _profileRepo = null!;
    private static ITodoRepository _todoRepo = null!;
    private static TodoList _currentTodoList = null!;
    private static Profile _currentProfile = null!;

    private static Dictionary<string, Func<string, ICommand>> _handlers = new();

    static CommandParser()
    {
        _handlers["add"] = ParseAdd;
        _handlers["view"] = a => new ViewCommand { TodoList = _currentTodoList };
        _handlers["delete"] = a => new DeleteCommand { TodoList = _currentTodoList, TodoRepo = _todoRepo };
        _handlers["update"] = a => new UpdateCommand { TodoList = _currentTodoList, TodoRepo = _todoRepo };
        _handlers["read"] = a => new ReadCommand { TodoList = _currentTodoList };
        _handlers["profile"] = a => new ProfileCommand { Profile = _currentProfile };
        _handlers["status"] = a => new StatusCommand { TodoList = _currentTodoList, TodoRepo = _todoRepo };
        _handlers["undo"] = a => new UndoCommand();
        _handlers["redo"] = a => new RedoCommand();
        _handlers["help"] = a => new HelpCommand();
        _handlers["exit"] = a => new ExitCommand();
        _handlers["search"] = a => new SearchCommand { TodoList = _currentTodoList };
        _handlers["load"] = ParseLoad;
        _handlers["sync"] = a => new SyncCommand();
    }

    public static void Initialize(TodoList todoList, Profile profile, ITodoRepository todoRepo, IProfileRepository profileRepo)
    {
        _currentTodoList = todoList;
        _currentProfile = profile;
        _todoRepo = todoRepo;
        _profileRepo = profileRepo;
    }

    public static ICommand Parse(string input)
    {
        var parts = input.Split(' ', 2);
        var name = parts[0].ToLower();
        var args = parts.Length > 1 ? parts[1] : "";

        if (_handlers.TryGetValue(name, out var handler))
            return handler(args);
        throw new InvalidCommandException(name, "неизвестная команда");
    }

    private static ICommand ParseAdd(string args)
    {
        var cmd = new AddCommand
        {
            TodoList = _currentTodoList,
            TodoRepo = _todoRepo,
            ProfileId = _currentProfile.Id
        };
        if (args.Contains("--multiline") || args.Contains("-m"))
            cmd.IsMultiline = true;
        else
        {
            var parts = args.Split('"');
            if (parts.Length >= 2)
                cmd.Text = parts[1].Trim();
        }
        return cmd;
    }

    private static ICommand ParseLoad(string args)
    {
        var parts = args.Split(' ');
        if (parts.Length == 2 && int.TryParse(parts[0], out int c) && int.TryParse(parts[1], out int s))
            return new LoadCommand { DownloadsCount = c, DownloadSize = s };
        throw new InvalidCommandException("load <количество> <размер>");
    }
}