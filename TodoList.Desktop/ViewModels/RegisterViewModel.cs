using System.Windows.Input;
using TodoListDesktop.Services;

namespace TodoListDesktop.ViewModels;

public sealed class RegisterViewModel : ViewModelBase
{
    private readonly TodoTaskService _taskService;
    private readonly Action _onRegistered;
    private readonly Action _onLoginRequested;
    private string _username = "";
    private string _email = "";
    private string _password = "";
    private string _firstName = "";
    private string _lastName = "";
    private string _birthYear = "";
    private string _message = "";
    private bool _isBusy;

    public RegisterViewModel(TodoTaskService taskService, Action onRegistered, Action onLoginRequested)
    {
        _taskService = taskService;
        _onRegistered = onRegistered;
        _onLoginRequested = onLoginRequested;
        RegisterCommand = new RelayCommand(Register, CanRegister);
        ShowLoginCommand = new RelayCommand(_onLoginRequested, () => !IsBusy);
    }

    public ICommand RegisterCommand { get; }

    public ICommand ShowLoginCommand { get; }

    public string Username
    {
        get => _username;
        set
        {
            if (SetProperty(ref _username, value))
            {
                RaiseCommandState();
            }
        }
    }

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

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (SetProperty(ref _firstName, value))
            {
                RaiseCommandState();
            }
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (SetProperty(ref _lastName, value))
            {
                RaiseCommandState();
            }
        }
    }

    public string BirthYear
    {
        get => _birthYear;
        set
        {
            if (SetProperty(ref _birthYear, value))
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

    private bool CanRegister()
    {
        return !IsBusy &&
               !string.IsNullOrWhiteSpace(Username) &&
               !string.IsNullOrWhiteSpace(Email) &&
               !string.IsNullOrWhiteSpace(Password) &&
               !string.IsNullOrWhiteSpace(FirstName) &&
               !string.IsNullOrWhiteSpace(LastName) &&
               int.TryParse(BirthYear, out _);
    }

    private async void Register()
    {
        try
        {
            IsBusy = true;
            if (!int.TryParse(BirthYear, out var parsedBirthYear))
            {
                throw new ArgumentException("Введите корректный год рождения.");
            }

            await _taskService.RegisterAsync(Username, Email, Password, FirstName, LastName, parsedBirthYear);
            _onRegistered();
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
        ((RelayCommand)RegisterCommand).RaiseCanExecuteChanged();
        ((RelayCommand)ShowLoginCommand).RaiseCanExecuteChanged();
    }
}
