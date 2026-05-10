using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public sealed partial class TodoListViewModel : ObservableObject
{
    private readonly ProfileRepository _profileRepository;
    private readonly TodoRepository _todoRepository;
    private readonly NavigationService _navigation;
    private readonly ProfileSessionService _session;
    private readonly List<TodoItem> _allTasks = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private StatusFilterOption _selectedStatusFilter;

    [ObservableProperty]
    private DateTime? _dateFrom;

    [ObservableProperty]
    private DateTime? _dateTo;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    [NotifyCanExecuteChangedFor(nameof(ChangeStatusCommand))]
    private TodoItemRowViewModel? _selectedTask;

    [ObservableProperty]
    private TodoStatus _selectedNewStatus = TodoStatus.NotStarted;

    [ObservableProperty]
    private string _message = string.Empty;

    public TodoListViewModel(
        ProfileRepository profileRepository,
        TodoRepository todoRepository,
        NavigationService navigation,
        ProfileSessionService session)
    {
        _profileRepository = profileRepository;
        _todoRepository = todoRepository;
        _navigation = navigation;
        _session = session;

        StatusFilters = new List<StatusFilterOption>
        {
            new("Все статусы", null),
            new(TodoStatus.NotStarted.ToDisplayName(), TodoStatus.NotStarted),
            new(TodoStatus.InProgress.ToDisplayName(), TodoStatus.InProgress),
            new(TodoStatus.Completed.ToDisplayName(), TodoStatus.Completed),
            new(TodoStatus.Postponed.ToDisplayName(), TodoStatus.Postponed),
            new(TodoStatus.Failed.ToDisplayName(), TodoStatus.Failed)
        };

        Statuses = Enum.GetValues<TodoStatus>().ToList();
        _selectedStatusFilter = StatusFilters[0];
        LoadTasks();
    }

    public ObservableCollection<TodoItemRowViewModel> Tasks { get; } = new();

    public IReadOnlyList<StatusFilterOption> StatusFilters { get; }

    public IReadOnlyList<TodoStatus> Statuses { get; }

    public string ProfileTitle => _session.RequireProfile().DisplayName;

    public bool HasSelectedTask => SelectedTask != null;

    partial void OnSelectedTaskChanged(TodoItemRowViewModel? value)
    {
        if (value != null)
        {
            SelectedNewStatus = value.Status;
        }
    }

    [RelayCommand]
    private void ApplyFilters()
    {
        IEnumerable<TodoItem> query = _allTasks;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            query = query.Where(todo => todo.Text.Contains(SearchText.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        if (SelectedStatusFilter.Status.HasValue)
        {
            query = query.Where(todo => todo.Status == SelectedStatusFilter.Status.Value);
        }

        if (DateFrom.HasValue)
        {
            query = query.Where(todo => todo.LastUpdate.Date >= DateFrom.Value.Date);
        }

        if (DateTo.HasValue)
        {
            query = query.Where(todo => todo.LastUpdate.Date <= DateTo.Value.Date);
        }

        Tasks.Clear();
        foreach (TodoItem todo in query.OrderBy(todo => todo.SortOrder))
        {
            Tasks.Add(new TodoItemRowViewModel(todo));
        }

        Message = Tasks.Count == 0 ? "Задачи не найдены." : $"Найдено задач: {Tasks.Count}.";
    }

    [RelayCommand]
    private void ResetFilters()
    {
        SearchText = string.Empty;
        SelectedStatusFilter = StatusFilters[0];
        DateFrom = null;
        DateTo = null;
        ApplyFilters();
    }

    [RelayCommand]
    private void Add()
    {
        _navigation.Navigate(new AddTaskViewModel(_profileRepository, _todoRepository, _navigation, _session));
    }

    [RelayCommand(CanExecute = nameof(HasSelectedTask))]
    private void Edit()
    {
        if (SelectedTask == null)
        {
            return;
        }

        _navigation.Navigate(new EditTaskViewModel(_profileRepository, _todoRepository, _navigation, _session, SelectedTask.Todo));
    }

    [RelayCommand(CanExecute = nameof(HasSelectedTask))]
    private void Delete()
    {
        if (SelectedTask == null)
        {
            return;
        }

        _todoRepository.Delete(SelectedTask.Id);
        LoadTasks();
    }

    [RelayCommand(CanExecute = nameof(HasSelectedTask))]
    private void ChangeStatus()
    {
        if (SelectedTask == null)
        {
            return;
        }

        _todoRepository.SetStatus(SelectedTask.Id, SelectedNewStatus);
        LoadTasks();
    }

    [RelayCommand]
    private void Refresh()
    {
        LoadTasks();
    }

    [RelayCommand]
    private void Logout()
    {
        _session.CurrentProfile = null;
        _navigation.Navigate(new LoginViewModel(_profileRepository, _todoRepository, _navigation, _session));
    }

    private void LoadTasks()
    {
        _allTasks.Clear();
        _allTasks.AddRange(_todoRepository.GetAll(_session.RequireProfile().Id));
        ApplyFilters();
    }
}
