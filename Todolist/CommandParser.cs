using System;
using System.Collections.Generic;
using Todolist.Commands;
using Todolist.Exceptions;

static class CommandParser
{
    private static readonly Dictionary<string, Func<string, ICommand>> _commandHandlers;

    static CommandParser()
    {
        _commandHandlers = new Dictionary<string, Func<string, ICommand>>(StringComparer.OrdinalIgnoreCase)
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

    public static ICommand Parse(string inputString)
    {
        if (string.IsNullOrWhiteSpace(inputString))
        {
            throw new InvalidArgumentException("Введите команду (пустая строка не допускается).");
        }

        string[] parts = inputString.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string command = parts.Length > 0 ? parts[0] : string.Empty;
        string args = parts.Length > 1 ? parts[1] : string.Empty;

        if (!_commandHandlers.TryGetValue(command, out var handler))
        {
            throw new InvalidCommandException($"Неизвестная команда: {command}. Введите 'help' для подсказки.");
        }

        return handler(args);
    }

    private static ICommand ParseProfile(string args) => new ProfileCommand(args);

    private static ICommand ParseAdd(string args) => new AddCommand(args);

    private static ICommand ParseView(string args) => new ViewCommand(args);

    private static ICommand ParseSearch(string args) => new SearchCommand(args);

    private static ICommand ParseRead(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new InvalidArgumentException("Ошибка: не указан индекс. Пример: read 1");
        if (!int.TryParse(args.Trim(), out int readIndex))
            throw new InvalidArgumentException("Ошибка: индекс должен быть числом.");
        if (readIndex < 1 || readIndex > AppInfo.Todos.Count)
            throw new TaskNotFoundException("Задача с таким индексом не существует.");
        return new ReadCommand(readIndex);
    }

    private static ICommand ParseStatus(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new InvalidArgumentException("Ошибка: не указаны параметры. Пример: status 2 InProgress");
        string[] statusParts = args.Trim().Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
        if (statusParts.Length < 2)
            throw new InvalidArgumentException("Ошибка: неверный формат. Пример: status 2 InProgress");
        if (!int.TryParse(statusParts[0], out int statusIndex))
            throw new InvalidArgumentException("Ошибка: индекс должен быть числом.");
        if (statusIndex < 1 || statusIndex > AppInfo.Todos.Count)
            throw new TaskNotFoundException("Задача с таким индексом не существует.");
        string statusStr = statusParts[1].Trim();
        if (!TryParseStatus(statusStr, out TodoStatus status))
            throw new InvalidArgumentException("Ошибка: неизвестный статус. Возможные: NotStarted, InProgress, Completed, Postponed, Failed");
        return new StatusCommand(statusIndex, status);
    }

    private static ICommand ParseDelete(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new InvalidArgumentException("Ошибка: не указан индекс. Пример: delete 1");
        if (!int.TryParse(args.Trim(), out int deleteIndex))
            throw new InvalidArgumentException("Ошибка: индекс должен быть числом.");
        if (deleteIndex < 1 || deleteIndex > AppInfo.Todos.Count)
            throw new TaskNotFoundException("Задача с таким индексом не существует.");
        return new DeleteCommand(deleteIndex);
    }

    private static ICommand ParseUpdate(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new InvalidArgumentException("Ошибка: не указаны параметры. Пример: update 2 \"Новый текст\"");
        string[] updateParts = args.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (updateParts.Length < 2)
            throw new InvalidArgumentException("Ошибка: неверный формат. Пример: update 2 \"Новый текст\"");
        if (!int.TryParse(updateParts[0], out int updateIndex))
            throw new InvalidArgumentException("Ошибка: индекс должен быть числом.");
        if (updateIndex < 1 || updateIndex > AppInfo.Todos.Count)
            throw new TaskNotFoundException("Задача с таким индексом не существует.");
        string newText = updateParts[1].Trim().Trim('"');
        return new UpdateCommand(updateIndex, newText);
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

