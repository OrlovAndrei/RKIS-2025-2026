using System.Collections.ObjectModel;
using System.Windows.Input;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public class TodoListViewModel : ViewModelBase
{
	private readonly NavigationService _navigationService;
	private string _searchText = string.Empty;
	private TodoItem? _selectedTask;

	public TodoListViewModel(NavigationService navigationService)
	{
		_navigationService = navigationService;
		Tasks = new ObservableCollection<TodoItem>();

		OpenAddTaskCommand = new RelayCommand(OpenAddTask);
		OpenEditTaskCommand = new RelayCommand(OpenEditTask, () => SelectedTask != null);
		RemoveTaskCommand = new RelayCommand(RemoveSelectedTask, () => SelectedTask != null);
		RefreshCommand = new RelayCommand(RefreshTasks);
		LogoutCommand = new RelayCommand(Logout);
	}

	public ObservableCollection<TodoItem> Tasks { get; }

	public string SearchText
	{
		get => _searchText;
		set => SetProperty(ref _searchText, value);
	}

	public TodoItem? SelectedTask
	{
		get => _selectedTask;
		set
		{
			if (SetProperty(ref _selectedTask, value))
			{
				(OpenEditTaskCommand as RelayCommand)?.RaiseCanExecuteChanged();
				(RemoveTaskCommand as RelayCommand)?.RaiseCanExecuteChanged();
			}
		}
	}

	public ICommand OpenAddTaskCommand { get; }
	public ICommand OpenEditTaskCommand { get; }
	public ICommand RemoveTaskCommand { get; }
	public ICommand RefreshCommand { get; }
	public ICommand LogoutCommand { get; }

	private void OpenAddTask()
	{
		_navigationService.Navigate(new AddTaskViewModel(_navigationService));
	}

	private void OpenEditTask()
	{
		if (SelectedTask == null)
		{
			return;
		}

		_navigationService.Navigate(new EditTaskViewModel(_navigationService, SelectedTask));
	}

	private void RemoveSelectedTask()
	{
		if (SelectedTask == null)
		{
			return;
		}

		Tasks.Remove(SelectedTask);
		SelectedTask = null;
	}

	private void RefreshTasks()
	{
		OnPropertyChanged(nameof(Tasks));
	}

	private void Logout()
	{
		_navigationService.Navigate(new LoginViewModel(_navigationService));
	}
}
