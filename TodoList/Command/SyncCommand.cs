using System;
using System.Net.Http;
using System.Threading.Tasks;

public class SyncCommand : ICommand
{
    public bool IsPull { get; set; }
    public bool IsPush { get; set; }

    public void Execute()
    {
        try
        {
            RunAsync().Wait();
        }
        catch (AggregateException ex)
        {
            throw ex.InnerException ?? ex;
        }
    }

    private async Task RunAsync()
    {
        if (!await IsServerAvailable())
        {
            Console.WriteLine("Ошибка: сервер недоступен.");
            return;
        }

        if (IsPull)
        {
            await PullData();
        }
        else if (IsPush)
        {
            await PushData();
        }
        else
        {
            Console.WriteLine("Используйте sync --pull или sync --push");
        }
    }

    private async Task<bool> IsServerAvailable()
    {
        try
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);
            var response = await client.GetAsync("http://localhost:5000/profiles");
            
            return response.StatusCode == System.Net.HttpStatusCode.OK ||
                   response.StatusCode == System.Net.HttpStatusCode.NotFound;
        }
        catch
        {
            return false;
        }
    }

    private async Task PullData()
    {
        Console.WriteLine("Синхронизация с сервера (pull)...");
        
        if (AppInfo.CurrentProfileId == null)
        {
            Console.WriteLine("Ошибка: нет активного профиля.");
            return;
        }

        try
        {
            var storage = CommandParser.GetStorage();
            if (storage == null)
            {
                Console.WriteLine("Ошибка: хранилище не инициализировано.");
                return;
            }
            var profiles = storage.LoadProfiles();
            AppInfo.Profiles = new System.Collections.Generic.List<Profile>(profiles);

            var todos = storage.LoadTodos(AppInfo.CurrentProfileId.Value);
            var todoList = new TodoList(new System.Collections.Generic.List<TodoItem>(todos));
            AppInfo.UserTodos[AppInfo.CurrentProfileId.Value] = todoList;
            
            Console.WriteLine($"Синхронизация завершена. Загружено профилей: {AppInfo.Profiles.Count}, задач: {todoList.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при синхронизации: {ex.Message}");
        }
    }

    private async Task PushData()
    {
        Console.WriteLine("Синхронизация на сервер (push)...");
        
        if (AppInfo.CurrentProfileId == null)
        {
            Console.WriteLine("Ошибка: нет активного профиля.");
            return;
        }

        try
        {
            var storage = CommandParser.GetStorage();
            if (storage == null)
            {
                Console.WriteLine("Ошибка: хранилище не инициализировано.");
                return;
            }

            storage.SaveProfiles(AppInfo.Profiles);
            
            storage.SaveTodos(AppInfo.CurrentProfileId.Value, AppInfo.CurrentTodoList);
            
            Console.WriteLine("Синхронизация завершена. Данные отправлены на сервер.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при синхронизации: {ex.Message}");
        }
    }
}