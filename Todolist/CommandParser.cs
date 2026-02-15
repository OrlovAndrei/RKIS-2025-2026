using System;
using System.Collections.Generic;
using Todolist.Commands;

static class CommandParser
{
    private static readonly Dictionary<string, Func<string, ICommand?>> _commandHandlers;

    static CommandParser()
    {
        _commandHandlers = new Dictionary<string, Func<string, ICommand?>>(StringComparer.OrdinalIgnoreCase)
        {
            ["profile"] = ParseProfile,
            ["add"] = ParseAdd,
            ["view"] = ParseView,
            ["read"] = ParseRead,
            ["status"] = ParseStatus,
            ["delete"] = ParseDelete,
            ["update"] = ParseUpdate,
            ["undo"] = _ => new UndoCommand(),
            ["redo"] = _ => new RedoCommand(),
            ["search"] = ParseSearch
        };
    }

    public static ICommand? Parse(string inputString)
    {
        if (string.IsNullOrWhiteSpace(inputString))
        {
            return null;
        }

        string[] parts = inputString.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string command = parts.Length > 0 ? parts[0] : string.Empty;
        string args = parts.Length > 1 ? parts[1] : string.Empty;

        if (_commandHandlers.TryGetValue(command, out var handler))
        {
            return handler(args);
        }

        return null;
    }

    private static ICommand? ParseProfile(string args) => new ProfileCommand(args);

    private static ICommand? ParseAdd(string args) => new AddCommand(args);

    private static ICommand? ParseView(string args) => new ViewCommand(args);

    private static ICommand? ParseSearch(string args) => new SearchCommand(args);

    private static ICommand? ParseRead(string args)
    {
        if (TryParseIndex(args, AppInfo.Todos.Count, out int readIndex))
        {
            return new ReadCommand(readIndex);
        }
        return null;
    }

    private static ICommand? ParseStatus(string args)
    {
        if (!string.IsNullOrWhiteSpace(args))
        {
            string[] statusParts = args.Trim().Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (statusParts.Length >= 2 && int.TryParse(statusParts[0], out int statusIndex))
            {
                if (statusIndex >= 1 && statusIndex <= AppInfo.Todos.Count)
                {
                    string statusStr = statusParts[1].Trim();
                    if (TryParseStatus(statusStr, out TodoStatus status))
                    {
                        return new StatusCommand(statusIndex, status);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неизвестный статус. Возможные: NotStarted, InProgress, Completed, Postponed, Failed");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: индекс вне диапазона.");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: неверный формат. Пример: status 2 InProgress");
            }
        }
        else
        {
            Console.WriteLine("Ошибка: не указаны параметры. Пример: status 2 InProgress");
        }
        return null;
    }

    private static ICommand? ParseDelete(string args)
    {
        if (TryParseIndex(args, AppInfo.Todos.Count, out int deleteIndex))
        {
            return new DeleteCommand(deleteIndex);
        }
        return null;
    }

    private static ICommand? ParseUpdate(string args)
    {
        if (!string.IsNullOrWhiteSpace(args))
        {
            string[] updateParts = args.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (updateParts.Length >= 2 && int.TryParse(updateParts[0], out int updateIndex))
            {
                string newText = updateParts[1].Trim().Trim('"');
                return new UpdateCommand(updateIndex, newText);
            }
            else
            {
                Console.WriteLine("Ошибка: неверный формат. Пример: update 2 \"Новый текст\"");
            }
        }
        else
        {
            Console.WriteLine("Ошибка: не указаны параметры. Пример: update 2 \"Новый текст\"");
        }
        return null;
    }

    private static bool TryParseIndex(string arg, int taskCount, out int indexOneBased)
    {
        indexOneBased = -1;
        if (string.IsNullOrWhiteSpace(arg))
        {
            Console.WriteLine("Ошибка: не указан индекс.");
            return false;
        }

        if (!int.TryParse(arg.Trim(), out int idxOneBased))
        {
            Console.WriteLine("Ошибка: индекс должен быть числом.");
            return false;
        }

        indexOneBased = idxOneBased;
        if (indexOneBased < 1 || indexOneBased > taskCount)
        {
            Console.WriteLine("Ошибка: индекс вне диапазона.");
            return false;
        }

        return true;
    }

    private static bool TryParseStatus(string statusStr, out TodoStatus status)
    {
        status = TodoStatus.NotStarted;

        if (Enum.TryParse<TodoStatus>(statusStr, true, out TodoStatus parsedStatus))
        {
            status = parsedStatus;
            return true;
        }

        string statusLower = statusStr.ToLowerInvariant();
        switch (statusLower)
        {
            case "notstarted":
            case "неначата":
                status = TodoStatus.NotStarted;
                return true;
            case "inprogress":
            case "вработе":
                status = TodoStatus.InProgress;
                return true;
            case "completed":
            case "done":
            case "завершена":
                status = TodoStatus.Completed;
                return true;
            case "postponed":
            case "отложена":
                status = TodoStatus.Postponed;
                return true;
            case "failed":
            case "провалена":
                status = TodoStatus.Failed;
                return true;
            default:
                return false;
        }
    }
}

