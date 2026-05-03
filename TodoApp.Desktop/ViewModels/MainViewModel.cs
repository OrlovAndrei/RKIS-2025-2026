using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels;

public class MainViewModel : ViewModelBase
{
	private readonly NavigationService _navigationService;
	private ViewModelBase? _currentViewModel;

	public MainViewModel()
	{
		_navigationService = new NavigationService();
		_navigationService.CurrentViewModelChanged += viewModel =>
		{
			CurrentViewModel = viewModel;
		};

		ShowLogin();
	}

	public ViewModelBase? CurrentViewModel
	{
		get => _currentViewModel;
		private set => SetProperty(ref _currentViewModel, value);
	}

	public void ShowLogin()
	{
		_navigationService.Navigate(new LoginViewModel(_navigationService));
	}

	public void ShowRegister()
	{
		_navigationService.Navigate(new RegisterViewModel(_navigationService));
	}

	public void ShowTodoList()
	{
		_navigationService.Navigate(new TodoListViewModel(_navigationService));
	}
}
