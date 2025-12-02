using System;

namespace TodoList
{
    /// <summary>
    /// Команда удаления задачи.
    /// </summary>
    internal class DeleteCommand : ICommand
    {
        public int Index { get; set; }

        private TodoItem? _deletedItem;

        public void Execute()
        {
            if (AppInfo.Todos == null)
            {
                Console.WriteLine("Ошибка: список задач не установлен.");
                return;
            }

            if (Index < 1 || Index > AppInfo.Todos.Count)
            {
                Console.WriteLine("Некорректный индекс. Используйте: delete <idx>");
                return;
            }

            int internalIndex = Index - 1;
            _deletedItem = AppInfo.Todos.GetItem(internalIndex);
            AppInfo.Todos.Delete(internalIndex);

            if (!string.IsNullOrWhiteSpace(AppInfo.TodoFilePath))
            {
                FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodoFilePath);
            }

            Console.WriteLine($"Задача {Index} удалена.");
        }

        public void Unexecute()
        {
            if (AppInfo.Todos == null || _deletedItem == null)
                return;

            int internalIndex = Index - 1;
            AppInfo.Todos.Insert(internalIndex, _deletedItem);

            if (!string.IsNullOrWhiteSpace(AppInfo.TodoFilePath))
            {
                FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodoFilePath);
            }
        }
    }
}

