using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoList;

public static class CommandParser
{
    private static readonly Dictionary<string, Func<string, ICommand>> _commandHandlers = new();

    static CommandParser()
    {
        _commandHandlers["help"] = ParseHelp;
        _commandHandlers["profile"] = ParseProfile;
        _commandHandlers["add"] = ParseAdd;
        _commandHandlers["view"] = ParseView;
        _commandHandlers["status"] = ParseStatus;
        _commandHandlers["delete"] = ParseDelete;
        _commandHandlers["update"] = ParseUpdate;
        _commandHandlers["read"] = ParseRead;
        _commandHandlers["undo"] = ParseUndo;
        _commandHandlers["redo"] = ParseRedo;
        _commandHandlers["exit"] = ParseExit;
        _commandHandlers["search"] = ParseSearch;   
    }

    public static ICommand? Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var parts = input.Split(' ', 2);
        string cmdName = parts[0].ToLower();
        string args = parts.Length > 1 ? parts[1] : "";

        if (_commandHandlers.TryGetValue(cmdName, out var handler))
            return handler(args);

        Console.WriteLine("Неизвестная команда.");
        return null;
    }

    private static ICommand ParseHelp(string args) => new HelpCommand();

    private static ICommand ParseProfile(string args)
    {
        bool logout = args.Contains("--out") || args.Contains("-o");
        return new ProfileCommand(logout);
    }

    private static ICommand ParseAdd(string args)
    {
        var flags = ParseFlagsFromArgs(args);
        bool isMultiline = flags.Contains("--multiline") || flags.Contains("-m");
        string text = ExtractTextFromArgs(args);
        return new AddCommand(text, isMultiline);
    }

    private static ICommand ParseView(string args)
    {
        var flags = ParseFlagsFromArgs(args);
        bool showAll = flags.Contains("--all") || flags.Contains("-a");
        bool showIndex = flags.Contains("--index") || flags.Contains("-i") || showAll;
        bool showStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
        bool showUpdateDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;
        return new ViewCommand(showIndex, showStatus, showUpdateDate);
    }

    private static ICommand ParseStatus(string args)
    {
        var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            Console.WriteLine("Ошибка: укажите индекс и статус (NotStarted, InProgress, Completed, Postponed, Failed)");
            return null;
        }
        if (!int.TryParse(parts[0], out int index))
        {
            Console.WriteLine("Ошибка: индекс должен быть числом.");
            return null;
        }
        if (!Enum.TryParse<TodoStatus>(parts[1], true, out var status))
        {
            Console.WriteLine($"Ошибка: неверный статус. Допустимые: {string.Join(", ", Enum.GetNames<TodoStatus>())}");
            return null;
        }
        return new StatusCommand(index - 1, status);
    }

    private static ICommand ParseDelete(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
        {
            Console.WriteLine("Ошибка: укажите индекс задачи.");
            return null;
        }
        if (!int.TryParse(args.Trim(), out int index))
        {
            Console.WriteLine("Ошибка: индекс должен быть числом.");
            return null;
        }
        return new DeleteCommand(index - 1);
    }

    private static ICommand ParseUpdate(string args)
    {
        var parts = args.Split(' ', 2);
        if (parts.Length < 2)
        {
            Console.WriteLine("Ошибка: укажите индекс и новый текст задачи.");
            return null;
        }
        if (!int.TryParse(parts[0], out int index))
        {
            Console.WriteLine("Ошибка: индекс должен быть числом.");
            return null;
        }
        string newText = parts[1];
        return new UpdateCommand(index - 1, newText);
    }

    private static ICommand ParseRead(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
        {
            Console.WriteLine("Ошибка: укажите индекс задачи.");
            return null;
        }
        if (!int.TryParse(args.Trim(), out int index))
        {
            Console.WriteLine("Ошибка: индекс должен быть числом.");
            return null;
        }
        return new ReadCommand(index - 1);
    }

    private static ICommand ParseUndo(string args) => new UndoCommand();
    private static ICommand ParseRedo(string args) => new RedoCommand();
    private static ICommand ParseExit(string args) => new ExitCommand();

    private static ICommand ParseSearch(string args)
    {
        string contains = null;
        string startsWith = null;
        string endsWith = null;
        DateTime? from = null;
        DateTime? to = null;
        TodoStatus? status = null;
        string sortBy = null;
        bool descending = false;
        int? top = null;

        var parts = SplitArgsRespectingQuotes(args);
        for (int i = 0; i < parts.Length; i++)
        {
            string flag = parts[i];
            switch (flag)
            {
                case "--contains":
                    if (i + 1 < parts.Length) contains = parts[++i];
                    break;
                case "--starts-with":
                    if (i + 1 < parts.Length) startsWith = parts[++i];
                    break;
                case "--ends-with":
                    if (i + 1 < parts.Length) endsWith = parts[++i];
                    break;
                case "--from":
                    if (i + 1 < parts.Length && DateTime.TryParse(parts[i + 1], out var d1))
                        from = d1;
                    i++;
                    break;
                case "--to":
                    if (i + 1 < parts.Length && DateTime.TryParse(parts[i + 1], out var d2))
                        to = d2;
                    i++;
                    break;
                case "--status":
                    if (i + 1 < parts.Length && Enum.TryParse<TodoStatus>(parts[i + 1], true, out var st))
                        status = st;
                    i++;
                    break;
                case "--sort":
                    if (i + 1 < parts.Length && (parts[i + 1] == "text" || parts[i + 1] == "date"))
                        sortBy = parts[++i];
                    break;
                case "--desc":
                    descending = true;
                    break;
                case "--top":
                    if (i + 1 < parts.Length && int.TryParse(parts[i + 1], out var t) && t > 0)
                        top = t;
                    i++;
                    break;
                default:
                    break;
            }
        }

        return new SearchCommand(contains, startsWith, endsWith, from, to, status, sortBy, descending, top);
    }

    private static HashSet<string> ParseFlagsFromArgs(string args)
    {
        var flags = new HashSet<string>();
        var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            if (part.StartsWith("--"))
                flags.Add(part);
            else if (part.StartsWith("-") && part.Length > 1)
            {
                for (int i = 1; i < part.Length; i++)
                    flags.Add("-" + part[i]);
            }
        }
        return flags;
    }

    private static string ExtractTextFromArgs(string args)
    {
        var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var textParts = parts.Where(p => !p.StartsWith("-")).ToArray();
        return string.Join(" ", textParts);
    }

    private static string[] SplitArgsRespectingQuotes(string args)
    {
        var result = new List<string>();
        bool inQuotes = false;
        int start = 0;
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (args[i] == ' ' && !inQuotes)
            {
                if (i > start)
                    result.Add(args.Substring(start, i - start).Trim('"'));
                start = i + 1;
            }
        }
        if (start < args.Length)
            result.Add(args.Substring(start).Trim('"'));
        return result.ToArray();
    }
}