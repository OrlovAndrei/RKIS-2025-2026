using System;
using System.Net.Http;
using TodoList.Data;
using TodoList.Exceptions;

namespace TodoList.Commands
{
    public class SyncCommand : ICommand
    {
        public string[] Flags { get; set; } = Array.Empty<string>();

        public void Execute()
        {
            bool isPush = Flags.Contains("push");
            bool isPull = Flags.Contains("pull");

            if (!isPush && !isPull)
            {
                throw new InvalidCommandException("Использование: sync --push или sync --pull");
            }

            if (isPush && isPull)
            {
                throw new InvalidCommandException("Нельзя использовать --push и --pull одновременно");
            }

            if (AppInfo.CurrentProfileId == null)
            {
                throw new AuthenticationException("Необходимо войти в систему");
            }

            if (!IsServerAvailable())
            {
                Console.WriteLine("Ошибка: сервер недоступен.");
                return;
            }

            var apiStorage = new ApiDataStorage();

            if (isPush)
            {
                PushData(apiStorage);
            }
            else
            {
                PullData(apiStorage);
            }
        }

        private bool IsServerAvailable()
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(3);
                var response = client.GetAsync("http://localhost:5000/profiles").Result;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void PushData(ApiDataStorage apiStorage)
        {
            try
            {
                Console.WriteLine("Отправка данных на сервер...");

                apiStorage.SaveProfiles(AppInfo.AllProfiles);
                Console.WriteLine("✓ Профили отправлены");

                if (AppInfo.CurrentUserTodos != null)
                {
                    apiStorage.SaveTodos(AppInfo.CurrentProfileId!.Value, AppInfo.CurrentUserTodos.GetAllTasks());
                    Console.WriteLine("✓ Задачи отправлены");
                }

                Console.WriteLine("Синхронизация (push) завершена успешно!");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при отправке данных: {ex.Message}", ex);
            }
        }

        private void PullData(ApiDataStorage apiStorage)
        {
            try
            {
                Console.WriteLine("Загрузка данных с сервера...");

                var loadedProfiles = apiStorage.LoadProfiles();
                AppInfo.AllProfiles.Clear();
                AppInfo.AllProfiles.AddRange(loadedProfiles);
                Console.WriteLine("✓ Профили загружены");

                var loadedTodos = apiStorage.LoadTodos(AppInfo.CurrentProfileId!.Value);
                
                var newTodoList = new TodoList(loadedTodos);
                
                newTodoList.TaskAdded += (task) => Program.SaveCurrentUserTasks();
                newTodoList.TaskDeleted += (task) => Program.SaveCurrentUserTasks();
                newTodoList.TaskUpdated += (task) => Program.SaveCurrentUserTasks();
                newTodoList.StatusChanged += (task) => Program.SaveCurrentUserTasks();

                AppInfo.AllTodos[AppInfo.CurrentProfileId.Value] = newTodoList;
                
                Console.WriteLine("✓ Задачи загружены");
                Console.WriteLine("Синхронизация (pull) завершена успешно!");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при загрузке данных: {ex.Message}", ex);
            }
        }

        public void Unexecute() { }
    }
}