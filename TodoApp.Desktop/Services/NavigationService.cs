using TodoApp.Desktop.ViewModels;

namespace TodoApp.Desktop.Services;

public class NavigationService
{
	public event Action<ViewModelBase>? CurrentViewModelChanged;

	private ViewModelBase? _currentViewModel;
	public ViewModelBase? CurrentViewModel
	{
		get => _currentViewModel;
		private set
		{
			_currentViewModel = value;
			if (value != null)
			{
				CurrentViewModelChanged?.Invoke(value);
			}
		}
	}

	public void Navigate(ViewModelBase viewModel)
	{
		CurrentViewModel = viewModel;
	}
}
