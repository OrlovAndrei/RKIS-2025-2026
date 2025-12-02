using System;

namespace TodoList
{
    /// <summary>
    /// Команда изменения статуса задачи.
    /// </summary>
    internal class StatusCommand : ICommand
    {
        public TodoList? TodoList { get; set; }
        public int Index { get; set; }
        public TodoStatus Status { get; set; }
        public string? TodoFilePath { get; set; }

        public void Execute()
        {
            if (TodoList == null)
            {
                Console.WriteLine("Ошибка: список задач не установлен.");
                return;
            }

            if (Index < 1 || Index > TodoList.Count)
            {
                Console.WriteLine("Некорректный индекс. Используйте: status <idx> <status>");
                return;
            }

            TodoList.SetStatus(Index - 1, Status);

            if (!string.IsNullOrWhiteSpace(TodoFilePath))
            {
                FileManager.SaveTodos(TodoList, TodoFilePath);
            }

            Console.WriteLine($"Статус задачи {Index} изменён на {Status}.");
        }
    }
}


