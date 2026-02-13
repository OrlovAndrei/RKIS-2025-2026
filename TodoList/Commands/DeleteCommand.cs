using System;

namespace TodoList
{
    public class DeleteCommand : ICommand
    {
        public int Index { get; private set; }
        public TodoList TodoList { get; private set; }

        public DeleteCommand(TodoList todoList, int index)
        {
            TodoList = todoList;
            Index = index;
        }

        public void Execute()
        {
            TodoList.Delete(Index);
            Console.WriteLine("Задача удалена");
        }
    }
}