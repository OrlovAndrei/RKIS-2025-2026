using System;

namespace TodoList
{
    /// <summary>
    /// Команда отметки задачи как выполненной.
    /// </summary>
    internal class DoneCommand : ICommand
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
                Console.WriteLine("Некорректный индекс. Используйте: done <idx>");
                return;
            }

            TodoItem item = TodoList.GetItem(Index - 1);
            item.MarkDone();

            if (!string.IsNullOrWhiteSpace(TodoFilePath))
            {
                FileManager.SaveTodos(TodoList, TodoFilePath);
            }

            Console.WriteLine($"Задача {Index} отмечена как выполненная.");
        }
    }
}

