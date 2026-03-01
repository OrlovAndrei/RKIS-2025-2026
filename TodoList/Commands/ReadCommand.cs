using System;

namespace TodoList
{
    public class ReadCommand : ICommand
    {
        public int Index { get; private set; }
        public TodoList TodoList { get; private set; }

        public ReadCommand(TodoList todoList, int index)
        {
            TodoList = todoList;
            Index = index;
        }

        public void Execute()
        {
            TodoItem item = TodoList.GetItem(Index);
            if (item != null)
            {
                Console.WriteLine(item.GetFullInfo());
            }
        }
    }
}