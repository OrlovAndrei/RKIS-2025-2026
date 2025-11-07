using System;

namespace Todolist.Commands
{
    internal class UpdateCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public int Index { get; set; }
        public string NewText { get; set; }

        public UpdateCommand(TodoList todoList, int index, string newText)
        {
            TodoList = todoList;
            Index = index;
            NewText = newText;
        }

        public void Execute()
        {
            if (Index < 1 || Index > TodoList.Count)
            {
                Console.WriteLine("Ошибка: индекс вне диапазона.");
                return;
            }

            try
            {
                TodoItem item = TodoList.GetItem(Index);
                item.UpdateText(NewText);
                FileManager.SaveTodos(TodoList, Program.TodoFilePath);
                Console.WriteLine($"Задача {Index} обновлена.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
