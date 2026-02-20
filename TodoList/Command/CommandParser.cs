using System;
using System.Collections.Generic;

public static class CommandParser
{
    private static TodoList _currentTodoList;
    private static Profile _currentProfile;
    private static string _currentTodoFilePath;
    private static string _currentProfileFilePath;

    private static Dictionary<string, Func<string, ICommand>> _commandHandlers = new();

    static CommandParser()
    {
        RegisterCommandHandlers();
    }

    public static void Initialize(TodoList todoList, Profile profile, string todoFilePath, string profileFilePath)
    {
        _currentTodoList = todoList;
        _currentProfile = profile;
        _currentTodoFilePath = todoFilePath;
        _currentProfileFilePath = profileFilePath;
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
        _commandHandlers["undo"] = (args) => new UndoCommand();
        _commandHandlers["redo"] = (args) => new RedoCommand();
        _commandHandlers["help"] = (args) => new HelpCommand();
        _commandHandlers["exit"] = (args) => new ExitCommand();
        _commandHandlers["search"] = ParseSearchCommand;
    }

    public static ICommand Parse(string inputString)
    {
        if (string.IsNullOrWhiteSpace(inputString))
        {
            throw new InvalidCommandException("Введена пустая строка");
        }

        string[] parts = inputString.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            throw new InvalidCommandException("Не удалось разобрать команду");
        }

        string commandName = parts[0].ToLower();
        string args = parts.Length > 1 ? parts[1] : "";

        if (_commandHandlers.TryGetValue(commandName, out var handler))
        {
            return handler(args);
        }
        else
        {
            throw new InvalidCommandException(commandName, "команда не зарегистрирована в словаре");
        }
    }

    private static ICommand ParseAddCommand(string args)
    {
        var command = new AddCommand
        {
            TodoList = _currentTodoList,
            TodoFilePath = _currentTodoFilePath
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

    private static ICommand ParseViewCommand(string args)
    {
        var command = new ViewCommand
        {
            TodoList = _currentTodoList
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

    private static ICommand ParseDeleteCommand(string args)
    {
        var command = new DeleteCommand
        {
            TodoList = _currentTodoList,
            TodoFilePath = _currentTodoFilePath
        };

        string[] parts = args.Split(' ');
        if (parts.Length >= 1 && int.TryParse(parts[0], out int taskNumber))
        {
            command.TaskNumber = taskNumber;
        }
        return command;
    }

    private static ICommand ParseUpdateCommand(string args)
    {
        var command = new UpdateCommand
        {
            TodoList = _currentTodoList,
            TodoFilePath = _currentTodoFilePath
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

    private static ICommand ParseReadCommand(string args)
    {
        var command = new ReadCommand
        {
            TodoList = _currentTodoList
        };

        string[] parts = args.Split(' ');
        if (parts.Length >= 1 && int.TryParse(parts[0], out int taskNumber))
        {
            command.TaskNumber = taskNumber;
        }
        return command;
    }

    private static ICommand ParseProfileCommand(string args)
    {
        var command = new ProfileCommand
        {
            Profile = _currentProfile,
            ProfileFilePath = _currentProfileFilePath
        };

        string flags = args.Trim();
        command.ShouldLogout = flags.Contains("--out") || flags.Contains("-o");

        return command;
    }

    private static ICommand ParseStatusCommand(string args)
    {
        var command = new StatusCommand
        {
            TodoList = _currentTodoList,
            TodoFilePath = _currentTodoFilePath
        };

        string[] parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length >= 2)
        {
            if (int.TryParse(parts[0], out int taskNumber))
            {
                command.TaskNumber = taskNumber;
            }

            string statusString = parts[1].ToLower();
            command.Status = StatusParser.ParseStatusWithDefault(statusString);
        }
        return command;
    }

    private static ICommand ParseSearchCommand(string args)
    {
        var command = new SearchCommand
        {
            TodoList = _currentTodoList
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

        for (int i = 0; i < tokens.Count; i++)
        {
            string token = tokens[i].ToLower();

            switch (token)
            {
                case "--contains":
                    if (i + 1 < tokens.Count)
                    {
                        command.ContainsText = tokens[i + 1];
                        i++;
                    }
                    break;

                case "--starts-with":
                    if (i + 1 < tokens.Count)
                    {
                        command.StartsWithText = tokens[i + 1];
                        i++;
                    }
                    break;

                case "--ends-with":
                    if (i + 1 < tokens.Count)
                    {
                        command.EndsWithText = tokens[i + 1];
                        i++;
                    }
                    break;

                case "--from":
                    if (i + 1 < tokens.Count)
                    {
                        if (DateTime.TryParse(tokens[i + 1], out DateTime fromDate))
                        {
                            command.FromDate = fromDate;
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
                        command.Status = StatusParser.ParseStatus(statusStr);
                        i++;
                    }
                    break;

                case "--sort":
                    if (i + 1 < tokens.Count)
                    {
                        command.SortBy = tokens[i + 1].ToLower();
                        i++;
                    }
                    break;

                case "--desc":
                    command.SortDescending = true;
                    break;

                case "--top":
                    if (i + 1 < tokens.Count)
                    {
                        if (int.TryParse(tokens[i + 1], out int top) && top > 0)
                        {
                            command.Top = top;
                        }
                        else
                        {
                            Console.WriteLine("Предупреждение: --top требует положительное число");
                        }
                        i++;
                    }
                    break;

                default:
                    if (token.StartsWith("--"))
                    {
                        throw new InvalidArgumentException("флаг", token, "неизвестный флаг для команды search");
                    }
                    break;
            }
        }

        return command;
    }
}