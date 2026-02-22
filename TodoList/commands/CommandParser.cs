using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoList.Exceptions;

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
        _commandHandlers["search"] = ParseSearch;
    }

    public static ICommand Parse(string input)
    {
        var parts = input.Split(' ', 2);
        var command = parts[0].ToLower();
        var args = parts.Length > 1 ? parts[1] : "";

        if (_commandHandlers.TryGetValue(command, out var handler))
            return handler(args);

        throw new InvalidCommandException($"Неизвестная команда '{command}'. Введите 'help' для списка команд.");
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

    private static ICommand ParseSearch(string args)
    {
        var tokens = ParseArguments(args);
        var cmd = new SearchCommand();
        var recognizedFlags = new HashSet<string> { "--contains", "--starts-with", "--ends-with", "--from", "--to", "--status", "--sort", "--desc", "--top" };

        for (int i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (!token.StartsWith("--"))
                continue; // пропускаем значения, они будут обработаны как аргументы флагов

            if (!recognizedFlags.Contains(token))
                throw new InvalidArgumentException($"Неизвестный флаг '{token}' для команды search.");

            switch (token)
            {
                case "--contains":
                    if (i + 1 >= tokens.Count || tokens[i + 1].StartsWith("--"))
                        throw new InvalidArgumentException("Флаг --contains требует значение.");
                    cmd.ContainsText = tokens[++i];
                    break;
                case "--starts-with":
                    if (i + 1 >= tokens.Count || tokens[i + 1].StartsWith("--"))
                        throw new InvalidArgumentException("Флаг --starts-with требует значение.");
                    cmd.StartsWithText = tokens[++i];
                    break;
                case "--ends-with":
                    if (i + 1 >= tokens.Count || tokens[i + 1].StartsWith("--"))
                        throw new InvalidArgumentException("Флаг --ends-with требует значение.");
                    cmd.EndsWithText = tokens[++i];
                    break;
                case "--from":
                    if (i + 1 >= tokens.Count || tokens[i + 1].StartsWith("--"))
                        throw new InvalidArgumentException("Флаг --from требует значение.");
                    var fromStr = tokens[++i];
                    if (!DateTime.TryParse(fromStr, out var fromDate))
                        throw new InvalidArgumentException($"Неверный формат даты для --from: '{fromStr}'. Ожидается yyyy-MM-dd.");
                    cmd.FromDate = fromDate;
                    break;
                case "--to":
                    if (i + 1 >= tokens.Count || tokens[i + 1].StartsWith("--"))
                        throw new InvalidArgumentException("Флаг --to требует значение.");
                    var toStr = tokens[++i];
                    if (!DateTime.TryParse(toStr, out var toDate))
                        throw new InvalidArgumentException($"Неверный формат даты для --to: '{toStr}'. Ожидается yyyy-MM-dd.");
                    cmd.ToDate = toDate;
                    break;
                case "--status":
                    if (i + 1 >= tokens.Count || tokens[i + 1].StartsWith("--"))
                        throw new InvalidArgumentException("Флаг --status требует значение.");
                    var statusStr = tokens[++i];
                    if (!Enum.TryParse<TodoStatus>(statusStr, true, out var status))
                        throw new InvalidArgumentException($"Неверный статус '{statusStr}'. Допустимые значения: {string.Join(", ", Enum.GetNames<TodoStatus>())}.");
                    cmd.Status = status;
                    break;
                case "--sort":
                    if (i + 1 >= tokens.Count || tokens[i + 1].StartsWith("--"))
                        throw new InvalidArgumentException("Флаг --sort требует значение (text или date).");
                    cmd.SortBy = tokens[++i];
                    if (cmd.SortBy != "text" && cmd.SortBy != "date")
                        throw new InvalidArgumentException("Флаг --sort принимает только значения 'text' или 'date'.");
                    break;
                case "--desc":
                    cmd.SortDescending = true;
                    break;
                case "--top":
                    if (i + 1 >= tokens.Count || tokens[i + 1].StartsWith("--"))
                        throw new InvalidArgumentException("Флаг --top требует положительное целое число.");
                    var topStr = tokens[++i];
                    if (!int.TryParse(topStr, out var top) || top <= 0)
                        throw new InvalidArgumentException("Флаг --top требует положительное целое число.");
                    cmd.Top = top;
                    break;
            }
        }

        return cmd;
    }

    private static List<string> ParseArguments(string input)
    {
        var result = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (c == '"')
            {
                if (inQuotes)
                {
                    inQuotes = false;
                    if (current.Length > 0)
                    {
                        result.Add(current.ToString());
                        current.Clear();
                    }
                }
                else
                {
                    inQuotes = true;
                }
            }
            else if (c == ' ' && !inQuotes)
            {
                if (current.Length > 0)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
            }
            else
            {
                current.Append(c);
            }
        }

        if (current.Length > 0)
            result.Add(current.ToString());

        return result;
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