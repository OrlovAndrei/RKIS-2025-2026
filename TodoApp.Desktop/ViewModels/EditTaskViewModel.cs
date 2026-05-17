using System.Windows;
using System;
using System.Windows.Input;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public class EditTaskViewModel : ViewModelBase
{
    private readonly TodoRepository _todoRepo;
    private readonly TodoItem _originalTask;
    private string _taskText = "";
    private TodoStatus _selectedStatus;
    private string _errorMessage = "";

    public string TaskText { get => _taskText; set { _taskText = value; OnPropertyChanged(); } }
    public TodoStatus SelectedStatus { get => _selectedStatus; set { _selectedStatus = value; OnPropertyChanged(); } }
    public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public event Action<bool>? CloseRequest;

    public EditTaskViewModel(TodoItem task)
    {
        _originalTask = task;
        _todoRepo = new TodoRepository();
        TaskText = task.Text;
        SelectedStatus = task.Status;
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
        _originalTask.UpdateText(TaskText);
        _originalTask.SetStatus(SelectedStatus);
        _todoRepo.Update(_originalTask);
        CloseRequest?.Invoke(true);
    }
}