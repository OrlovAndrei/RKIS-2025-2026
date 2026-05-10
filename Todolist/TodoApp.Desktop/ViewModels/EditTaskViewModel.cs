using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public sealed partial class EditTaskViewModel : ObservableObject
{
    private readonly ProfileRepository _profileRepository;
    private readonly TodoRepository _todoRepository;
    private readonly NavigationService _navigation;
    private readonly ProfileSessionService _session;
    private readonly TodoItem _task;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string _text;

    [ObservableProperty]
    private TodoStatus _selectedStatus;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public EditTaskViewModel(
        ProfileRepository profileRepository,
        TodoRepository todoRepository,
        NavigationService navigation,
        ProfileSessionService session,
        TodoItem task)
    {
        _profileRepository = profileRepository;
        _todoRepository = todoRepository;
        _navigation = navigation;
        _session = session;
        _task = task;
        _text = task.Text;
        _selectedStatus = task.Status;
        Statuses = Enum.GetValues<TodoStatus>().ToList();
    }

    public string Title => "Редактирование задачи";

    public IReadOnlyList<TodoStatus> Statuses { get; }

    public bool CanSave => !string.IsNullOrWhiteSpace(Text);

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        try
        {
            _task.Text = Text.Trim();
            _task.Status = SelectedStatus;
            _todoRepository.Update(_task);
            NavigateToList();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        NavigateToList();
    }

    private void NavigateToList()
    {
        _navigation.Navigate(new TodoListViewModel(_profileRepository, _todoRepository, _navigation, _session));
    }
}
