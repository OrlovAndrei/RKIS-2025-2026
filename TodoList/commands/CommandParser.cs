using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoList.commands;

public static class CommandParser
{
    private static readonly Dictionary<string, Func<string, ICommand>> _commandHandlers = new();

    static CommandParser()
    {
        _commandHandlers["help"] = ParseHelp;
        _commandHandlers["add"] = ParseAdd;
        _commandHandlers["view"] = ParseView;
        _commandHandlers["read"] = ParseRead;
        _commandHandlers["setstatus"] = ParseSetStatus;
        _commandHandlers["delete"] = ParseDelete;
        _commandHandlers["update"] = ParseUpdate;
        _commandHandlers["profile"] = ParseProfile;
        _commandHandlers["undo"] = ParseUndo;
        _commandHandlers["redo"] = ParseRedo;
        _commandHandlers["exit"] = ParseExit;
    }

    public static ICommand Parse(string input)
    {
        var parts = input.Split(' ', 2);
        var command = parts[0].ToLower();
        var args = parts.Length > 1 ? parts[1] : "";

        if (_commandHandlers.TryGetValue(command, out var handler))
            return handler(args);

        Console.WriteLine("Неизвестная команда.");
        return new HelpCommand();
    }

    private static ICommand ParseHelp(string args) => new HelpCommand();

    private static ICommand ParseAdd(string args)
    {
        var flags = ParseFlags(args);
        var multiline = flags.Contains("-m") || flags.Contains("--multi");
        var parts = args.Split(' ').Where(p => !p.StartsWith("-")).ToArray();
        var fullParts = new List<string> { "add" };
        fullParts.AddRange(parts);
        return new AddCommand
        {
            parts = fullParts.ToArray(),
            multiline = multiline
        };
    }

    private static ICommand ParseView(string args)
    {
        var flags = ParseFlags(args);
        return new ViewCommand
        {
            ShowIndex = flags.Contains("--index") || flags.Contains("-i"),
            ShowStatus = flags.Contains("--status") || flags.Contains("-s"),
            ShowDate = flags.Contains("--update-date") || flags.Contains("-d"),
            ShowAll = flags.Contains("--all") || flags.Contains("-a")
        };
    }

    private static ICommand ParseRead(string args)
    {
        var parts = args.Split(' ');
        var fullParts = new List<string> { "read" };
        fullParts.AddRange(parts);
        return new ReadCommand { parts = fullParts.ToArray() };
    }

    private static ICommand ParseSetStatus(string args)
    {
        var parts = args.Split(' ');
        var fullParts = new List<string> { "setstatus" };
        fullParts.AddRange(parts);
        return new SetStatusCommand { parts = fullParts.ToArray() };
    }

    private static ICommand ParseDelete(string args)
    {
        var parts = args.Split(' ');
        var fullParts = new List<string> { "delete" };
        fullParts.AddRange(parts);
        return new DeleteCommand { parts = fullParts.ToArray() };
    }

    private static ICommand ParseUpdate(string args)
    {
        var parts = args.Split(' ');
        var fullParts = new List<string> { "update" };
        fullParts.AddRange(parts);
        return new UpdateCommand { parts = fullParts.ToArray() };
    }

    private static ICommand ParseProfile(string args)
    {
        var parts = args.Split(' ');
        var fullParts = new List<string> { "profile" };
        fullParts.AddRange(parts);
        return new ProfileCommand { parts = fullParts.ToArray() };
    }

    private static ICommand ParseUndo(string args) => new UndoCommand();
    private static ICommand ParseRedo(string args) => new RedoCommand();
    private static ICommand ParseExit(string args) => new ExitCommand();

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