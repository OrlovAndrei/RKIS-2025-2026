using System.Collections.ObjectModel;
using System.Windows.Input;
using TodoList.Models;
using TodoListWpf.Services;

namespace TodoListWpf.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    private readonly TodoTaskService _taskService;
    private string _newTaskText = "";
    private string _statusMessage = "Загрузка задач...";
    private TodoItemViewModel? _selectedTask;
    private bool _isBusy;

    public MainWindowViewModel(TodoTaskService taskService)
    {
        _taskService = taskService;
        StatusOptions = Enum.GetValues<TodoStatus>();
        AddTaskCommand = new RelayCommand(AddTask, CanAddTask);
        DeleteTaskCommand = new RelayCommand(DeleteSelectedTask, () => SelectedTask != null && !IsBusy);
        RefreshCommand = new RelayCommand(LoadTasks, () => !IsBusy);

        LoadTasks();
    }

    public ObservableCollection<TodoItemViewModel> Tasks { get; } = new();

    public TodoStatus[] StatusOptions { get; }

    public ICommand AddTaskCommand { get; }

    public ICommand DeleteTaskCommand { get; }

    public ICommand RefreshCommand { get; }

    public string NewTaskText
    {
        get => _newTaskText;
        set
        {
            if (SetProperty(ref _newTaskText, value))
            {
                RaiseCommandState();
            }
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        private set => SetProperty(ref _statusMessage, value);
    }

    public TodoItemViewModel? SelectedTask
    {
        get => _selectedTask;
        set
        {
            if (SetProperty(ref _selectedTask, value))
            {
                RaiseCommandState();
            }
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (SetProperty(ref _isBusy, value))
            {
                RaiseCommandState();
            }
        }
    }

    private bool CanAddTask()
    {
        return !IsBusy && !string.IsNullOrWhiteSpace(NewTaskText);
    }

    private void LoadTasks()
    {
        RunSafely(async () =>
        {
            var tasks = await _taskService.GetTasksAsync();

            Tasks.Clear();
            foreach (var task in tasks)
            {
                Tasks.Add(new TodoItemViewModel(task, _taskService));
            }

            StatusMessage = Tasks.Count == 0
                ? "Список задач пуст. Добавьте первую задачу."
                : $"Загружено задач: {Tasks.Count}";
        });
    }

    private void AddTask()
    {
        RunSafely(async () =>
        {
            var task = await _taskService.AddTaskAsync(NewTaskText.Trim());
            Tasks.Add(new TodoItemViewModel(task, _taskService));
            NewTaskText = "";
            StatusMessage = "Задача добавлена.";
        });
    }

    private void DeleteSelectedTask()
    {
        if (SelectedTask == null)
        {
            return;
        }

        var taskToDelete = SelectedTask;

        RunSafely(async () =>
        {
            await _taskService.DeleteTaskAsync(taskToDelete.Id);
            Tasks.Remove(taskToDelete);
            SelectedTask = null;
            StatusMessage = "Задача удалена.";
        });
    }

    private async void RunSafely(Func<Task> action)
    {
        try
        {
            IsBusy = true;
            await action();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void RaiseCommandState()
    {
        ((RelayCommand)AddTaskCommand).RaiseCanExecuteChanged();
        ((RelayCommand)DeleteTaskCommand).RaiseCanExecuteChanged();
        ((RelayCommand)RefreshCommand).RaiseCanExecuteChanged();
    }
}
