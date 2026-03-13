using System;
using Todolist.Exceptions;

namespace Todolist.Commands
{
    internal class StatusCommand : IUndo
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
            if (AppInfo.CurrentProfileId == Guid.Empty)
                throw new AuthenticationException("Необходимо войти в профиль для работы с задачами.");

            TodoItem item = AppInfo.Todos.GetItem(Index);
            oldStatus = item.Status;
            AppInfo.Todos.SetStatus(Index, Status);
            string statusString = TodoStatusHelper.ToDisplayString(Status);
            Console.WriteLine($"Статус задачи {Index} сменён на: {statusString}");
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
