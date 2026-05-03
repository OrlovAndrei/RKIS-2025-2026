using TodoApp.Desktop.Services;

namespace TodoApp.Desktop.ViewModels;

public class MainViewModel : ViewModelBase
{
	private readonly NavigationService _navigationService;
	private ViewModelBase? _currentViewModel;

	public MainViewModel()
	{
		var state = new DesktopStateService();
		_navigationService = new NavigationService(state);
		_navigationService.CurrentViewModelChanged += viewModel => CurrentViewModel = viewModel;
		_navigationService.ShowLogin();
	}

	public ViewModelBase? CurrentViewModel
	{
		get => _currentViewModel;
		private set => SetProperty(ref _currentViewModel, value);
	}
}
