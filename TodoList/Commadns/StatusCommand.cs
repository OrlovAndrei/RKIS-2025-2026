using System;

namespace TodoList
{
    public class StatusCommand : ICommand
    {
        public int TaskNumber { get; set; }
        public TodoStatus Status { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            try
            {
                TodoList.SetStatus(TaskNumber, Status);
                Console.WriteLine($"Статус задачи {TaskNumber} изменен на: {GetStatusText(Status)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка изменения статуса: {ex.Message}");
            }
        }

        private string GetStatusText(TodoStatus status)
        {
            return status switch
            {
                TodoStatus.NotStarted => "Не начато",
                TodoStatus.InProgress => "В процессе",
                TodoStatus.Completed => "Выполнено",
                TodoStatus.Postponed => "Отложено",фывфывфыв
                TodoStatus.Failed => "Провалено",
                _ => "Неизвестно"
            };
        }
    }
}