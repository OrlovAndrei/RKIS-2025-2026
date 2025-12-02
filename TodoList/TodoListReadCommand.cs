using System;

namespace TodoList
{
    /// <summary>
    /// Команда вывода полной информации о задаче.
    /// </summary>
    internal class ReadCommand : ICommand
    {
        public int Index { get; set; }

        public void Execute()
        {
            if (AppInfo.Todos == null)
            {
                Console.WriteLine("Ошибка: список задач не установлен.");
                return;
            }

            if (Index < 1 || Index > AppInfo.Todos.Count)
            {
                Console.WriteLine("Некорректный индекс. Используйте: read <idx>");
                return;
            }

            TodoItem item = AppInfo.Todos.GetItem(Index - 1);
            Console.WriteLine(item.GetFullInfo());
        }

        public void Unexecute()
        {
            // Чтение не изменяет состояние
        }
    }
}

