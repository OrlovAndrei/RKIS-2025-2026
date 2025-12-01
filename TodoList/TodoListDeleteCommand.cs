using System;

namespace TodoList
{
    /// <summary>
    /// Команда удаления задачи.
    /// </summary>
    internal class DeleteCommand : ICommand
    {
        public TodoList? TodoList { get; set; }
        public int Index { get; set; }
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
                Console.WriteLine("Некорректный индекс. Используйте: delete <idx>");
                return;
            }

            TodoList.Delete(Index - 1);

            if (!string.IsNullOrWhiteSpace(TodoFilePath))
            {
                FileManager.SaveTodos(TodoList, TodoFilePath);
            }

            Console.WriteLine($"Задача {Index} удалена.");
        }
    }
}

