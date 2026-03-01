using System;

namespace TodoApp.Commands
{
    public class StatusCommand : BaseCommand
    {
        public override string Name => "status";
        public override string Description => "Изменить статус задачи (notstarted, inprogress, completed, postponed, failed)";

        // Индекс задачи и новый статус
        public int TaskIndex { get; set; }
        public TodoStatus Status { get; set; }
        
        // Для отмены
        private TodoStatus oldStatus;
        private int actualIndex = -1;

        public override bool Execute()
        {
            if (AppInfo.Todos == null)
            {
                Console.WriteLine(" Ошибка: TodoList не установлен");
                return false;
            }

            if (AppInfo.Todos.IsEmpty)
            {
                Console.WriteLine(" Список задач пуст!");
                return false;
            }

            try
            {
                actualIndex = TaskIndex - 1;
                var task = AppInfo.Todos.GetItem(actualIndex);
                oldStatus = task.Status;
                
                AppInfo.Todos.SetStatus(actualIndex, Status);
                Console.WriteLine($" Статус задачи #{TaskIndex} изменен на: {GetStatusString(Status)}");
                
                // Сохраняем команду в стек undo
                PushToUndoStack();
                AutoSave();
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($" Ошибка: задача с номером {TaskIndex} не найдена!");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка: {ex.Message}");
                return false;
            }
        }

        public override bool Unexecute()
        {
            if (actualIndex >= 0 && AppInfo.Todos != null)
            {
                try
                {
                    // Восстанавливаем старый статус
                    AppInfo.Todos.SetStatus(actualIndex, oldStatus);
                    Console.WriteLine($" Отмена: восстановлен статус задачи #{actualIndex + 1}");
                    AutoSave();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Ошибка при отмене изменения статуса: {ex.Message}");
                    return false;
                }
            }
            return false;
        }

        private string GetStatusString(TodoStatus status)
        {
            return status switch
            {
                TodoStatus.NotStarted => "Не начата",
                TodoStatus.InProgress => "В процессе",
                TodoStatus.Completed => "Выполнена",
                TodoStatus.Postponed => "Отложена",
                TodoStatus.Failed => "Провалена",
                _ => "Неизвестно"
            };
        }
    }
}