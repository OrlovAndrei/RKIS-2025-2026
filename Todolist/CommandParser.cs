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
            ["redo"] = _ => new RedoCommand()
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
                        Console.WriteLine("?‘?ñ+óø: ?ç?ç‘??‘<ü ‘?‘'ø‘'‘?‘?. \"?‘?‘'‘?õ?‘<ç ‘?‘'ø‘'‘?‘?‘<: NotStarted, InProgress, Completed, Postponed, Failed");
                    }
                }
                else
                {
                    Console.WriteLine("?‘?ñ+óø: ñ??çó‘? ??ç ?ñøõøú??ø.");
                }
            }
            else
            {
                Console.WriteLine("?‘?ñ+óø: ?ç?ç‘??‘<ü ‘\"?‘??ø‘'. ?‘?ñ?ç‘?: status 2 InProgress");
            }
        }
        else
        {
            Console.WriteLine("?‘?ñ+óø: ‘?óøñ‘'ç ñ??çó‘? ñ ‘?‘'ø‘'‘?‘?. ?‘?ñ?ç‘?: status 2 InProgress");
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
                Console.WriteLine("?‘?ñ+óø: ?ç?ç‘??‘<ü ‘\"?‘??ø‘'. ?‘?ñ?ç‘?: update 2 \"???‘<ü ‘'çó‘?‘'\"");
            }
        }
        else
        {
            Console.WriteLine("?‘?ñ+óø: ‘?óøñ‘'ç ñ??çó‘? ñ ???‘<ü ‘'çó‘?‘'. ?‘?ñ?ç‘?: update 2 \"???‘<ü ‘'çó‘?‘'\"");
        }
        return null;
    }

    private static bool TryParseIndex(string arg, int taskCount, out int indexOneBased)
    {
        indexOneBased = -1;
        if (string.IsNullOrWhiteSpace(arg))
        {
            Console.WriteLine("?‘?ñ+óø: ‘?óøñ‘'ç ñ??çó‘? úø?ø‘Øñ.");
            return false;
        }

        if (!int.TryParse(arg.Trim(), out int idxOneBased))
        {
            Console.WriteLine("?‘?ñ+óø: ñ??çó‘? ??>ç? +‘<‘'‘? ‘Øñ‘?>??.");
            return false;
        }

        indexOneBased = idxOneBased;
        if (indexOneBased < 1 || indexOneBased > taskCount)
        {
            Console.WriteLine("?‘?ñ+óø: ñ??çó‘? ??ç ?ñøõøú??ø.");
            return false;
        }

        return true;
    }

    private static bool TryParseStatus(string statusStr, out TodoStatus status)
    {
        status = TodoStatus.NotStarted;
        
        // ??õ‘<‘'óø õø‘?‘?ñ??ø ‘? ‘?‘Øç‘'?? ‘?ç?ñ‘?‘'‘?ø ñ +çú
        if (Enum.TryParse<TodoStatus>(statusStr, true, out TodoStatus parsedStatus))
        {
            status = parsedStatus;
            return true;
        }
        
        // ÷øóç õ???ç‘?ñ?øç? ‘?‘?‘?‘?óñç ?øú?ø?ñ‘? ?>‘? ‘???+‘?‘'?ø
        string statusLower = statusStr.ToLowerInvariant();
        switch (statusLower)
        {
            case "notstarted":
            case "?ç ?ø‘Øø‘'?":
                status = TodoStatus.NotStarted;
                return true;
            case "inprogress":
            case "? õ‘??‘Åç‘?‘?ç":
                status = TodoStatus.InProgress;
                return true;
            case "completed":
            case "?‘<õ?>?ç??":
            case "done":
                status = TodoStatus.Completed;
                return true;
            case "postponed":
            case "?‘'>?ç??":
                status = TodoStatus.Postponed;
                return true;
            case "failed":
            case "õ‘???ø>ç??":
                status = TodoStatus.Failed;
                return true;
            default:
                return false;
        }
    }
}

