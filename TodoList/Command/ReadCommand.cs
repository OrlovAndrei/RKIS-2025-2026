using System;

public class ReadCommand : ICommand
{
    public int TaskNumber { get; set; }
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        int taskIndex = TaskNumber - 1;

        if (taskIndex < 0)
        {
            throw new InvalidArgumentException("TaskNumber", TaskNumber, "Номер задачи должен быть положительным");
        }

        try
        {
            TodoItem item = TodoList.GetItem(taskIndex);
            Console.WriteLine($"=== Задача #{TaskNumber} ===");
            Console.WriteLine(item.GetFullInfo());
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new TaskNotFoundException(TaskNumber);
        }
    }
}