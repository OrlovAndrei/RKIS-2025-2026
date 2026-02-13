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
            {
                Console.WriteLine("Ошибка: нет активного профиля.");
                return;
            }

            if (_index < 1 || _index > AppInfo.CurrentTodos.Count)
            {
                Console.WriteLine("Задача с таким индексом не найдена.");
                return;
            }

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