using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TodoApp.Data;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public sealed partial class AddTaskViewModel : ObservableObject
{
    private readonly ProfileRepository _profileRepository;
    private readonly TodoRepository _todoRepository;
    private readonly NavigationService _navigation;
    private readonly ProfileSessionService _session;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string _text = string.Empty;

    [ObservableProperty]
    private TodoStatus _selectedStatus = TodoStatus.NotStarted;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public AddTaskViewModel(
        ProfileRepository profileRepository,
        TodoRepository todoRepository,
        NavigationService navigation,
        ProfileSessionService session)
    {
        _profileRepository = profileRepository;
        _todoRepository = todoRepository;
        _navigation = navigation;
        _session = session;
        Statuses = Enum.GetValues<TodoStatus>().ToList();
    }

    public string Title => "Добавление задачи";

    public IReadOnlyList<TodoStatus> Statuses { get; }

    public bool CanSave => !string.IsNullOrWhiteSpace(Text);

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        try
        {
            var task = new TodoItem(Text.Trim())
            {
                ProfileId = _session.RequireProfile().Id,
                Status = SelectedStatus
            };

            _todoRepository.Add(task);
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
