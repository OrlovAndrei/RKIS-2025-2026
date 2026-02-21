using System;
using TodoList.Exceptions;

namespace TodoList.Commands
{
    public class StatusCommand : ICommand, IUndo
    {
        public string Arg { get; set; }

        private TodoStatus _originalStatus;
        private int _statusIndex;

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
                throw new InvalidArgumentException($"Некорректный статус. Допустимые значения: NotStarted, InProgress, Completed, Postponed, Failed");
            }

            _originalStatus = tasks[_statusIndex].Status;
            tasks[_statusIndex].SetStatus(status);
            Console.WriteLine($"Статус задачи {idx} изменен на: {status}");
            Program.SaveCurrentUserTasks();
        }

        public void Unexecute()
        {
            var todos = AppInfo.CurrentUserTodos;
            if (todos != null)
            {
                var tasks = todos.GetAllTasks();
                tasks[_statusIndex].SetStatus(_originalStatus);
                Console.WriteLine("Изменение статуса отменено");
                Program.SaveCurrentUserTasks();
            }
        }
    }
}