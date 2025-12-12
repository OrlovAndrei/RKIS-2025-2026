using System;

public class UpdateCommand : ICommand
{
    public int TaskNumber { get; set; }
    public string NewText { get; set; }
    public TodoList TodoList { get; set; }
    public string TodoFilePath { get; set; }
    public string OldText { get; set; }
    public int UpdatedIndex { get; set; }

    public void Execute()
    {
        int taskIndex = TaskNumber - 1;
        try
        {
            TodoItem item = TodoList.GetItem(taskIndex);
            OldText = item.Text;
            UpdatedIndex = taskIndex;

            TodoList.UpdateText(taskIndex, NewText);

            Console.WriteLine($"Задача обновлена");

            AppInfo.UndoStack.Push(this);
        }
        catch (System.ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {TaskNumber} не существует.");
        }
    }

    public void Unexecute()
    {
        if (!string.IsNullOrEmpty(OldText))
        {

            TodoList.UpdateText(UpdatedIndex, OldText);

            Console.WriteLine($"Обновление задачи отменено");
        }
    }
}