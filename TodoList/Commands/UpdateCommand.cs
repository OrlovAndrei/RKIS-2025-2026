using System;
using TodoList.Exceptions;
using TodoList.Models;
using TodoList.Data;

namespace TodoList.Commands
{
    public class UpdateCommand : ICommand, IUndo, IRepositoryCommand
    {
        public string Arg { get; set; } = string.Empty;

        private IProfileRepository _profileRepository = null!;
        private ITodoRepository _todoRepository = null!;

        private string _originalText = string.Empty;
        private int _updatedIndex;
        private int _updatedItemId;
        private Guid _updatedProfileId;

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
                throw new InvalidArgumentException("Укажите номер задачи и текст.");
            }

            _updatedIndex = idx - 1;
            var tasks = todos.GetAllTasks();

            if (_updatedIndex < 0 || _updatedIndex >= tasks.Count)
            {
                throw new TaskNotFoundException(idx);
            }

            string newText = parts[1].Trim('"', '\'');
            if (string.IsNullOrWhiteSpace(newText))
            {
                throw new InvalidArgumentException("Текст задачи не может быть пустым.");
            }

            var task = tasks[_updatedIndex];
            _originalText = task.Text;
            _updatedItemId = task.Id;
            _updatedProfileId = currentProfileId.Value;

            if (Program.UseDatabase && _todoRepository != null)
            {
                task.Text = newText;
                task.LastUpdate = DateTime.Now;

                bool updated = _todoRepository.UpdateAsync(task).Result;
                if (!updated)
                {
                    throw new TaskNotFoundException(idx);
                }

                RefreshCurrentUserTodos(currentProfileId.Value);
            }
            else
            {
                task.UpdateText(newText);
                Program.SaveCurrentUserTasks();
            }

            Console.WriteLine("Задача обновлена");
        }

        public void Unexecute()
        {
            if (Program.UseDatabase && _todoRepository != null)
            {
                var task = _todoRepository.GetByIdAsync(_updatedItemId, _updatedProfileId).Result;
                if (task == null)
                {
                    return;
                }

                task.Text = _originalText;
                task.LastUpdate = DateTime.Now;
                _todoRepository.UpdateAsync(task).Wait();
                RefreshCurrentUserTodos(_updatedProfileId);
                Console.WriteLine("Обновление задачи отменено");
                return;
            }

            var todos = AppInfo.CurrentUserTodos;
            if (todos != null)
            {
                var tasks = todos.GetAllTasks();
                tasks[_updatedIndex].UpdateText(_originalText);
                Console.WriteLine("Обновление задачи отменено");
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
