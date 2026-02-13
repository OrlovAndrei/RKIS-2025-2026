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

        if (string.IsNullOrWhiteSpace(args))
        {
            return command;
        }

        List<string> tokens = new List<string>();
        bool inQuotes = false;
        string currentToken = "";

        for (int i = 0; i < args.Length; i++)
        {
            char c = args[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
                if (!inQuotes && !string.IsNullOrEmpty(currentToken))
                {
                    tokens.Add(currentToken);
                    currentToken = "";
                }
            }
            else if (c == ' ' && !inQuotes)
            {
                if (!string.IsNullOrEmpty(currentToken))
                {
                    tokens.Add(currentToken);
                    currentToken = "";
                }
            }
            else
            {
                currentToken += c;
            }
        }

        if (!string.IsNullOrEmpty(currentToken))
        {
            tokens.Add(currentToken);
        }

        // Парсим флаги
        for (int i = 0; i < tokens.Count; i++)
        {
            string token = tokens[i].ToLower();

            switch (token)
            {
                case "--contains":
                    if (i + 1 < tokens.Count)
                    {
                        command.ContainsText = tokens[i + 1];
                        Console.WriteLine($"DEBUG: --contains = {command.ContainsText}"); // Для отладки
                        i++;
                    }
                    break;

                case "--starts-with":
                    if (i + 1 < tokens.Count)
                    {
                        command.StartsWithText = tokens[i + 1];
                        Console.WriteLine($"DEBUG: --starts-with = {command.StartsWithText}"); // Для отладки
                        i++;
                    }
                    break;

                case "--ends-with":
                    if (i + 1 < tokens.Count)
                    {
                        command.EndsWithText = tokens[i + 1];
                        Console.WriteLine($"DEBUG: --ends-with = {command.EndsWithText}"); // Для отладки
                        i++;
                    }
                    break;

                case "--from":
                    if (i + 1 < tokens.Count)
                    {
                        if (DateTime.TryParse(tokens[i + 1], out DateTime fromDate))
                        {
                            command.FromDate = fromDate;
                            Console.WriteLine($"DEBUG: --from = {fromDate}"); // Для отладки
                        }
                        else
                        {
                            Console.WriteLine("Предупреждение: неверный формат даты. Используйте yyyy-MM-dd");
                        }
                        i++;
                    }
                    break;

                case "--to":
                    if (i + 1 < tokens.Count)
                    {
                        if (DateTime.TryParse(tokens[i + 1], out DateTime toDate))
                        {
                            command.ToDate = toDate;
                            Console.WriteLine($"DEBUG: --to = {toDate}"); // Для отладки
                        }
                        else
                        {
                            Console.WriteLine("Предупреждение: неверный формат даты. Используйте yyyy-MM-dd");
                        }
                        i++;
                    }
                    break;

                case "--status":
                    if (i + 1 < tokens.Count)
                    {
                        string statusStr = tokens[i + 1].ToLower();
                        command.Status = statusStr switch
                        {
                            "notstarted" => TodoStatus.NotStarted,
                            "inprogress" => TodoStatus.InProgress,
                            "completed" => TodoStatus.Completed,
                            "postponed" => TodoStatus.Postponed,
                            "failed" => TodoStatus.Failed,
                            _ => null
                        };
                        Console.WriteLine($"DEBUG: --status = {statusStr} -> {command.Status}"); // Для отладки
                        i++;
                    }
                    break;

                case "--sort":
                    if (i + 1 < tokens.Count)
                    {
                        command.SortBy = tokens[i + 1].ToLower();
                        Console.WriteLine($"DEBUG: --sort = {command.SortBy}"); // Для отладки
                        i++;
                    }
                    break;

                case "--desc":
                    command.SortDescending = true;
                    Console.WriteLine($"DEBUG: --desc = true"); // Для отладки
                    break;

                case "--top":
                    if (i + 1 < tokens.Count)
                    {
                        if (int.TryParse(tokens[i + 1], out int top) && top > 0)
                        {
                            command.Top = top;
                            Console.WriteLine($"DEBUG: --top = {top}"); // Для отладки
                        }
                        else
                        {
                            Console.WriteLine("Предупреждение: --top требует положительное число");
                        }
                        i++;
                    }
                    break;
            }
        }

        return command;
    }
}