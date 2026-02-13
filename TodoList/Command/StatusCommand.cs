using System;

public class StatusCommand : ICommand
{
    public int TaskNumber { get; set; }
    public TodoStatus Status { get; set; }
    public TodoList TodoList { get; set; }
    public string TodoFilePath { get; set; }
    public TodoStatus OldStatus { get; set; }
    public int StatusIndex { get; set; }

    public void Execute()
    {
        int taskIndex = TaskNumber - 1;
        try
        {
            TodoItem item = TodoList.GetItem(taskIndex);
            OldStatus = item.Status;
            StatusIndex = taskIndex;
            item.SetStatus(Status);
            Console.WriteLine($"Статус задачи изменен");

            AppInfo.UndoStack.Push(this);
        }
        catch (System.ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {TaskNumber} не существует.");
        }
    }
    public void Unexecute()
    {
        TodoItem item = TodoList.GetItem(StatusIndex);
        item.SetStatus(OldStatus);
        FileManager.SaveTodos(TodoList, TodoFilePath);
        Console.WriteLine($"Изменение статуса отменено");
    }
}