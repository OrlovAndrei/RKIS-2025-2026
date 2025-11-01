using System;

class ReadCommand : ICommand
{
    public TodoList TodoList { get; set; }
    public int Index { get; set; }

    public ReadCommand(TodoList todoList, int index)
    {
        TodoList = todoList;
        Index = index;
    }

    public void Execute()
    {
        try
        {
            TodoList.Read(Index);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}

