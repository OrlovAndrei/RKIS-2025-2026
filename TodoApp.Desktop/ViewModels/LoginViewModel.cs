using System.Windows.Input;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels;

public class LoginViewModel : ViewModelBase
{
	private readonly NavigationService _navigationService;
	private string _login = string.Empty;
	private string _password = string.Empty;
	private string _statusMessage = "Введите логин и пароль.";

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
		StatusMessage = "Логика авторизации будет подключена на следующем этапе.";
		_navigationService.Navigate(new TodoListViewModel(_navigationService));
	}

	private void OpenRegister()
	{
		_navigationService.Navigate(new RegisterViewModel(_navigationService));
	}
}
