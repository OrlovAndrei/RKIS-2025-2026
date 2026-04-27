using System;
using System.Linq;
using TodoList.Exceptions;

namespace TodoList.commands;

public class SyncCommand : ICommand
{
    public bool IsPull { get; set; }
    public bool IsPush { get; set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
            throw new AuthenticationException("Необходимо войти в профиль для синхронизации.");

        if (AppInfo.DataStorage is not ApiDataStorage apiStorage)
        {
            Console.WriteLine("Синхронизация доступна только при использовании API хранилища.");
            Console.WriteLine("Убедитесь, что в Program.cs используется ApiDataStorage.");
            return;
        }

        // Проверка доступности сервера
        var isAvailable = apiStorage.IsServerAvailableAsync().GetAwaiter().GetResult();
        if (!isAvailable)
        {
            Console.WriteLine("Ошибка: сервер недоступен.");
            return;
        }

        if (IsPull && IsPush)
        {
            Console.WriteLine("Ошибка: используйте только один флаг --pull или --push.");
            return;
        }

        if (IsPull)
        {
            PullData(apiStorage);
        }
        else if (IsPush)
        {
            PushData(apiStorage);
        }
        else
        {
            Console.WriteLine("Укажите флаг --pull или --push. Использование: sync --pull | sync --push");
        }
    }

    private void PullData(ApiDataStorage apiStorage)
    {
        Console.WriteLine("Синхронизация: получение данных с сервера...");

        try
        {
            // Получаем профили с сервера
            var serverProfiles = apiStorage.LoadProfiles().ToList();
            if (serverProfiles.Any())
            {
                AppInfo.Profiles = serverProfiles;
                Console.WriteLine($"Загружено профилей: {serverProfiles.Count}");
            }
            else
            {
                Console.WriteLine("На сервере нет профилей.");
            }

            // Для текущего пользователя загружаем задачи
            if (AppInfo.CurrentProfileId.HasValue)
            {
                var serverTodos = apiStorage.LoadTodos(AppInfo.CurrentProfileId.Value).ToList();
                var currentTodoList = AppInfo.GetCurrentTodoList();
                currentTodoList.items.Clear();
                foreach (var item in serverTodos)
                {
                    currentTodoList.Add(item);
                }
                Console.WriteLine($"Загружено задач: {serverTodos.Count}");
            }

            Console.WriteLine("Синхронизация (pull) завершена.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при синхронизации: {ex.Message}");
        }
    }

    private void PushData(ApiDataStorage apiStorage)
    {
        Console.WriteLine("Синхронизация: отправка данных на сервер...");

        try
        {
            // Отправляем профили
            apiStorage.SaveProfiles(AppInfo.Profiles);
            Console.WriteLine($"Отправлено профилей: {AppInfo.Profiles.Count}");

            // Отправляем задачи текущего пользователя
            if (AppInfo.CurrentProfileId.HasValue)
            {
                var currentTodoList = AppInfo.GetCurrentTodoList();
                apiStorage.SaveTodos(AppInfo.CurrentProfileId.Value, currentTodoList.items);
                Console.WriteLine($"Отправлено задач: {currentTodoList.items.Count}");
            }

            Console.WriteLine("Синхронизация (push) завершена.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при синхронизации: {ex.Message}");
        }
    }

    public void Unexecute()
    {
        // Команда sync не отменяется
    }
}