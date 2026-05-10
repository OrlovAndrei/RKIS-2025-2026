using System.Windows.Input;
using TodoList.Models;
using TodoListDesktop.Services;

namespace TodoListDesktop.ViewModels;

public sealed class EditTaskViewModel : ViewModelBase
{
    private readonly TodoTaskService _taskService;
    private readonly TodoItemViewModel _task;
    private readonly Action _onSaved;
    private readonly Action _onCanceled;
    private string _text;
    private TodoStatus _status;
    private string _message = "";
    private bool _isBusy;

    public EditTaskViewModel(
        TodoTaskService taskService,
        TodoItemViewModel task,
        Action onSaved,
        Action onCanceled)
    {
        _taskService = taskService;
        _task = task;
        _onSaved = onSaved;
        _onCanceled = onCanceled;
        _text = task.Text;
        _status = task.Status;
        StatusOptions = Enum.GetValues<TodoStatus>();
        SaveCommand = new RelayCommand(Save, CanSave);
        CancelCommand = new RelayCommand(_onCanceled, () => !IsBusy);
    }

    public TodoStatus[] StatusOptions { get; }

    public ICommand SaveCommand { get; }

    public ICommand CancelCommand { get; }

    public string Text
    {
        get => _text;
        set
        {
            if (SetProperty(ref _text, value))
            {
                RaiseCommandState();
            }
        }
    }

    public TodoStatus Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    public string Message
    {
        get => _message;
        private set => SetProperty(ref _message, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (SetProperty(ref _isBusy, value))
            {
                RaiseCommandState();
            }
        }
    }

    private bool CanSave()
    {
        return !IsBusy && !string.IsNullOrWhiteSpace(Text);
    }

    private async void Save()
    {
        try
        {
            IsBusy = true;
            await _taskService.UpdateTaskAsync(_task.Id, Text.Trim(), Status);
            _onSaved();
        }
        catch (Exception ex)
        {
            Message = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void RaiseCommandState()
    {
        ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
        ((RelayCommand)CancelCommand).RaiseCanExecuteChanged();
    }
}
