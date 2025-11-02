using System;

public class ReadCommand : ICommand
{
    public int TaskNumber { get; set; }
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        int taskIndex = TaskNumber - 1;
        try
        {
            TodoItem item = TodoList.GetItem(taskIndex);
            Console.WriteLine($"=== Задача #{TaskNumber} ===");
            Console.WriteLine(item.GetFullInfo());
        }
        catch (System.ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {TaskNumber} не существует.");
        }
    }
}