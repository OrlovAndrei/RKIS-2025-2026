using System;
using System.IO;

namespace TodoList
{
    /// <summary>
    /// Команда изменения статуса задачи.
    /// </summary>
    internal class StatusCommand : ICommand
    {
        public int Index { get; set; }
        public TodoStatus Status { get; set; }

        private TodoStatus _oldStatus;

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
                Console.WriteLine("Некорректный индекс. Используйте: status <idx> <status>");
                return;
            }

            int internalIndex = Index - 1;
            var item = AppInfo.Todos.GetItem(internalIndex);
            _oldStatus = item.Status;
            AppInfo.Todos.SetStatus(internalIndex, Status);

            // Сохраняем в файл текущего профиля
            string todoPath = Path.Combine(AppInfo.DataDirectory, $"todos_{AppInfo.CurrentProfileId}.csv");
            FileManager.SaveTodos(AppInfo.Todos, todoPath);

            Console.WriteLine($"Статус задачи {Index} изменён на {Status}.");
        }

        public void Unexecute()
        {
            if (AppInfo.CurrentProfileId == null || AppInfo.Todos == null)
                return;

            if (Index < 1 || Index > AppInfo.Todos.Count)
                return;

            int internalIndex = Index - 1;
            AppInfo.Todos.SetStatus(internalIndex, _oldStatus);

            string todoPath = Path.Combine(AppInfo.DataDirectory, $"todos_{AppInfo.CurrentProfileId}.csv");
            FileManager.SaveTodos(AppInfo.Todos, todoPath);
        }
    }
}


