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
            _actualIndex = _index - 1;
            if (_actualIndex < 0 || _actualIndex >= AppInfo.Todos.Count)
            {
                Console.WriteLine("Задача с таким индексом не найдена.");
                return;
            }

            _deletedItem = AppInfo.Todos[_actualIndex];
            AppInfo.Todos.Delete(_index);
            AppInfo.UndoStack.Push(this);  
            AppInfo.RedoStack.Clear();
            Console.WriteLine("Удалено.");
        }

        public void Unexecute()
        {
            if (_deletedItem != null)
            {
                AppInfo.Todos.Insert(_actualIndex, _deletedItem);
                Console.WriteLine("Удаление задачи отменено.");
            }
        }
    }
}