using System;
using Todolist.Commands;

static class CommandParser
{
    public static ICommand? Parse(string inputString)
    {
        if (string.IsNullOrWhiteSpace(inputString))
        {
            return null;
        }

        string[] parts = inputString.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string command = parts.Length > 0 ? parts[0].ToLower() : string.Empty;
        string args = parts.Length > 1 ? parts[1] : string.Empty;

        switch (command)
        {
            case "profile":
                return new ProfileCommand();

            case "add":
                return new AddCommand(args);

            case "view":
                return new ViewCommand(args);

            case "read":
                if (TryParseIndex(args, AppInfo.Todos.Count, out int readIndex))
                {
                    return new ReadCommand(readIndex);
                }
                return null;

            case "status":
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
                                Console.WriteLine("Ошибка: неверный статус. Доступные статусы: NotStarted, InProgress, Completed, Postponed, Failed");
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
                    Console.WriteLine("Ошибка: укажите индекс и статус. Пример: status 2 InProgress");
                }
                return null;

            case "delete":
                if (TryParseIndex(args, AppInfo.Todos.Count, out int deleteIndex))
                {
                    return new DeleteCommand(deleteIndex);
                }
                return null;

            case "update":
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
                    Console.WriteLine("Ошибка: укажите индекс и новый текст. Пример: update 2 \"Новый текст\"");
                }
                return null;

            default:
                return null;
        }
    }

    private static bool TryParseIndex(string arg, int taskCount, out int indexOneBased)
    {
        indexOneBased = -1;
        if (string.IsNullOrWhiteSpace(arg))
        {
            Console.WriteLine("Ошибка: укажите индекс задачи.");
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
        
        // Попытка парсинга с учетом регистра и без
        if (Enum.TryParse<TodoStatus>(statusStr, true, out TodoStatus parsedStatus))
        {
            status = parsedStatus;
            return true;
        }
        
        // Также поддерживаем русские названия для удобства
        string statusLower = statusStr.ToLowerInvariant();
        switch (statusLower)
        {
            case "notstarted":
            case "не начато":
                status = TodoStatus.NotStarted;
                return true;
            case "inprogress":
            case "в процессе":
                status = TodoStatus.InProgress;
                return true;
            case "completed":
            case "выполнено":
            case "done":
                status = TodoStatus.Completed;
                return true;
            case "postponed":
            case "отложено":
                status = TodoStatus.Postponed;
                return true;
            case "failed":
            case "провалено":
                status = TodoStatus.Failed;
                return true;
            default:
                return false;
        }
    }
}
