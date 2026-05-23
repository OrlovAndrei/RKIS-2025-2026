using TodoList.Exceptions;
using TodoList.Models;
using TodoList.Services;

namespace TodoList;

public class SyncCommand : ICommand
{
    private readonly bool _pull;
    private readonly bool _push;

    public SyncCommand(bool pull, bool push)
    {
        _pull = pull;
        _push = push;
    }

    public void Execute()
    {
        if (!_pull && !_push)
        {
            Console.WriteLine("Использование: sync --pull или sync --push");
            return;
        }

        if (!IsServerAvailable())
        {
            Console.WriteLine("Ошибка: сервер недоступен.");
            return;
        }

        var api = new ApiDataStorage();
        var profileRepo = new ProfileRepository();
        var todoRepo = new TodoRepository();

        if (_pull) PullData(api, profileRepo, todoRepo);
        else PushData(api, profileRepo, todoRepo);
    }

    private bool IsServerAvailable()
    {
        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(3) };
            var response = client.GetAsync("http://localhost:5000/profiles").GetAwaiter().GetResult();
            return true;
        }
        catch { return false; }
    }

    private void PullData(ApiDataStorage api, ProfileRepository profileRepo, TodoRepository todoRepo)
    {
        Console.WriteLine("Загрузка данных с сервера...");

        var serverProfiles = api.LoadProfiles().ToList();
        foreach (var sp in serverProfiles)
        {
            var existing = profileRepo.GetByIdAsync(sp.Id).GetAwaiter().GetResult();
            if (existing == null)
                profileRepo.AddAsync(sp).GetAwaiter().GetResult();
            else
                profileRepo.UpdateAsync(sp).GetAwaiter().GetResult();

            var serverTodos = api.LoadTodos(sp.Id).ToList();
            todoRepo.ReplaceAllForProfileAsync(sp.Id, serverTodos).GetAwaiter().GetResult();
        }

        AppInfo.Profiles = serverProfiles;
        foreach (var p in serverProfiles)
        {
            var todos = todoRepo.GetAllForProfileAsync(p.Id).GetAwaiter().GetResult();
            AppInfo.TodosByUser[p.Id] = new TodoList(todos.ToList());
        }

        if (AppInfo.CurrentProfileId.HasValue && AppInfo.TodosByUser.ContainsKey(AppInfo.CurrentProfileId.Value))
            Console.WriteLine("Текущий пользователь: данные обновлены.");

        Console.WriteLine("Синхронизация (pull) завершена.");
    }

    private void PushData(ApiDataStorage api, ProfileRepository profileRepo, TodoRepository todoRepo)
    {
        Console.WriteLine("Отправка данных на сервер...");

        var localProfiles = AppInfo.Profiles.ToList();
        api.SaveProfiles(localProfiles);

        foreach (var profile in localProfiles)
        {
            IEnumerable<TodoItem> todos;
            if (AppInfo.TodosByUser.TryGetValue(profile.Id, out var todoList))
                todos = todoList.ToList();
            else
                todos = todoRepo.GetAllForProfileAsync(profile.Id).GetAwaiter().GetResult();

            api.SaveTodos(profile.Id, todos);
        }

        Console.WriteLine("Синхронизация (push) завершена.");
    }
}