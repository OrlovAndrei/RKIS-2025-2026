using System;

namespace TodoList
{
    public class ReadCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public int Index { get; set; }

        public void Execute()
        {
            Console.WriteLine(TodoList.GetItem(Index).GetFullInfo());
        }
    }
}
