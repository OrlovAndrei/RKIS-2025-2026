using System;

class DoneCommand : ICommand
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
            Console.WriteLine($"Задача {Index} отмечена как выполненная.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}

