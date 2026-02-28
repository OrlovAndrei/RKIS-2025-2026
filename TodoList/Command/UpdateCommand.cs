using System;

public class UpdateCommand : ICommand, IUndo
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

        if (taskIndex < 0)
        {
            throw new InvalidArgumentException("TaskNumber", TaskNumber, "Номер задачи должен быть положительным");
        }

        if (string.IsNullOrWhiteSpace(NewText))
        {
            throw new InvalidArgumentException("NewText", NewText, "Текст задачи не может быть пустым");
        }

        try
        {
            TodoItem item = TodoList.GetItem(taskIndex);
            OldText = item.Text;
            UpdatedIndex = taskIndex;

            TodoList.UpdateText(taskIndex, NewText);

            Console.WriteLine($"Задача обновлена");

            AppInfo.UndoStack.Push(this);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new TaskNotFoundException(TaskNumber);
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