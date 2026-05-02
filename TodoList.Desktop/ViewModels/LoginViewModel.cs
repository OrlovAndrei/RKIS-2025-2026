using System.Windows.Input;
using TodoListDesktop.Services;

namespace TodoListDesktop.ViewModels;

public sealed class LoginViewModel : ViewModelBase
{
    private readonly TodoTaskService _taskService;
    private readonly Action _onLoggedIn;
    private readonly Action _onRegisterRequested;
    private string _email = "";
    private string _password = "";
    private string _message = "";
    private bool _isBusy;

    public LoginViewModel(TodoTaskService taskService, Action onLoggedIn, Action onRegisterRequested)
    {
        _taskService = taskService;
        _onLoggedIn = onLoggedIn;
        _onRegisterRequested = onRegisterRequested;
        LoginCommand = new RelayCommand(Login, CanLogin);
        ShowRegisterCommand = new RelayCommand(_onRegisterRequested, () => !IsBusy);
    }

    public ICommand LoginCommand { get; }

    public ICommand ShowRegisterCommand { get; }

    public string Email
    {
        get => _email;
        set
        {
            if (SetProperty(ref _email, value))
            {
                RaiseCommandState();
            }
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (SetProperty(ref _password, value))
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

    private bool CanLogin()
    {
        return !IsBusy &&
               !string.IsNullOrWhiteSpace(Email) &&
               !string.IsNullOrWhiteSpace(Password);
    }

    private async void Login()
    {
        try
        {
            IsBusy = true;
            await _taskService.LoginAsync(Email, Password);
            _onLoggedIn();
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
        ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
        ((RelayCommand)ShowRegisterCommand).RaiseCanExecuteChanged();
    }
}
