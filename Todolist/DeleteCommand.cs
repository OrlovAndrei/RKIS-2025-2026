using System;

class DeleteCommand : ICommand
{
    public TodoList TodoList { get; set; }
    public int Index { get; set; }

    public DeleteCommand(TodoList todoList, int index)
    {
        TodoList = todoList;
        Index = index;
    }

    public void Execute()
    {
        try
        {
            TodoList.Delete(Index);
            Console.WriteLine($"Задача {Index} удалена.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}

