using System;
using System.Linq;
using TodoApp.Exceptions;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Commands
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
            var apiStorage = new ApiDataStorage();
            if (!apiStorage.IsServerAvailable())
            {
                Console.WriteLine("Ошибка: сервер недоступен.");
                return;
            }

            if (_pull)
            {
                Pull(apiStorage);
                Console.WriteLine("Синхронизация с сервера завершена.");
                return;
            }

            if (_push)
            {
                Push(apiStorage);
                Console.WriteLine("Синхронизация на сервер завершена.");
                return;
            }

            throw new InvalidArgumentException("Используйте: sync --pull или sync --push.");
        }

        private void Push(ApiDataStorage apiStorage)
        {
            apiStorage.SaveProfiles(AppInfo.Profiles);

            foreach (var profile in AppInfo.Profiles)
            {
                var todos = AppInfo.UserTodos.ContainsKey(profile.Id)
                    ? AppInfo.UserTodos[profile.Id].GetAll()
                    : AppInfo.Storage.LoadTodos(profile.Id);

                apiStorage.SaveTodos(profile.Id, todos);
            }
        }

        private void Pull(ApiDataStorage apiStorage)
        {
            var profiles = apiStorage.LoadProfiles().ToList();

            AppInfo.Profiles = profiles;
            AppInfo.Storage.SaveProfiles(AppInfo.Profiles);
            AppInfo.UserTodos.Clear();

            foreach (var profile in profiles)
            {
                var todos = apiStorage.LoadTodos(profile.Id).ToList();
                AppInfo.Storage.SaveTodos(profile.Id, todos);
                AppInfo.UserTodos[profile.Id] = AppInfo.CreateStoredTodoList(profile.Id, todos);
            }

            if (AppInfo.CurrentProfile != null)
            {
                AppInfo.CurrentProfile = profiles.FirstOrDefault(profile => profile.Id == AppInfo.CurrentProfile.Id);
            }

            AppInfo.ClearUndoRedo();
        }
    }
}
