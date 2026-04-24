using System;
using TodoList.Exceptions;
using TodoList.Models;
using TodoList.Services;

namespace TodoList.Commands
{
    public class StatusCommand : ICommand, IUndo, IRepositoryCommand
    {
        public string Arg { get; set; } = string.Empty;

        private IProfileRepository _profileRepository = null!;
        private ITodoRepository _todoRepository = null!;

        private TodoStatus _originalStatus;
        private int _statusIndex;
        private int _statusItemId;
        private Guid _statusProfileId;

        public void SetRepositories(IProfileRepository profileRepository, ITodoRepository todoRepository)
        {
            _profileRepository = profileRepository;
            _todoRepository = todoRepository;
        }

        public void Execute()
        {
            var currentProfileId = AppInfo.CurrentProfileId;
            if (currentProfileId == null)
            {
                throw new AuthenticationException("Не удалось получить список задач. Войдите в профиль.");
            }

            var todos = AppInfo.CurrentUserTodos;
            if (todos == null)
            {
                throw new AuthenticationException("Не удалось получить список задач. Войдите в профиль.");
            }

            var parts = Arg.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2 || !int.TryParse(parts[0], out int idx))
            {
                throw new InvalidArgumentException("Укажите номер задачи и статус.");
            }

            _statusIndex = idx - 1;
            var tasks = todos.GetAllTasks();

            if (_statusIndex < 0 || _statusIndex >= tasks.Count)
            {
                throw new TaskNotFoundException(idx);
            }

            if (!Enum.TryParse<TodoStatus>(parts[1], true, out TodoStatus status))
            {
                throw new InvalidArgumentException("Некорректный статус. Допустимые значения: NotStarted, InProgress, Completed, Postponed, Failed");
            }

            _originalStatus = tasks[_statusIndex].Status;
            _statusItemId = tasks[_statusIndex].Id;
            _statusProfileId = currentProfileId.Value;

            if (Program.UseDatabase && _todoRepository != null)
            {
                bool updated = _todoRepository.SetStatusAsync(_statusItemId, status, currentProfileId.Value).Result;
                if (!updated)
                {
                    throw new TaskNotFoundException(idx);
                }

                RefreshCurrentUserTodos(currentProfileId.Value);
            }
            else
            {
                tasks[_statusIndex].SetStatus(status);
                Program.SaveCurrentUserTasks();
            }

            Console.WriteLine($"Статус задачи {idx} изменен на: {status}");
        }

        public void Unexecute()
        {
            if (Program.UseDatabase && _todoRepository != null)
            {
                _todoRepository.SetStatusAsync(_statusItemId, _originalStatus, _statusProfileId).Wait();
                RefreshCurrentUserTodos(_statusProfileId);
                Console.WriteLine("Изменение статуса отменено");
                return;
            }

            var todos = AppInfo.CurrentUserTodos;
            if (todos != null)
            {
                var tasks = todos.GetAllTasks();
                tasks[_statusIndex].SetStatus(_originalStatus);
                Console.WriteLine("Изменение статуса отменено");
                Program.SaveCurrentUserTasks();
            }
        }

        private void RefreshCurrentUserTodos(Guid profileId)
        {
            var todos = AppInfo.CurrentUserTodos;
            if (todos == null)
            {
                return;
            }

            var tasks = todos.GetAllTasks();
            tasks.Clear();
            tasks.AddRange(_todoRepository.GetAllAsync(profileId).Result);
        }
    }
}
