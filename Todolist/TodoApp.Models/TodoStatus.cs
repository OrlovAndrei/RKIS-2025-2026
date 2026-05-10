namespace TodoApp.Models;

public enum TodoStatus
{
    NotStarted,
    InProgress,
    Completed,
    Postponed,
    Failed
}

public static class TodoStatusExtensions
{
    public static string ToDisplayName(this TodoStatus status)
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
