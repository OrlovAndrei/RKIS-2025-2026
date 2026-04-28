using System;
using Todolist.Exceptions;
using Todolist.Models;

namespace Todolist
{
    public class StatusCommand : ICommand
    {
        public int TaskNumber { get; private set; }
        public TodoStatus NewStatus { get; private set; }
        private TodoStatus _oldStatus;
        private TodoItem? _statusItem;

        public StatusCommand(int taskNumber, TodoStatus status)
        {
            TaskNumber = taskNumber;
            NewStatus = status;
        }

        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
                throw new AuthenticationException("Необходимо войти в профиль");

            var todoList = AppInfo.GetCurrentTodos();

            if (TaskNumber < 1 || TaskNumber > todoList.GetCount())
                throw new TaskNotFoundException($"Задача №{TaskNumber} не существует");

            _statusItem = todoList.GetItem(TaskNumber - 1);
            _oldStatus = _statusItem.Status;

            _statusItem.SetStatus(NewStatus);

            AppInfo.TodoRepository.Update(_statusItem);

            Console.WriteLine($"Задача №{TaskNumber} статус изменен с {_oldStatus} на {NewStatus}");
        }

        public void Unexecute()
        {
            if (!AppInfo.CurrentProfileId.HasValue || _statusItem == null)
                return;

            var todoList = AppInfo.GetCurrentTodos();

            _statusItem.SetStatus(_oldStatus);
            AppInfo.TodoRepository.Update(_statusItem);

            Console.WriteLine($"Отменено изменение статуса задачи №{TaskNumber}. Восстановлен статус: {_oldStatus}");
        }
    }
}