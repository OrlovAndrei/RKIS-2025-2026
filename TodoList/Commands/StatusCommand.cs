using System;

namespace Todolist
{
    public class StatusCommand : ICommand
    {
        public Todolist TodoList { get; private set; }
        public int TaskNumber { get; private set; }
        public TodoStatus NewStatus { get; private set; }
        private TodoStatus _oldStatus;
        private TodoItem _statusItem;

        public StatusCommand(Todolist todoList, int taskNumber, TodoStatus status)
        {
            TodoList = todoList;
            TaskNumber = taskNumber;
            NewStatus = status;
        }

        public void Execute()
        {
            _statusItem = TodoList.GetItem(TaskNumber - 1);
            _oldStatus = _statusItem.Status;
            
            TodoList.SetStatus(TaskNumber - 1, NewStatus);
            Console.WriteLine($"Задача №{TaskNumber} статус изменен с {_oldStatus} на {NewStatus}");
        }

        public void Unexecute()
        {
            if (_statusItem != null)
            {
                TodoList.SetStatus(TaskNumber - 1, _oldStatus, false);
                Console.WriteLine($"Отменено изменение статуса задачи №{TaskNumber}. Восстановлен статус: {_oldStatus}");
            }
        }
    }
}