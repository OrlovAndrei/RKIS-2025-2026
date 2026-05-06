using System;
using System.Collections.Generic;
using System.Linq;

namespace Todolist.Commands
{
    internal sealed class SyncCommand : ICommand
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
            using var remoteStorage = new ApiDataStorage();

            if (!remoteStorage.IsServerAvailable())
            {
                Console.WriteLine("\u041E\u0448\u0438\u0431\u043A\u0430: \u0441\u0435\u0440\u0432\u0435\u0440 \u043D\u0435\u0434\u043E\u0441\u0442\u0443\u043F\u0435\u043D.");
                return;
            }

            if (_push)
            {
                Push(remoteStorage);
                Console.WriteLine("Local data uploaded to the server.");
            }

            if (_pull)
            {
                Pull(remoteStorage);
                Console.WriteLine("Local state updated from server data.");
            }
        }

        private static void Push(ApiDataStorage remoteStorage)
        {
            AppInfo.Storage.SaveProfiles(AppInfo.Profiles);

            if (AppInfo.CurrentProfileId != Guid.Empty)
                AppInfo.Storage.SaveTodos(AppInfo.CurrentProfileId, AppInfo.Todos);

            List<Profile> localProfiles = AppInfo.Storage.LoadProfiles().ToList();
            remoteStorage.SaveProfiles(localProfiles);

            foreach (Profile profile in localProfiles)
            {
                List<TodoItem> todos = AppInfo.Storage.LoadTodos(profile.Id).ToList();
                remoteStorage.SaveTodos(profile.Id, todos);
            }
        }

        private static void Pull(ApiDataStorage remoteStorage)
        {
            Guid previousProfileId = AppInfo.CurrentProfileId;

            List<Profile> remoteProfiles = remoteStorage.LoadProfiles().ToList();
            AppInfo.Storage.SaveProfiles(remoteProfiles);

            var todosByProfile = new Dictionary<Guid, List<TodoItem>>();
            foreach (Profile profile in remoteProfiles)
            {
                List<TodoItem> todos = remoteStorage.LoadTodos(profile.Id).ToList();
                todosByProfile[profile.Id] = todos;
                AppInfo.Storage.SaveTodos(profile.Id, todos);
            }

            AppInfo.Profiles = remoteProfiles;
            AppInfo.TodosByProfile.Clear();

            foreach ((Guid profileId, List<TodoItem> todos) in todosByProfile)
            {
                AppInfo.TodosByProfile[profileId] = new TodoList(todos);
            }

            AppInfo.UndoStack.Clear();
            AppInfo.RedoStack.Clear();

            if (previousProfileId != Guid.Empty &&
                AppInfo.TodosByProfile.TryGetValue(previousProfileId, out TodoList? pulledCurrentTodos))
            {
                AppInfo.CurrentProfileId = previousProfileId;
                AppInfo.Todos = pulledCurrentTodos;
                Program.AttachTodoEventHandlers(AppInfo.Todos);
                return;
            }

            if (AppInfo.Profiles.Count > 0)
            {
                AppInfo.CurrentProfile = AppInfo.Profiles[0];
                Program.AttachTodoEventHandlers(AppInfo.Todos);
                return;
            }

            AppInfo.CurrentProfileId = Guid.Empty;
            AppInfo.Todos = new TodoList();
        }
    }
}

