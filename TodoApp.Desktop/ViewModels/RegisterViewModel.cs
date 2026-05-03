using System.Windows.Input;
using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels;

public class RegisterViewModel : ViewModelBase
{
	private readonly NavigationService _navigationService;
	private string _login = string.Empty;
	private string _password = string.Empty;
	private string _firstName = string.Empty;
	private string _lastName = string.Empty;
	private int _birthYear = DateTime.Today.Year;
	private string _statusMessage = "Заполните данные для регистрации.";

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
		StatusMessage = "Логика регистрации будет подключена на следующем этапе.";
		_navigationService.Navigate(new TodoListViewModel(_navigationService));
	}

	private void BackToLogin()
	{
		_navigationService.Navigate(new LoginViewModel(_navigationService));
	}
}
