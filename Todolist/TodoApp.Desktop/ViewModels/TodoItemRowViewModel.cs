using CommunityToolkit.Mvvm.ComponentModel;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public sealed partial class TodoItemRowViewModel : ObservableObject
{
    public TodoItemRowViewModel(TodoItem todo)
    {
        Todo = todo;
    }

    public TodoItem Todo { get; }

    public Guid Id => Todo.Id;

    public string Text => Todo.Text;

    public TodoStatus Status => Todo.Status;

    public string StatusDisplay => Todo.Status.ToDisplayName();

    public DateTime LastUpdate => Todo.LastUpdate;
}
