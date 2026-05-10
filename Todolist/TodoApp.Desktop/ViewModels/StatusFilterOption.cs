using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public sealed class StatusFilterOption
{
    public StatusFilterOption(string displayName, TodoStatus? status)
    {
        DisplayName = displayName;
        Status = status;
    }

    public string DisplayName { get; }

    public TodoStatus? Status { get; }
}
