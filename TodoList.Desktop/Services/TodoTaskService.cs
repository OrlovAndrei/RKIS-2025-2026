using TodoList.Models;
using TodoList.Data;

namespace TodoListDesktop.Services;

public sealed class TodoTaskService
{
    private readonly IProfileRepository _profileRepository;
    private readonly ITodoRepository _todoRepository;
    private Profile? _currentProfile;

    public TodoTaskService(IProfileRepository profileRepository, ITodoRepository todoRepository)
    {
        _profileRepository = profileRepository;
        _todoRepository = todoRepository;
    }

    public Profile? CurrentProfile => _currentProfile;

    public async Task LoginAsync(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Введите логин и пароль.");
        }

        var profile = await _profileRepository.GetByLoginAsync(login.Trim());
        if (profile == null || profile.Password != password)
        {
            throw new InvalidOperationException("Неверный логин или пароль.");
        }

        _currentProfile = profile;
    }

    public async Task RegisterAsync(string login, string password, string firstName, string lastName, int birthYear)
    {
        if (string.IsNullOrWhiteSpace(login) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Заполните все поля регистрации.");
        }

        var existing = await _profileRepository.GetByLoginAsync(login.Trim());
        if (existing != null)
        {
            throw new InvalidOperationException("Пользователь с таким логином уже существует.");
        }

        _currentProfile = await _profileRepository.AddAsync(new Profile(
            Guid.NewGuid(),
            login.Trim(),
            password,
            firstName.Trim(),
            lastName.Trim(),
            birthYear));
    }

    public void Logout()
    {
        _currentProfile = null;
    }

    public async Task<IReadOnlyList<TodoItem>> GetTasksAsync()
    {
        var profile = GetRequiredProfile();
        var tasks = await _todoRepository.GetAllAsync(profile.Id);
        return tasks.ToList();
    }

    public async Task<TodoItem> AddTaskAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Введите текст задачи.", nameof(text));
        }

        var profile = GetRequiredProfile();
        var item = new TodoItem(text)
        {
            ProfileId = profile.Id
        };

        return await _todoRepository.AddAsync(item);
    }

    public async Task UpdateStatusAsync(int taskId, TodoStatus status)
    {
        var profile = GetRequiredProfile();
        var updated = await _todoRepository.SetStatusAsync(taskId, status, profile.Id);

        if (!updated)
        {
            throw new InvalidOperationException("Задача не найдена.");
        }
    }

    public async Task UpdateTaskAsync(int taskId, string text, TodoStatus status)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Введите текст задачи.", nameof(text));
        }

        var profile = GetRequiredProfile();
        var item = await _todoRepository.GetByIdAsync(taskId, profile.Id);
        if (item == null)
        {
            throw new InvalidOperationException("Задача не найдена.");
        }

        item.Text = text;
        item.Status = status;
        item.LastUpdate = DateTime.Now;

        var updated = await _todoRepository.UpdateAsync(item);
        if (!updated)
        {
            throw new InvalidOperationException("Задача не найдена.");
        }
    }

    public async Task DeleteTaskAsync(int taskId)
    {
        var profile = GetRequiredProfile();
        var deleted = await _todoRepository.DeleteAsync(taskId, profile.Id);

        if (!deleted)
        {
            throw new InvalidOperationException("Задача не найдена.");
        }
    }

    private Profile GetRequiredProfile()
    {
        if (_currentProfile != null)
        {
            return _currentProfile;
        }

        throw new InvalidOperationException("Сначала войдите в профиль.");
    }
}
