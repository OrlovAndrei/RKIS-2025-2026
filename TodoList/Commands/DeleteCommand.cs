using System;
using TodoList.Exceptions;
using TodoList.Models;
using TodoList.Data;

namespace TodoList.Commands
{
    public class DeleteCommand : ICommand, IUndo, IRepositoryCommand
    {
        public string Arg { get; set; } = string.Empty;

        private IProfileRepository _profileRepository = null!;
        private ITodoRepository _todoRepository = null!;

        private TodoItem? _deletedItem;
        private int _deletedIndex;

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

            if (!int.TryParse(Arg, out int idx))
            {
                throw new InvalidArgumentException("Укажите номер задачи.");
            }

            _deletedIndex = idx - 1;
            var tasks = todos.GetAllTasks();

            if (_deletedIndex < 0 || _deletedIndex >= tasks.Count)
            {
                throw new TaskNotFoundException(idx);
            }

            _deletedItem = tasks[_deletedIndex];

            if (Program.UseDatabase && _todoRepository != null)
            {
                bool deleted = _todoRepository.DeleteAsync(_deletedItem.Id, currentProfileId.Value).Result;
                if (!deleted)
                {
                    throw new TaskNotFoundException(idx);
                }

                RefreshCurrentUserTodos(currentProfileId.Value);
            }
            else
            {
                tasks.RemoveAt(_deletedIndex);
                Program.SaveCurrentUserTasks();
            }

            Console.WriteLine("Задача удалена");
        }

        public void Unexecute()
        {
            if (_deletedItem == null)
            {
                return;
            }

            if (Program.UseDatabase && _todoRepository != null)
            {
                _deletedItem = _todoRepository.AddAsync(_deletedItem).Result;
                RefreshCurrentUserTodos(_deletedItem.ProfileId);
                Console.WriteLine("Удаление задачи отменено");
                return;
            }

            var todos = AppInfo.CurrentUserTodos;
            if (todos != null)
            {
                var tasks = todos.GetAllTasks();
                tasks.Insert(_deletedIndex, _deletedItem);
                Console.WriteLine("Удаление задачи отменено");
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
