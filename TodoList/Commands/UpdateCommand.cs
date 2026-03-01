using System;
using TodoList.Exceptions;

namespace TodoList.Commands
{
    public class UpdateCommand : ICommand, IUndo
    {
        public string Arg { get; set; }

        private string _originalText;
        private int _updatedIndex;

        public void Execute()
        {
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

            _originalText = tasks[_updatedIndex].Text;
            tasks[_updatedIndex].UpdateText(parts[1].Trim('"', '\''));
            Console.WriteLine("Задача обновлена");
            Program.SaveCurrentUserTasks();
        }

        public void Unexecute()
        {
            var todos = AppInfo.CurrentUserTodos;
            if (_originalText != null && todos != null)
            {
                var tasks = todos.GetAllTasks();
                tasks[_updatedIndex].UpdateText(_originalText);
                Console.WriteLine("Обновление задачи отменено");
                Program.SaveCurrentUserTasks();
            }
        }
    }
}