using System;

namespace Todolist.Commands
{
    internal class DoneCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public int Index { get; set; }

        public DoneCommand(TodoList todoList, int index)
        {
            TodoList = todoList;
            Index = index;
        }

        public void Execute()
        {
            try
            {
                TodoItem item = TodoList.GetItem(Index);
                item.MarkDone();
                FileManager.SaveTodos(TodoList, Program.TodoFilePath);
                Console.WriteLine($"Задача {Index} отмечена как выполненная.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
