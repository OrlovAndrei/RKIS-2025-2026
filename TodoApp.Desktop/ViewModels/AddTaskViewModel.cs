using System.Collections.ObjectModel;
using System.Windows.Input;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public class AddTaskViewModel : ViewModelBase
{
	private readonly NavigationService _navigationService;
	private string _taskText = string.Empty;
	private TodoStatus _selectedStatus = TodoStatus.NotStarted;

	public AddTaskViewModel(NavigationService navigationService)
	{
		_navigationService = navigationService;
		AvailableStatuses = new ObservableCollection<TodoStatus>(Enum.GetValues<TodoStatus>());
		SaveCommand = new RelayCommand(SaveTask);
		CancelCommand = new RelayCommand(Cancel);
	}

	public string TaskText
	{
		get => _taskText;
		set => SetProperty(ref _taskText, value);
	}

	public TodoStatus SelectedStatus
	{
		get => _selectedStatus;
		set => SetProperty(ref _selectedStatus, value);
	}

	public ObservableCollection<TodoStatus> AvailableStatuses { get; }

	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }

	private void SaveTask()
	{
		_navigationService.Navigate(new TodoListViewModel(_navigationService));
	}

	private void Cancel()
	{
		_navigationService.Navigate(new TodoListViewModel(_navigationService));
	}
}
