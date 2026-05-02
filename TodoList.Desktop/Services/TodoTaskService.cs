using TodoList.Models;
using TodoList.Services;

namespace TodoListWpf.Services;

public sealed class TodoTaskService
{
    private const string DefaultLogin = "wpf-user";

    private readonly IProfileRepository _profileRepository;
    private readonly ITodoRepository _todoRepository;
    private Profile? _currentProfile;

    public TodoTaskService(IProfileRepository profileRepository, ITodoRepository todoRepository)
    {
        _profileRepository = profileRepository;
        _todoRepository = todoRepository;
    }

    public async Task<IReadOnlyList<TodoItem>> GetTasksAsync()
    {
        var profile = await GetOrCreateProfileAsync();
        var tasks = await _todoRepository.GetAllAsync(profile.Id);
        return tasks.ToList();
    }

    public async Task<TodoItem> AddTaskAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Введите текст задачи.", nameof(text));
        }

        var profile = await GetOrCreateProfileAsync();
        var item = new TodoItem(text)
        {
            ProfileId = profile.Id
        };

        return await _todoRepository.AddAsync(item);
    }

    public async Task UpdateStatusAsync(int taskId, TodoStatus status)
    {
        var profile = await GetOrCreateProfileAsync();
        var updated = await _todoRepository.SetStatusAsync(taskId, status, profile.Id);

        if (!updated)
        {
            throw new InvalidOperationException("Задача не найдена.");
        }
    }

    public async Task DeleteTaskAsync(int taskId)
    {
        var profile = await GetOrCreateProfileAsync();
        var deleted = await _todoRepository.DeleteAsync(taskId, profile.Id);

        if (!deleted)
        {
            throw new InvalidOperationException("Задача не найдена.");
        }
    }

    private async Task<Profile> GetOrCreateProfileAsync()
    {
        if (_currentProfile != null)
        {
            return _currentProfile;
        }

        var profile = await _profileRepository.GetByLoginAsync(DefaultLogin);
        if (profile != null)
        {
            _currentProfile = profile;
            return profile;
        }

        _currentProfile = await _profileRepository.AddAsync(new Profile(
            Guid.NewGuid(),
            DefaultLogin,
            "wpf",
            "WPF",
            "User",
            2000));

        return _currentProfile;
    }
}
