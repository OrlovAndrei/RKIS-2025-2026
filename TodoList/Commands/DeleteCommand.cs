using System;

namespace Todolist
{
    public class DeleteCommand : ICommand
    {
        public int TaskNumber { get; private set; }
        public Todolist TodoList { get; private set; }
        private TodoItem _deletedItem;
        private int _deletedIndex;

        public DeleteCommand(Todolist todoList, int taskNumber)
        {
            TodoList = todoList;
            TaskNumber = taskNumber;
        }

        public void Execute()
        {
            _deletedIndex = TaskNumber - 1;
            _deletedItem = TodoList.GetItem(_deletedIndex);
            string taskText = _deletedItem.Text;
            TodoList.Delete(_deletedIndex);
            Console.WriteLine($"Задача №{TaskNumber} '{taskText}' удалена");
        }

        public void Unexecute()
        {
            if (_deletedItem != null)
            {
                TodoList.Insert(_deletedItem, _deletedIndex);
                Console.WriteLine($"Отменено удаление задачи №{TaskNumber}: {_deletedItem.Text}");
            }
        }
    }
}