using System;

namespace Todolist
{
    public class StatusCommand : ICommand
    {
        public int Index { get; set; }
        public TodoStatus Status { get; set; }
        private TodoStatus _oldStatus;
        private int _taskIndex;

        public void Execute()
        {
            _taskIndex = Index - 1;
            TodoItem item = AppInfo.Todos.GetItem(_taskIndex);
            if (item != null)
            {
                _oldStatus = item.Status;
                item.SetStatus(Status);
                Console.WriteLine($"Задача {Index} установлена в статус: {GetStatusText(Status)}");

                string dataDir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data");
                string todoFile = System.IO.Path.Combine(dataDir, "todo.csv");
                FileManager.SaveTodos(AppInfo.Todos, todoFile);
            }
            else
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
            }
        }

        public void Unexecute()
        {
            TodoItem item = AppInfo.Todos.GetItem(_taskIndex);
            if (item != null)
            {
                item.SetStatus(_oldStatus);
                Console.WriteLine($"Отменено изменение статуса: {GetStatusText(Status)} -> {GetStatusText(_oldStatus)}");
            }
        }

        private string GetStatusText(TodoStatus status)
        {
            return status switch
            {
                TodoStatus.NotStarted => "Не начато",
                TodoStatus.InProgress => "В процессе",
                TodoStatus.Completed => "Выполнено",
                TodoStatus.Postponed => "Отложено",
                TodoStatus.Failed => "Провалено",
                _ => "Неизвестно"
            };
        }
    }
}
