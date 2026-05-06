using System;

static class TodoStatusHelper
{
    public static bool TryParse(string input, out TodoStatus status)
    {
        status = TodoStatus.NotStarted;
        if (string.IsNullOrWhiteSpace(input))
            return false;

        if (Enum.TryParse(input, true, out TodoStatus parsedStatus))
        {
            status = parsedStatus;
            return true;
        }

        string compact = input.Trim().ToLowerInvariant()
            .Replace(" ", string.Empty)
            .Replace("_", string.Empty)
            .Replace("-", string.Empty);

        switch (compact)
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

    public static string ToDisplayString(TodoStatus status)
    {
        return status switch
        {
            TodoStatus.NotStarted => "Не начата",
            TodoStatus.InProgress => "В работе",
            TodoStatus.Completed => "Завершена",
            TodoStatus.Postponed => "Отложена",
            TodoStatus.Failed => "Провалена",
            _ => "Неизвестно"
        };
    }
}
