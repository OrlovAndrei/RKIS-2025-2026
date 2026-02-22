using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
        _commandHandlers["search"] = ParseSearch; // Новая команда
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

    // Новая команда search
    private static ICommand ParseSearch(string args)
    {
        var tokens = ParseArguments(args);
        var cmd = new SearchCommand();

        for (int i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            switch (token)
            {
                case "--contains":
                    if (i + 1 < tokens.Count && !tokens[i + 1].StartsWith("--"))
                        cmd.ContainsText = tokens[++i];
                    else
                        Console.WriteLine("Предупреждение: флаг --contains требует значение");
                    break;
                case "--starts-with":
                    if (i + 1 < tokens.Count && !tokens[i + 1].StartsWith("--"))
                        cmd.StartsWithText = tokens[++i];
                    else
                        Console.WriteLine("Предупреждение: флаг --starts-with требует значение");
                    break;
                case "--ends-with":
                    if (i + 1 < tokens.Count && !tokens[i + 1].StartsWith("--"))
                        cmd.EndsWithText = tokens[++i];
                    else
                        Console.WriteLine("Предупреждение: флаг --ends-with требует значение");
                    break;
                case "--from":
                    if (i + 1 < tokens.Count && !tokens[i + 1].StartsWith("--"))
                    {
                        if (DateTime.TryParse(tokens[++i], out var fromDate))
                            cmd.FromDate = fromDate;
                        else
                            Console.WriteLine("Ошибка: неверный формат даты для --from. Используйте yyyy-MM-dd");
                    }
                    else
                        Console.WriteLine("Предупреждение: флаг --from требует значение");
                    break;
                case "--to":
                    if (i + 1 < tokens.Count && !tokens[i + 1].StartsWith("--"))
                    {
                        if (DateTime.TryParse(tokens[++i], out var toDate))
                            cmd.ToDate = toDate;
                        else
                            Console.WriteLine("Ошибка: неверный формат даты для --to. Используйте yyyy-MM-dd");
                    }
                    else
                        Console.WriteLine("Предупреждение: флаг --to требует значение");
                    break;
                case "--status":
                    if (i + 1 < tokens.Count && !tokens[i + 1].StartsWith("--"))
                    {
                        if (Enum.TryParse<TodoStatus>(tokens[++i], true, out var status))
                            cmd.Status = status;
                        else
                            Console.WriteLine($"Ошибка: неверный статус. Допустимые значения: {string.Join(", ", Enum.GetNames<TodoStatus>())}");
                    }
                    else
                        Console.WriteLine("Предупреждение: флаг --status требует значение");
                    break;
                case "--sort":
                    if (i + 1 < tokens.Count && !tokens[i + 1].StartsWith("--"))
                    {
                        cmd.SortBy = tokens[++i];
                    }
                    else
                        Console.WriteLine("Предупреждение: флаг --sort требует значение (text или date)");
                    break;
                case "--desc":
                    cmd.SortDescending = true;
                    break;
                case "--top":
                    if (i + 1 < tokens.Count && !tokens[i + 1].StartsWith("--"))
                    {
                        if (int.TryParse(tokens[++i], out var top) && top > 0)
                            cmd.Top = top;
                        else
                            Console.WriteLine("Ошибка: --top требует положительное целое число");
                    }
                    else
                        Console.WriteLine("Предупреждение: флаг --top требует значение");
                    break;
                default:
                    // Игнорируем неизвестные флаги или значения, которые не являются флагами (они уже обработаны как значения)
                    break;
            }
        }

        return cmd;
    }

    // Вспомогательный метод для разбора аргументов с учётом кавычек
    private static List<string> ParseArguments(string input)
    {
        var result = new List<string>();
        var current = new System.Text.StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (c == '"')
            {
                if (inQuotes)
                {
                    // Завершаем кавычку
                    inQuotes = false;
                    if (current.Length > 0)
                    {
                        result.Add(current.ToString());
                        current.Clear();
                    }
                }
                else
                {
                    // Начинаем кавычку
                    inQuotes = true;
                }
            }
            else if (c == ' ' && !inQuotes)
            {
                // Разделитель вне кавычек
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