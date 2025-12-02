using System;

namespace TodoList
{
    /// <summary>
    /// Команда обновления текста задачи.
    /// </summary>
    internal class UpdateCommand : ICommand
    {
        public int Index { get; set; }
        public string? NewText { get; set; }

        private string? _oldText;

        public void Execute()
        {
            if (AppInfo.Todos == null)
            {
                Console.WriteLine("Ошибка: список задач не установлен.");
                return;
            }

            if (Index < 1 || Index > AppInfo.Todos.Count)
            {
                Console.WriteLine("Некорректный индекс. Используйте: update <idx> \"новый текст\"");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewText))
            {
                Console.WriteLine("Пустой текст. Используйте: update <idx> \"новый текст\"");
                return;
            }

            TodoItem item = AppInfo.Todos.GetItem(Index - 1);
            _oldText = item.Text;
            item.UpdateText(NewText);

            if (!string.IsNullOrWhiteSpace(AppInfo.TodoFilePath))
            {
                FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodoFilePath);
            }

            Console.WriteLine($"Задача {Index} обновлена.");
        }

        public void Unexecute()
        {
            if (AppInfo.Todos == null || string.IsNullOrWhiteSpace(_oldText))
                return;

            if (Index < 1 || Index > AppInfo.Todos.Count)
                return;

            TodoItem item = AppInfo.Todos.GetItem(Index - 1);
            item.UpdateText(_oldText);

            if (!string.IsNullOrWhiteSpace(AppInfo.TodoFilePath))
            {
                FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodoFilePath);
            }
        }
    }
}

