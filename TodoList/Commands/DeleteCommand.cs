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
            {
                Console.WriteLine("Ошибка: нет активного профиля.");
                return;
            }

            _actualIndex = _index - 1;
            if (_actualIndex < 0 || _actualIndex >= AppInfo.CurrentTodos.Count)
            {
                Console.WriteLine("Задача с таким индексом не найдена.");
                return;
            }

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