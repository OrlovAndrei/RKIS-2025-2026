using System;

namespace TodoList
{
    /// <summary>
    /// Команда просмотра списка задач.
    /// </summary>
    internal class ViewCommand : ICommand
    {
        public TodoList? TodoList { get; set; }
        public bool ShowIndex { get; set; } = true;
        public bool ShowDone { get; set; } = true;
        public bool ShowDate { get; set; } = true;

        public void Execute()
        {
            if (TodoList == null)
            {
                Console.WriteLine("Ошибка: список задач не установлен.");
                return;
            }

            TodoList.View(ShowIndex, ShowDone, ShowDate);
        }
    }
}

