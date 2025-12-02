using System;

namespace TodoList
{
    /// <summary>
    /// Команда изменения статуса задачи.
    /// </summary>
    internal class StatusCommand : ICommand
    {
        public int Index { get; set; }
        public TodoStatus Status { get; set; }

        private TodoStatus _oldStatus;

        public void Execute()
        {
            if (AppInfo.Todos == null)
            {
                Console.WriteLine("Ошибка: список задач не установлен.");
                return;
            }

            if (Index < 1 || Index > AppInfo.Todos.Count)
            {
                Console.WriteLine("Некорректный индекс. Используйте: status <idx> <status>");
                return;
            }

            int internalIndex = Index - 1;
            var item = AppInfo.Todos.GetItem(internalIndex);
            _oldStatus = item.Status;
            AppInfo.Todos.SetStatus(internalIndex, Status);

            if (!string.IsNullOrWhiteSpace(AppInfo.TodoFilePath))
            {
                FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodoFilePath);
            }

            Console.WriteLine($"Статус задачи {Index} изменён на {Status}.");
        }

        public void Unexecute()
        {
            if (AppInfo.Todos == null)
                return;

            if (Index < 1 || Index > AppInfo.Todos.Count)
                return;

            int internalIndex = Index - 1;
            AppInfo.Todos.SetStatus(internalIndex, _oldStatus);

            if (!string.IsNullOrWhiteSpace(AppInfo.TodoFilePath))
            {
                FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodoFilePath);
            }
        }
    }
}


