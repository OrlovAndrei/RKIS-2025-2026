using System;
using System.Threading.Tasks;

public class UpdateCommand : ICommand, IUndo
{
    public int TaskNumber { get; set; }
    public string NewText { get; set; } = "";
    public TodoList TodoList { get; set; } = null!;
    public ITodoRepository TodoRepo { get; set; } = null!;
    public string OldText { get; set; } = "";
    public int UpdatedIndex { get; set; }

    public void Execute()
    {
        int idx = TaskNumber - 1;
        if (idx < 0)
            throw new InvalidArgumentException("TaskNumber", TaskNumber, "Номер задачи должен быть положительным");
        if (string.IsNullOrWhiteSpace(NewText))
            throw new InvalidArgumentException("NewText", NewText, "Текст задачи не может быть пустым");
        try
        {
            var item = TodoList.GetItem(idx);
            OldText = item.Text;
            UpdatedIndex = idx;
            TodoList.UpdateText(idx, NewText);
            Task.Run(() => TodoRepo.UpdateAsync(item)).Wait();
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
            var item = TodoList.GetItem(UpdatedIndex);
            TodoList.UpdateText(UpdatedIndex, OldText);
            item.UpdateText(OldText);
            Task.Run(() => TodoRepo.UpdateAsync(item)).Wait();
            Console.WriteLine($"Обновление задачи отменено");
        }
    }
}