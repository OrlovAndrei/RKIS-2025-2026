using System;

public class ReadCommand : ICommand
{
    public int TaskNumber { get; set; }
    public TodoList TodoList { get; set; } = null!;

    public void Execute()
    {
        int idx = TaskNumber - 1;
        if (idx < 0)
            throw new InvalidArgumentException("TaskNumber", TaskNumber, "Номер задачи должен быть положительным");
        try
        {
            var item = TodoList.GetItem(idx);
            Console.WriteLine($"=== Задача #{TaskNumber} ===");
            Console.WriteLine(item.GetFullInfo());
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new TaskNotFoundException(TaskNumber);
        }
    }
}