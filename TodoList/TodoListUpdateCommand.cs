using System;

namespace TodoList
{
    /// <summary>
    /// Команда обновления текста задачи.
    /// </summary>
    internal class UpdateCommand : ICommand
    {
        public TodoList? TodoList { get; set; }
        public int Index { get; set; }
        public string? NewText { get; set; }
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
                Console.WriteLine("Некорректный индекс. Используйте: update <idx> \"новый текст\"");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewText))
            {
                Console.WriteLine("Пустой текст. Используйте: update <idx> \"новый текст\"");
                return;
            }

            TodoItem item = TodoList.GetItem(Index - 1);
            item.UpdateText(NewText);

            if (!string.IsNullOrWhiteSpace(TodoFilePath))
            {
                FileManager.SaveTodos(TodoList, TodoFilePath);
            }

            Console.WriteLine($"Задача {Index} обновлена.");
        }
    }
}

