using System.Windows.Input;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private string _login = "";
    private string _password = "";
    private string _errorMessage = "";
    private readonly ProfileRepository _profileRepo;

    public string Login { get => _login; set { _login = value; OnPropertyChanged(); } }
    public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
    public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }

    public ICommand LoginCommand { get; }
    public ICommand GoToRegisterCommand { get; }

    public event Action<bool, Profile?>? LoginCompleted;

    public LoginViewModel()
    {
        _profileRepo = new ProfileRepository();
        LoginCommand = new RelayCommand(_ => ExecuteLogin());
        GoToRegisterCommand = new RelayCommand(_ => LoginCompleted?.Invoke(false, null));
    }

    private void ExecuteLogin()
    {
        var profile = _profileRepo.GetByLogin(Login);
        if (profile == null || !profile.CheckPassword(Password))
        {
            ErrorMessage = "Неверный логин или пароль";
            return;
        }
        LoginCompleted?.Invoke(true, profile);
    }
}