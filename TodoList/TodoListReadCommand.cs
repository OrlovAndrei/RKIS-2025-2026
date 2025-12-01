using System;

namespace TodoList
{
    /// <summary>
    /// Команда вывода полной информации о задаче.
    /// </summary>
    internal class ReadCommand : ICommand
    {
        public TodoList? TodoList { get; set; }
        public int Index { get; set; }

        public void Execute()
        {
            if (TodoList == null)
            {
                Console.WriteLine("Ошибка: список задач не установлен.");
                return;
            }

            if (Index < 1 || Index > TodoList.Count)
            {
                Console.WriteLine("Некорректный индекс. Используйте: read <idx>");
                return;
            }

            TodoItem item = TodoList.GetItem(Index - 1);
            Console.WriteLine(item.GetFullInfo());
        }
    }
}

