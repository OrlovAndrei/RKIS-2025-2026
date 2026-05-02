using TodoList.Models;

namespace TodoListDesktop.Services;

public sealed class TodoTaskService
{
    private readonly TodoApiClient _apiClient;
    private LoginResponse? _currentUser;

    public TodoTaskService(TodoApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public event Action? Unauthorized;

    public LoginResponse? CurrentUser => _currentUser;

    public async Task LoginAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Введите email и пароль.");
        }

        _currentUser = await _apiClient.LoginAsync(email.Trim(), password);
        _apiClient.SetToken(_currentUser.Token);
    }

    public async Task RegisterAsync(
        string username,
        string email,
        string password,
        string firstName,
        string lastName,
        int birthYear)
    {
        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Заполните все поля регистрации.");
        }

        _currentUser = await _apiClient.RegisterAsync(new RegisterRequest
        {
            Username = username.Trim(),
            Email = email.Trim(),
            Password = password,
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            BirthYear = birthYear
        });
        _apiClient.SetToken(_currentUser.Token);
    }

    public void Logout()
    {
        _currentUser = null;
        _apiClient.ClearToken();
    }

    public async Task<IReadOnlyList<TodoItem>> GetTasksAsync()
    {
        return await RunAuthorizedAsync(async () =>
            (await _apiClient.GetTodosAsync()).Select(ToTodoItem).ToList());
    }

    public async Task<TodoItem> AddTaskAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Введите текст задачи.", nameof(text));
        }

        return await RunAuthorizedAsync(async () =>
            ToTodoItem(await _apiClient.CreateTodoAsync(text.Trim())));
    }

    public async Task UpdateStatusAsync(int taskId, TodoStatus status)
    {
        await RunAuthorizedAsync(async () => await _apiClient.UpdateStatusAsync(taskId, status));
    }

    public async Task UpdateTaskAsync(int taskId, string text, TodoStatus status)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Введите текст задачи.", nameof(text));
        }

        await RunAuthorizedAsync(async () => await _apiClient.UpdateTodoAsync(taskId, text.Trim(), status));
    }

    public async Task DeleteTaskAsync(int taskId)
    {
        await RunAuthorizedAsync(async () => await _apiClient.DeleteTodoAsync(taskId));
    }

    private async Task RunAuthorizedAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (UnauthorizedAccessException)
        {
            Logout();
            Unauthorized?.Invoke();
            throw;
        }
    }

    private async Task<T> RunAuthorizedAsync<T>(Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (UnauthorizedAccessException)
        {
            Logout();
            Unauthorized?.Invoke();
            throw;
        }
    }

    private static TodoItem ToTodoItem(TodoItemResponse response)
    {
        return new TodoItem(response.Text, response.Status, response.LastUpdate)
        {
            Id = response.Id
        };
    }
}
