using System;

namespace TodoApp.Commands
{
    public class StatusCommand : ICommand
    {
        public string Name => "status";
        public string Description => "Изменить статус задачи (notstarted, inprogress, completed, postponed, failed)";

        // Индекс задачи и новый статус
        public int TaskIndex { get; set; }
        public TodoStatus Status { get; set; }

        // Свойства для работы с данными
        public TodoList TodoList { get; set; }
        public string TodoFilePath { get; set; }

        public bool Execute()
        {
            if (TodoList == null)
            {
                Console.WriteLine(" Ошибка: TodoList не установлен");
                return false;
            }

            if (TodoList.IsEmpty)
            {
                Console.WriteLine(" Список задач пуст!");
                return false;
            }

            try
            {
                TodoList.SetStatus(TaskIndex - 1, Status);
                Console.WriteLine($" Статус задачи #{TaskIndex} изменен на: {GetStatusString(Status)}");
                
                // Автосохранение после успешного изменения
                if (!string.IsNullOrEmpty(TodoFilePath))
                {
                    FileManager.SaveTodos(TodoList, TodoFilePath);
                }
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