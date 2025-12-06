using System;

namespace Todolist
{
    public class StatusCommand : ICommand
    {
        public int TaskNumber { get; private set; }
        public TodoStatus NewStatus { get; private set; }
        private TodoStatus _oldStatus;
        private TodoItem _statusItem;

        public StatusCommand(int taskNumber, TodoStatus status)
        {
            TaskNumber = taskNumber;
            NewStatus = status;
        }

        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
            {
                Console.WriteLine("Ошибка: необходимо войти в профиль");
                return;
            }

            var todoList = AppInfo.GetCurrentTodos();
            
            if (TaskNumber < 1 || TaskNumber > todoList.GetCount())
            {
                Console.WriteLine($"Ошибка: задача №{TaskNumber} не существует");
                return;
            }

            _statusItem = todoList.GetItem(TaskNumber - 1);
            _oldStatus = _statusItem.Status;
            
            todoList.SetStatus(TaskNumber - 1, NewStatus);
            Console.WriteLine($"Задача №{TaskNumber} статус изменен с {_oldStatus} на {NewStatus}");
            
            FileManager.SaveTodos(todoList, AppInfo.CurrentProfileId.Value);
        }

        public void Unexecute()
        {
            if (!AppInfo.CurrentProfileId.HasValue || _statusItem == null)
                return;

            var todoList = AppInfo.GetCurrentTodos();
            
            todoList.SetStatus(TaskNumber - 1, _oldStatus, false);
            Console.WriteLine($"Отменено изменение статуса задачи №{TaskNumber}. Восстановлен статус: {_oldStatus}");
            
            FileManager.SaveTodos(todoList, AppInfo.CurrentProfileId.Value);
        }
    }
}