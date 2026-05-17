using System.Windows.Input;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public class RegisterViewModel : ViewModelBase
{
    private string _login = "";
    private string _password = "";
    private string _firstName = "";
    private string _lastName = "";
    private int _birthYear = 2000;
    private string _errorMessage = "";
    private readonly ProfileRepository _profileRepo;

    public string Login { get => _login; set { _login = value; OnPropertyChanged(); } }
    public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
    public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(); } }
    public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(); } }
    public int BirthYear { get => _birthYear; set { _birthYear = value; OnPropertyChanged(); } }
    public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }

    public ICommand RegisterCommand { get; }
    public ICommand GoToLoginCommand { get; }

    public event Action<bool, Profile?>? RegisterCompleted;

    public RegisterViewModel()
    {
        _profileRepo = new ProfileRepository();
        RegisterCommand = new RelayCommand(_ => ExecuteRegister());
        GoToLoginCommand = new RelayCommand(_ => RegisterCompleted?.Invoke(false, null));
    }

    private void ExecuteRegister()
    {
        if (string.IsNullOrWhiteSpace(Login))
        {
            ErrorMessage = "Логин не может быть пустым";
            return;
        }
        if (_profileRepo.GetByLogin(Login) != null)
        {
            ErrorMessage = "Пользователь с таким логином уже существует";
            return;
        }
        if (string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Пароль не может быть пустым";
            return;
        }
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
        {
            ErrorMessage = "Имя и фамилия обязательны";
            return;
        }
        if (BirthYear < 1900 || BirthYear > DateTime.Now.Year)
        {
            ErrorMessage = "Некорректный год рождения";
            return;
        }

        var profile = new Profile(Login, Password, FirstName, LastName, BirthYear);
        _profileRepo.Add(profile);
        RegisterCompleted?.Invoke(true, profile);
    }
}