using System;

public class DeleteCommand : ICommand
{
    public int TaskNumber { get; set; }
    public TodoList TodoList { get; set; }

    public string TodoFilePath { get; set; }
    public void Execute()
    {
        int taskIndex = TaskNumber - 1;
        try
        {
            TodoList.Delete(taskIndex);
            Console.WriteLine($"Задача удалена");

            FileManager.SaveTodos(TodoList, TodoFilePath);
        }
        catch (System.ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {TaskNumber} не существует.");
        }
    }
}