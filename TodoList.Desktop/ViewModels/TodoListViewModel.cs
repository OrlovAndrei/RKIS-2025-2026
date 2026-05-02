using System.Collections.ObjectModel;
using System.Windows.Input;
using TodoList.Models;
using TodoListDesktop.Services;

namespace TodoListDesktop.ViewModels;

public sealed class TodoListViewModel : ViewModelBase
{
    private readonly TodoTaskService _taskService;
    private readonly Action _onAddRequested;
    private readonly Action<TodoItemViewModel> _onEditRequested;
    private readonly Action _onLogoutRequested;
    private readonly List<TodoItem> _allTasks = new();
    private TodoItemViewModel? _selectedTask;
    private TodoStatus? _selectedStatus;
    private string _searchText = "";
    private string _message = "Загрузка задач...";
    private bool _isBusy;

    public TodoListViewModel(
        TodoTaskService taskService,
        Action onAddRequested,
        Action<TodoItemViewModel> onEditRequested,
        Action onLogoutRequested)
    {
        _taskService = taskService;
        _onAddRequested = onAddRequested;
        _onEditRequested = onEditRequested;
        _onLogoutRequested = onLogoutRequested;

        StatusOptions = new TodoStatus?[] { null }
            .Concat(Enum.GetValues<TodoStatus>().Select(status => (TodoStatus?)status))
            .ToArray();

        RefreshCommand = new RelayCommand(LoadTasks, () => !IsBusy);
        AddTaskCommand = new RelayCommand(_onAddRequested, () => !IsBusy);
        EditTaskCommand = new RelayCommand(EditSelectedTask, () => SelectedTask != null && !IsBusy);
        DeleteTaskCommand = new RelayCommand(DeleteSelectedTask, () => SelectedTask != null && !IsBusy);
        LogoutCommand = new RelayCommand(_onLogoutRequested, () => !IsBusy);

        LoadTasks();
    }

    public ObservableCollection<TodoItemViewModel> Tasks { get; } = new();

    public TodoStatus?[] StatusOptions { get; }

    public ICommand RefreshCommand { get; }

    public ICommand AddTaskCommand { get; }

    public ICommand EditTaskCommand { get; }

    public ICommand DeleteTaskCommand { get; }

    public ICommand LogoutCommand { get; }

    public string CurrentUserName => _taskService.CurrentProfile == null
        ? "Профиль"
        : $"{_taskService.CurrentProfile.FirstName} {_taskService.CurrentProfile.LastName}";

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                ApplyFilters();
            }
        }
    }

    public TodoStatus? SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            if (SetProperty(ref _selectedStatus, value))
            {
                ApplyFilters();
            }
        }
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

    public string Message
    {
        get => _message;
        private set => SetProperty(ref _message, value);
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

    private async void LoadTasks()
    {
        try
        {
            IsBusy = true;
            var tasks = await _taskService.GetTasksAsync();
            _allTasks.Clear();
            _allTasks.AddRange(tasks);
            ApplyFilters();
        }
        catch (Exception ex)
        {
            Message = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ApplyFilters()
    {
        var query = _allTasks.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            query = query.Where(task =>
                task.Text.Contains(SearchText.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        if (SelectedStatus != null)
        {
            query = query.Where(task => task.Status == SelectedStatus);
        }

        Tasks.Clear();
        foreach (var task in query.OrderBy(task => task.Id))
        {
            Tasks.Add(new TodoItemViewModel(task, _taskService));
        }

        Message = Tasks.Count == 0
            ? "Задачи не найдены."
            : $"Показано задач: {Tasks.Count}";
    }

    private void EditSelectedTask()
    {
        if (SelectedTask != null)
        {
            _onEditRequested(SelectedTask);
        }
    }

    private async void DeleteSelectedTask()
    {
        if (SelectedTask == null)
        {
            return;
        }

        try
        {
            IsBusy = true;
            await _taskService.DeleteTaskAsync(SelectedTask.Id);
            _allTasks.RemoveAll(task => task.Id == SelectedTask.Id);
            SelectedTask = null;
            ApplyFilters();
        }
        catch (Exception ex)
        {
            Message = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void RaiseCommandState()
    {
        ((RelayCommand)RefreshCommand).RaiseCanExecuteChanged();
        ((RelayCommand)AddTaskCommand).RaiseCanExecuteChanged();
        ((RelayCommand)EditTaskCommand).RaiseCanExecuteChanged();
        ((RelayCommand)DeleteTaskCommand).RaiseCanExecuteChanged();
        ((RelayCommand)LogoutCommand).RaiseCanExecuteChanged();
    }
}
