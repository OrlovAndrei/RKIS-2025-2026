using System;
using System.IO;

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
            if (AppInfo.CurrentProfileId == null)
            {
                Console.WriteLine("Ошибка: необходимо войти в профиль.");
                return;
            }

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

            // Сохраняем в файл текущего профиля
            string todoPath = Path.Combine(AppInfo.DataDirectory, $"todos_{AppInfo.CurrentProfileId}.csv");
            FileManager.SaveTodos(AppInfo.Todos, todoPath);

            Console.WriteLine($"Задача {Index} удалена.");
        }

        public void Unexecute()
        {
            if (AppInfo.CurrentProfileId == null || AppInfo.Todos == null || _deletedItem == null)
                return;

            int internalIndex = Index - 1;
            AppInfo.Todos.Insert(internalIndex, _deletedItem);

            string todoPath = Path.Combine(AppInfo.DataDirectory, $"todos_{AppInfo.CurrentProfileId}.csv");
            FileManager.SaveTodos(AppInfo.Todos, todoPath);
        }
    }
}

