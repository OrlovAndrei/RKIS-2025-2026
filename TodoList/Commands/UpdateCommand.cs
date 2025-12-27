using System;

namespace TodoList
{
    public class UpdateCommand : ICommand
    {
        public int Index { get; private set; }
        public string NewText { get; private set; }
        public TodoList TodoList { get; private set; }

        public UpdateCommand(TodoList todoList, int index, string newText)
        {
            TodoList = todoList;
            Index = index;
            NewText = newText;
        }

        public void Execute()
        {
            TodoItem item = TodoList.GetItem(Index);
            if (item != null)
            {
                item.UpdateText(NewText);
                Console.WriteLine("Задача обновлена");
            }
        }
    }
}