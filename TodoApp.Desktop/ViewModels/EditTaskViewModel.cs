using System.Collections.ObjectModel;
using System.Windows.Input;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public class EditTaskViewModel : ViewModelBase
{
	private readonly NavigationService _navigationService;
	private readonly TodoItem _sourceTask;
	private string _taskText;
	private TodoStatus _selectedStatus;

	public EditTaskViewModel(NavigationService navigationService, TodoItem task)
	{
		_navigationService = navigationService;
		_sourceTask = task;
		_taskText = task.Text;
		_selectedStatus = task.Status;

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
		_sourceTask.Text = TaskText;
		_sourceTask.Status = SelectedStatus;
		_sourceTask.LastUpdated = DateTime.Now;
		_navigationService.Navigate(new TodoListViewModel(_navigationService));
	}

	private void Cancel()
	{
		_navigationService.Navigate(new TodoListViewModel(_navigationService));
	}
}
