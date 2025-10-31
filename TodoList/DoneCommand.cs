using System;

namespace TodoList
{
    public class DoneCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public int Index { get; set; }

        public void Execute()
        {
            TodoList.GetItem(Index).MarkDone();
            Console.WriteLine($"Задача #{Index + 1} отмечена выполненной.");
        }
    }
}
