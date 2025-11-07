using System;

public class UpdateCommand : ICommand
{
    public int TaskNumber { get; set; }
    public string NewText { get; set; }
    public TodoList TodoList { get; set; }

    public string TodoFilePath { get; set; }
    public void Execute()
    {
        int taskIndex = TaskNumber - 1;
        try
        {
            TodoItem item = TodoList.GetItem(taskIndex);
            item.UpdateText(NewText);
            Console.WriteLine($"Задача обновлена");

            FileManager.SaveTodos(TodoList, TodoFilePath);
        }
        catch (System.ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {TaskNumber} не существует.");
        }
    }
}