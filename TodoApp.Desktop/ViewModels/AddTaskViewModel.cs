using System.Windows;
using System.Windows.Input;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public class AddTaskViewModel : ViewModelBase
{
    private readonly Guid _profileId;
    private readonly TodoRepository _todoRepo;
    private string _taskText = "";
    private TodoStatus _selectedStatus = TodoStatus.NotStarted;
    private string _errorMessage = "";

    public string TaskText { get => _taskText; set { _taskText = value; OnPropertyChanged(); } }
    public TodoStatus SelectedStatus { get => _selectedStatus; set { _selectedStatus = value; OnPropertyChanged(); } }
    public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public event Action<bool>? CloseRequest;

    public AddTaskViewModel(Guid profileId)
    {
        _profileId = profileId;
        _todoRepo = new TodoRepository();
        SaveCommand = new RelayCommand(_ => Save());
        CancelCommand = new RelayCommand(_ => CloseRequest?.Invoke(false));
    }

    private void Save()
    {
        if (string.IsNullOrWhiteSpace(TaskText))
        {
            ErrorMessage = "Текст задачи не может быть пустым";
            return;
        }
        var newItem = new TodoItem(TaskText, _profileId) { Status = SelectedStatus };
        _todoRepo.Add(newItem);
        CloseRequest?.Invoke(true);
    }
}