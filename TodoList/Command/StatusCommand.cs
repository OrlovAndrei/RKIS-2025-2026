using System;

public class StatusCommand : ICommand
{
    public int TaskNumber { get; set; }
    public TodoStatus Status { get; set; }
    public TodoList TodoList { get; set; }
    public string TodoFilePath { get; set; }

    public void Execute()
    {
        int taskIndex = TaskNumber - 1;
        try
        {
            TodoItem item = TodoList.GetItem(taskIndex);
            item.SetStatus(Status);
            Console.WriteLine($"Статус задачи изменен");

            FileManager.SaveTodos(TodoList, TodoFilePath);
        }
        catch (System.ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {TaskNumber} не существует.");
        }
    }
}