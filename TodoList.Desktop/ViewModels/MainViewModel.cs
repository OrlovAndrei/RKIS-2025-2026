using TodoListDesktop.Services;

namespace TodoListDesktop.ViewModels;

public sealed class MainViewModel : ViewModelBase
{
    private readonly TodoTaskService _taskService;
    private ViewModelBase _currentViewModel;

    public MainViewModel(TodoTaskService taskService)
    {
        _taskService = taskService;
        _taskService.Unauthorized += ShowLogin;
        _currentViewModel = CreateLoginViewModel();
    }

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    private LoginViewModel CreateLoginViewModel()
    {
        return new LoginViewModel(
            _taskService,
            ShowTodoList,
            ShowRegister);
    }

    private RegisterViewModel CreateRegisterViewModel()
    {
        return new RegisterViewModel(
            _taskService,
            ShowTodoList,
            ShowLogin);
    }

    private TodoListViewModel CreateTodoListViewModel()
    {
        return new TodoListViewModel(
            _taskService,
            ShowAddTask,
            ShowEditTask,
            ShowLogin);
    }

    private void ShowLogin()
    {
        _taskService.Logout();
        CurrentViewModel = CreateLoginViewModel();
    }

    private void ShowRegister()
    {
        CurrentViewModel = CreateRegisterViewModel();
    }

    private void ShowTodoList()
    {
        CurrentViewModel = CreateTodoListViewModel();
    }

    private void ShowAddTask()
    {
        CurrentViewModel = new AddTaskViewModel(_taskService, ShowTodoList, ShowTodoList);
    }

    private void ShowEditTask(TodoItemViewModel task)
    {
        CurrentViewModel = new EditTaskViewModel(_taskService, task, ShowTodoList, ShowTodoList);
    }
}
