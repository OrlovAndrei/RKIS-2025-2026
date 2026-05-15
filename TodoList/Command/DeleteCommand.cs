using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DeleteCommand : ICommand, IUndo
{
    public int TaskNumber { get; set; }
    public TodoList TodoList { get; set; } = null!;
    public ITodoRepository TodoRepo { get; set; } = null!;
    public TodoItem DeletedItem { get; set; } = null!;
    public int DeletedIndex { get; set; }

    public void Execute()
    {
        int idx = TaskNumber - 1;
        if (idx < 0)
            throw new InvalidArgumentException("TaskNumber", TaskNumber, "Номер задачи должен быть положительным");
        try
        {
            DeletedItem = TodoList.GetItem(idx);
            DeletedIndex = idx;
            TodoList.Delete(idx);
            Task.Run(() => TodoRepo.DeleteAsync(DeletedItem.Id)).Wait();
            Console.WriteLine($"Задача удалена");
            AppInfo.UndoStack.Push(this);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new TaskNotFoundException(TaskNumber);
        }
    }

    public void Unexecute()
    {
        if (DeletedItem != null)
        {
            var items = new List<TodoItem>();
            for (int i = 0; i < TodoList.Count; i++)
            {
                if (i == DeletedIndex)
                    items.Add(DeletedItem);
                items.Add(TodoList.GetItem(i));
            }
            if (DeletedIndex >= TodoList.Count)
                items.Add(DeletedItem);
            while (TodoList.Count > 0)
                TodoList.Delete(0);
            foreach (var item in items)
                TodoList.Add(item);
            Task.Run(() => TodoRepo.AddAsync(DeletedItem)).Wait();
            Console.WriteLine($"Удаление задачи отменено");
        }
    }
}