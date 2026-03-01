using TodoList.Exceptions;

namespace TodoList
{
    public class DeleteCommand : ICommand
    {
        private readonly int _index;
        private TodoItem _deletedItem;
        private int _actualIndex;

        public DeleteCommand(int index)
        {
            _index = index;
        }

        public void Execute()
        {
            if (AppInfo.CurrentTodos == null)
                throw new AuthenticationException("Необходимо войти в профиль.");

            if (_index < 1 || _index > AppInfo.CurrentTodos.Count)
                throw new TaskNotFoundException($"Задача с индексом {_index} не найдена.");

            _actualIndex = _index - 1;
            _deletedItem = AppInfo.CurrentTodos[_actualIndex];
            AppInfo.CurrentTodos.Delete(_index);
            AppInfo.UndoStack.Push(this);
            AppInfo.RedoStack.Clear();
            Console.WriteLine("Удалено.");
        }

        public void Unexecute()
        {
            if (_deletedItem != null && AppInfo.CurrentTodos != null)
            {
                AppInfo.CurrentTodos.Insert(_actualIndex, _deletedItem);
                Console.WriteLine("Удаление задачи отменено.");
            }
        }
    }
}