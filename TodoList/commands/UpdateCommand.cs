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
            AppInfo.Todos.Update(Index - 1, NewText);

            Console.WriteLine($"Задача {Index} обновлена.");
        }

        public void Unexecute()
        {
            if (AppInfo.CurrentProfileId == null || AppInfo.Todos == null || string.IsNullOrWhiteSpace(_oldText))
                return;

            if (Index < 1 || Index > AppInfo.Todos.Count)
                return;

            AppInfo.Todos.Update(Index - 1, _oldText);
        }
    }
}

