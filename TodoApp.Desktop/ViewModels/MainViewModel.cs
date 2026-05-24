using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TodoApp.Desktop.Services;
using TodoApp.Models;

namespace TodoApp.Desktop.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private object? _currentViewModel;
    private Profile? _currentProfile;
    private INavigationService? _navigationService;

    public object? CurrentViewModel
    {
        get => _currentViewModel;
        set 
        { 
            _currentViewModel = value; 
            OnPropertyChanged();
            // Диагностика: вывод в консоль отладки
            System.Diagnostics.Debug.WriteLine($"CurrentViewModel changed to: {value?.GetType().Name ?? "null"}");
        }
    }

    public Profile? CurrentProfile
    {
        get => _currentProfile;
        set { _currentProfile = value; OnPropertyChanged(); }
    }

    public MainViewModel()
    {
        // Принудительно создаём LoginViewModel
        var loginVM = new LoginViewModel(this);
        CurrentViewModel = loginVM;
        System.Diagnostics.Debug.WriteLine("MainViewModel constructed, LoginViewModel assigned");
    }

    public void Initialize(INavigationService navigationService)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
    }

    public void OnLoginSuccess(Profile profile)
    {
        CurrentProfile = profile;
        CurrentViewModel = new TodoListViewModel(profile.Id, this);
        System.Diagnostics.Debug.WriteLine($"Logged in as {profile.Login}, switched to TodoListViewModel");
    }

    public void NavigateToAddTask()
    {
        if (CurrentProfile != null)
        {
            CurrentViewModel = new AddTaskViewModel(CurrentProfile.Id, this);
            System.Diagnostics.Debug.WriteLine("Navigated to AddTaskViewModel");
        }
    }
    
    public void NavigateToEditTask(TodoItem item)
    {
        CurrentViewModel = new EditTaskViewModel(item, this);
        System.Diagnostics.Debug.WriteLine("Navigated to EditTaskViewModel");
    }
    
    public void ReturnToTodoList()
    {
        if (CurrentProfile != null)
        {
            CurrentViewModel = new TodoListViewModel(CurrentProfile.Id, this);
            System.Diagnostics.Debug.WriteLine("Returned to TodoListViewModel");
        }
    }

    // Метод для принудительного обновления (вызывать, если интерфейс не появился)
    public void ForceRefresh()
    {
        var temp = CurrentViewModel;
        CurrentViewModel = null;
        CurrentViewModel = temp;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}