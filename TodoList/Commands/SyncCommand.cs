using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todolist.Exceptions;
using Todolist.Models;

namespace Todolist
{
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
            if (!AppInfo.CurrentProfileId.HasValue)
                throw new AuthenticationException("Необходимо войти в профиль");

            if (!_pull && !_push)
            {
                Console.WriteLine("Укажите флаг --pull или --push для синхронизации");
                return;
            }

            if (_pull && _push)
            {
                Console.WriteLine("Нельзя использовать оба флага --pull и --push одновременно");
                return;
            }

            Console.WriteLine("Проверка доступности сервера...");
            var apiStorage = AppInfo.DataStorage as ApiDataStorage;

            if (apiStorage == null)
            {
                Console.WriteLine("Ошибка: ApiDataStorage не используется. Убедитесь, что DataStorage инициализирован как ApiDataStorage");
                return;
            }

            var checkTask = Task.Run(async () => await apiStorage.CheckServerAvailabilityAsync());
            bool isAvailable = checkTask.GetAwaiter().GetResult();

            if (!isAvailable)
            {
                Console.WriteLine("Ошибка: сервер недоступен.");
                Console.WriteLine("Убедитесь, что сервер запущен на http://localhost:5000/");
                return;
            }

            Console.WriteLine("Сервер доступен.");

            if (_pull)
            {
                SyncPull(apiStorage);
            }
            else if (_push)
            {
                SyncPush(apiStorage);
            }
        }

        private void SyncPull(ApiDataStorage apiStorage)
        {
            Console.WriteLine("\n=== Синхронизация: получение данных с сервера ===");

            try
            {
                var currentProfile = AppInfo.GetCurrentProfile();
                var currentTodos = new List<TodoItem>(AppInfo.GetCurrentTodos());

                Console.WriteLine("Загрузка профилей с сервера...");
                var serverProfiles = apiStorage.LoadProfiles().ToList();

                Console.WriteLine("Загрузка задач с сервера...");
                var serverTodos = apiStorage.LoadTodos(AppInfo.CurrentProfileId.Value).ToList();

                var currentServerProfile = serverProfiles.FirstOrDefault(p => p.Id == AppInfo.CurrentProfileId.Value);
                if (currentServerProfile == null)
                {
                    currentServerProfile = serverProfiles.FirstOrDefault(p => p.Login == currentProfile?.Login);
                }

                if (currentServerProfile != null)
                {
                    var localProfile = AppInfo.Profiles.FirstOrDefault(p => p.Id == AppInfo.CurrentProfileId.Value);
                    if (localProfile != null)
                    {
                        var index = AppInfo.Profiles.IndexOf(localProfile);
                        if (index >= 0)
                        {
                            AppInfo.Profiles[index] = currentServerProfile;
                        }
                    }
                    else
                    {
                        AppInfo.Profiles.Add(currentServerProfile);
                    }
                }

                var todoList = AppInfo.GetCurrentTodos();
                
                while (todoList.GetCount() > 0)
                {
                    todoList.Delete(0);
                }

                foreach (var item in serverTodos)
                {
                    todoList.Add(item);
                }

                AppInfo.DataStorage.SaveProfiles(AppInfo.Profiles);
                AppInfo.DataStorage.SaveTodos(AppInfo.CurrentProfileId.Value, todoList);

                Console.WriteLine($"\nСинхронизация завершена (PULL):");
                Console.WriteLine($"- Загружено профилей: {serverProfiles.Count}");
                Console.WriteLine($"- Загружено задач: {serverTodos.Count}");
                Console.WriteLine("\nЛокальные данные обновлены.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка при синхронизации PULL: {ex.Message}");
            }
        }

        private void SyncPush(ApiDataStorage apiStorage)
        {
            Console.WriteLine("\n=== Синхронизация: отправка данных на сервер ===");

            try
            {
                var localProfiles = AppInfo.Profiles;
                var localTodos = AppInfo.GetCurrentTodos().ToList();

                Console.WriteLine($"Отправка профилей на сервер... ({localProfiles.Count} профилей)");
                apiStorage.SaveProfiles(localProfiles);

                Console.WriteLine($"Отправка задач на сервер... ({localTodos.Count} задач)");
                apiStorage.SaveTodos(AppInfo.CurrentProfileId.Value, localTodos);

                Console.WriteLine($"\nСинхронизация завершена (PUSH):");
                Console.WriteLine($"- Отправлено профилей: {localProfiles.Count}");
                Console.WriteLine($"- Отправлено задач: {localTodos.Count}");
                Console.WriteLine("\nДанные на сервере обновлены.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка при синхронизации PUSH: {ex.Message}");
            }
        }

        public void Unexecute()
        {
            throw new NotImplementedException("Команда sync не поддерживает отмену");
        }
    }
}