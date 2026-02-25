using TodoList.Exceptions;

namespace TodoList
{
    public class StatusCommand : ICommand
    {
        private readonly int _index;
        private readonly TodoStatus _newStatus;
        private TodoStatus _oldStatus;

        public StatusCommand(int index, TodoStatus status)
        {
            _index = index;
            _newStatus = status;
        }

        public void Execute()
        {
            if (AppInfo.CurrentTodos == null)
                throw new AuthenticationException("Необходимо войти в профиль.");

            if (_index < 1 || _index > AppInfo.CurrentTodos.Count)
                throw new TaskNotFoundException($"Задача с индексом {_index} не найдена.");

            _oldStatus = AppInfo.CurrentTodos[_index - 1].Status;
            AppInfo.CurrentTodos.SetStatus(_index, _newStatus);
            AppInfo.UndoStack.Push(this);
            AppInfo.RedoStack.Clear();
            Console.WriteLine($"Статус задачи {_index} изменен на {_newStatus}.");
        }

        public void Unexecute()
        {
            if (AppInfo.CurrentTodos != null)
            {
                AppInfo.CurrentTodos.SetStatus(_index, _oldStatus);
                Console.WriteLine($"Статус задачи {_index} возвращен к {_oldStatus}.");
            }
        }
    }
}