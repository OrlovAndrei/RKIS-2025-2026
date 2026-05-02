using TodoList.Models;
using TodoListDesktop.Services;

namespace TodoListDesktop.ViewModels;

public sealed class TodoItemViewModel : ViewModelBase
{
    private readonly TodoTaskService _taskService;
    private readonly TodoItem _item;
    private TodoStatus _status;

    public TodoItemViewModel(TodoItem item, TodoTaskService taskService)
    {
        _item = item;
        _taskService = taskService;
        _status = item.Status;
    }

    public int Id => _item.Id;

    public string Text => _item.Text;

    public DateTime LastUpdate => _item.LastUpdate;

    public TodoStatus Status
    {
        get => _status;
        set
        {
            if (!SetProperty(ref _status, value))
            {
                return;
            }

            _item.Status = value;
            _item.LastUpdate = DateTime.Now;
            OnPropertyChanged(nameof(LastUpdate));
            _ = _taskService.UpdateStatusAsync(_item.Id, value);
        }
    }
}
