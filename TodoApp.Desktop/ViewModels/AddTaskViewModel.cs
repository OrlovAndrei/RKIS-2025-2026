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
	private string _statusMessage = "Заполни форму новой задачи.";

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

	public string StatusMessage
	{
		get => _statusMessage;
		set => SetProperty(ref _statusMessage, value);
	}

	public ObservableCollection<TodoStatus> AvailableStatuses { get; }

	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }

	private void SaveTask()
	{
		if (string.IsNullOrWhiteSpace(TaskText))
		{
			StatusMessage = "Текст задачи не должен быть пустым.";
			return;
		}

		var state = _navigationService.State;
		var currentProfile = state.CurrentProfile;
		if (currentProfile == null)
		{
			StatusMessage = "Нужно сначала войти в профиль.";
			return;
		}

		int nextId = state.Tasks.Any() ? state.Tasks.Max(task => task.Id) + 1 : 1;
		state.Tasks.Add(new TodoItem
		{
			Id = nextId,
			Text = TaskText.Trim(),
			Status = SelectedStatus,
			CreatedAt = DateTime.Now,
			LastUpdated = DateTime.Now,
			ProfileId = currentProfile.Id
		});

		_navigationService.ShowTodoList();
	}

	private void Cancel()
	{
		_navigationService.ShowTodoList();
	}
}
