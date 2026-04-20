using System;

namespace TodoList
{
    public class StatusCommand : ICommand
    {
        public int Index { get; private set; }
        public TodoStatus Status { get; private set; }
        public TodoList TodoList { get; private set; }

        public StatusCommand(TodoList todoList, int index, TodoStatus status)
        {
            TodoList = todoList;
            Index = index;
            Status = status;
        }

        public void Execute()
        {
            TodoList.SetStatus(Index, Status);
            Console.WriteLine("Задача выполнена");
        }
    }
}