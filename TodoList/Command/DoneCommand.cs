using System;

public class DoneCommand : ICommand
{
    public int TaskNumber { get; set; }
    public TodoList TodoList { get; set; }
    public string TodoFilePath { get; set; }
    public void Execute()
    {
        int taskIndex = TaskNumber - 1;
        try
        {
            TodoItem item = TodoList.GetItem(taskIndex);
            item.MarkDone();
            Console.WriteLine($"Задача '{item.Text}' отмечена как выполненная");

            FileManager.SaveTodos(TodoList, TodoFilePath);
        }
        catch (System.ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {TaskNumber} не существует.");
        }
    }
}