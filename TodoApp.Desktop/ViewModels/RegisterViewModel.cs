using System.Windows.Input;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public class RegisterViewModel : ViewModelBase
{
	private readonly NavigationService _navigationService;
	private string _login = string.Empty;
	private string _password = string.Empty;
	private string _firstName = string.Empty;
	private string _lastName = string.Empty;
	private int _birthYear = DateTime.Today.Year;
	private string _statusMessage = "Создай новый профиль для входа в систему.";

	public RegisterViewModel(NavigationService navigationService)
	{
		_navigationService = navigationService;
		RegisterCommand = new RelayCommand(ExecuteRegister);
		BackToLoginCommand = new RelayCommand(BackToLogin);
	}

	public string Login
	{
		get => _login;
		set => SetProperty(ref _login, value);
	}

	public string Password
	{
		get => _password;
		set => SetProperty(ref _password, value);
	}

	public string FirstName
	{
		get => _firstName;
		set => SetProperty(ref _firstName, value);
	}

	public string LastName
	{
		get => _lastName;
		set => SetProperty(ref _lastName, value);
	}

	public int BirthYear
	{
		get => _birthYear;
		set => SetProperty(ref _birthYear, value);
	}

	public string StatusMessage
	{
		get => _statusMessage;
		set => SetProperty(ref _statusMessage, value);
	}

	public ICommand RegisterCommand { get; }
	public ICommand BackToLoginCommand { get; }

	private void ExecuteRegister()
	{
		if (string.IsNullOrWhiteSpace(Login) ||
			string.IsNullOrWhiteSpace(Password) ||
			string.IsNullOrWhiteSpace(FirstName))
		{
			StatusMessage = "Логин, пароль и имя обязательны.";
			return;
		}

		bool exists = _navigationService.State.Profiles.Any(profile =>
			profile.Login.Equals(Login, StringComparison.OrdinalIgnoreCase));

		if (exists)
		{
			StatusMessage = "Такой логин уже существует.";
			return;
		}

		var profile = new Profile(Login, Password, FirstName, LastName, BirthYear);
		_navigationService.State.Profiles.Add(profile);
		_navigationService.State.CurrentProfile = profile;
		_navigationService.ShowTodoList();
	}

	private void BackToLogin()
	{
		_navigationService.ShowLogin();
	}
}
