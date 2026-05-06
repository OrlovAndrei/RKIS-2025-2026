using System.Windows.Input;
using TodoListDesktop.Services;

namespace TodoListDesktop.ViewModels;

public sealed class AddTaskViewModel : ViewModelBase
{
    private readonly TodoTaskService _taskService;
    private readonly Action _onSaved;
    private readonly Action _onCanceled;
    private string _text = "";
    private string _message = "";
    private bool _isBusy;

    public AddTaskViewModel(TodoTaskService taskService, Action onSaved, Action onCanceled)
    {
        _taskService = taskService;
        _onSaved = onSaved;
        _onCanceled = onCanceled;
        SaveCommand = new RelayCommand(Save, CanSave);
        CancelCommand = new RelayCommand(_onCanceled, () => !IsBusy);
    }

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
            await _taskService.AddTaskAsync(Text.Trim());
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
