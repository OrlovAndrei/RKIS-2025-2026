using System;
using System.Threading.Tasks;

public class StatusCommand : ICommand, IUndo
{
    public int TaskNumber { get; set; }
    public TodoStatus Status { get; set; }
    public TodoList TodoList { get; set; } = null!;
    public ITodoRepository TodoRepo { get; set; } = null!;
    public TodoStatus OldStatus { get; set; }
    public int StatusIndex { get; set; }

    public void Execute()
    {
        int idx = TaskNumber - 1;
        if (idx < 0)
            throw new InvalidArgumentException("TaskNumber", TaskNumber, "Номер задачи должен быть положительным");
        try
        {
            var item = TodoList.GetItem(idx);
            OldStatus = item.Status;
            StatusIndex = idx;
            item.SetStatus(Status);
            Task.Run(() => TodoRepo.SetStatusAsync(item.Id, Status)).Wait();
            Console.WriteLine($"Статус задачи изменен");
            AppInfo.UndoStack.Push(this);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new TaskNotFoundException(TaskNumber);
        }
    }

    public void Unexecute()
    {
        var item = TodoList.GetItem(StatusIndex);
        item.SetStatus(OldStatus);
        Task.Run(() => TodoRepo.SetStatusAsync(item.Id, OldStatus)).Wait();
        Console.WriteLine($"Изменение статуса отменено");
    }
}