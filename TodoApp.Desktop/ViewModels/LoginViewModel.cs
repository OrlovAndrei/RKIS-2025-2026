using System.Windows.Input;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels;

public class LoginViewModel : ViewModelBase
{
	private readonly NavigationService _navigationService;
	private string _login = string.Empty;
	private string _password = string.Empty;
	private string _statusMessage = "Для быстрого входа можно использовать demo / demo";

	public LoginViewModel(NavigationService navigationService)
	{
		_navigationService = navigationService;
		LoginCommand = new RelayCommand(ExecuteLogin);
		OpenRegisterCommand = new RelayCommand(OpenRegister);
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

	public string StatusMessage
	{
		get => _statusMessage;
		set => SetProperty(ref _statusMessage, value);
	}

	public ICommand LoginCommand { get; }
	public ICommand OpenRegisterCommand { get; }

	private void ExecuteLogin()
	{
		var profile = _navigationService.State.Profiles
			.FirstOrDefault(p => p.Login.Equals(Login, StringComparison.OrdinalIgnoreCase) && p.Password == Password);

		if (profile == null)
		{
			StatusMessage = "Пользователь не найден. Проверь логин и пароль.";
			return;
		}

		_navigationService.State.CurrentProfile = profile;
		StatusMessage = $"Добро пожаловать, {profile.FirstName}.";
		_navigationService.ShowTodoList();
	}

	private void OpenRegister()
	{
		_navigationService.ShowRegister();
	}
}
