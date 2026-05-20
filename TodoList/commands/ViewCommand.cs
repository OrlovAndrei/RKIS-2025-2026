using System;

namespace TodoList
{
    /// <summary>
    /// Команда просмотра списка задач.
    /// </summary>
    internal class ViewCommand : ICommand
    {
        public bool ShowIndex { get; set; } = true;
        public bool ShowDone { get; set; } = true;
        public bool ShowDate { get; set; } = true;

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

            AppInfo.Todos.View(ShowIndex, ShowDone, ShowDate);
        }

        public void Unexecute()
        {
            // Просмотр не изменяет состояние, отменять нечего
        }
    }
}

