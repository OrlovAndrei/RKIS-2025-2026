using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly Profile _currentProfile;
    private readonly TodoRepository _todoRepo;
    private ObservableCollection<TodoItem> _tasks = new();
    private TodoItem? _selectedTask;
    private string _searchText = "";
    private TodoStatus? _selectedStatusFilter;
    private string _statusMessage = "";

    public ObservableCollection<TodoItem> Tasks { get => _tasks; set { _tasks = value; OnPropertyChanged(); } }
    public TodoItem? SelectedTask { get => _selectedTask; set { _selectedTask = value; OnPropertyChanged(); } }
    public string SearchText { get => _searchText; set { _searchText = value; OnPropertyChanged(); LoadTasks(); } }
    public TodoStatus? SelectedStatusFilter { get => _selectedStatusFilter; set { _selectedStatusFilter = value; OnPropertyChanged(); LoadTasks(); } }
    public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(); } }

    public ICommand LoadTasksCommand { get; }
    public ICommand AddTaskCommand { get; }
    public ICommand EditTaskCommand { get; }
    public ICommand DeleteTaskCommand { get; }
    public ICommand ChangeStatusCommand { get; }
    public ICommand LogoutCommand { get; }

    public event Action? LogoutRequested;

    public MainViewModel(Profile profile)
    {
        _currentProfile = profile;
        _todoRepo = new TodoRepository();

        LoadTasksCommand = new RelayCommand(_ => LoadTasks());
        AddTaskCommand = new RelayCommand(_ => AddTask());
        EditTaskCommand = new RelayCommand(_ => EditTask(), _ => SelectedTask != null);
        DeleteTaskCommand = new RelayCommand(_ => DeleteTask(), _ => SelectedTask != null);
        ChangeStatusCommand = new RelayCommand(param => ChangeStatus(param), _ => SelectedTask != null);
        LogoutCommand = new RelayCommand(_ => Logout());

        LoadTasks();
    }

    private void LoadTasks()
    {
        var all = _todoRepo.GetAllForUser(_currentProfile.Id);
        var filtered = all.Where(t =>
            (string.IsNullOrEmpty(SearchText) || t.Text.Contains(SearchText)) &&
            (SelectedStatusFilter == null || t.Status == SelectedStatusFilter));
        Tasks.Clear();
        foreach (var task in filtered)
            Tasks.Add(task);
        StatusMessage = $"Задач: {Tasks.Count}";
    }

    private void AddTask()
    {
        var addVM = new AddTaskViewModel(_currentProfile.Id);
        var addWindow = new Views.AddTaskWindow { DataContext = addVM };
        addVM.CloseRequest += (saved) => { if (saved) LoadTasks(); };
        addWindow.ShowDialog();
    }

    private void EditTask()
    {
        if (SelectedTask == null) return;
        var editVM = new EditTaskViewModel(SelectedTask);
        var editWindow = new Views.EditTaskWindow(SelectedTask); // передаём задачу в конструктор окна
        editWindow.ShowDialog();
        LoadTasks();
    }

    private void DeleteTask()
    {
        if (SelectedTask == null) return;
        var result = MessageBox.Show($"Удалить задачу \"{SelectedTask.Text}\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            _todoRepo.Delete(SelectedTask.Id);
            LoadTasks();
        }
    }

    private void ChangeStatus(object? param)
    {
        if (SelectedTask == null || param is not TodoStatus newStatus) return;
        _todoRepo.SetStatus(SelectedTask.Id, newStatus);
        LoadTasks();
    }

    private void Logout()
    {
        LogoutRequested?.Invoke();
    }
}