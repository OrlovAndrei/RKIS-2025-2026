using System;

namespace Todolist.Commands
{
    internal class StatusCommand : ICommand
    {
        public int Index { get; set; }
        public TodoStatus Status { get; set; }
        private TodoStatus? oldStatus = null;

        public StatusCommand(int index, TodoStatus status)
        {
            Index = index;
            Status = status;
        }

        public void Execute()
        {
            try
            {
                if (Index < 1 || Index > AppInfo.Todos.Count)
                {
                    Console.WriteLine("Ошибка: индекс вне диапазона.");
                    return;
                }

                TodoItem item = AppInfo.Todos.GetItem(Index);
                oldStatus = item.Status;
                AppInfo.Todos.SetStatus(Index, Status);
                
                string statusString = Status switch
                {
                    TodoStatus.NotStarted => "Не начата",
                    TodoStatus.InProgress => "В работе",
                    TodoStatus.Completed => "Завершена",
                    TodoStatus.Postponed => "Отложена",
                    TodoStatus.Failed => "Провалена",
                    _ => "Неизвестно"
                };
                
                Console.WriteLine($"Статус задачи {Index} сменён на: {statusString}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        public void Unexecute()
        {
            if (oldStatus.HasValue && Index >= 1 && Index <= AppInfo.Todos.Count)
            {
                AppInfo.Todos.SetStatus(Index, oldStatus.Value);
            }
        }
    }
}

