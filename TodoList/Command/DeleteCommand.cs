using System;
using System.Collections.Generic;

public class DeleteCommand : ICommand, IUndo  // Добавлен IUndo
{
    public int TaskNumber { get; set; }
    public TodoList TodoList { get; set; }
    public string TodoFilePath { get; set; }
    public TodoItem DeletedItem { get; set; }
    public int DeletedIndex { get; set; }

    public void Execute()
    {
        int taskIndex = TaskNumber - 1;
        try
        {
            DeletedItem = TodoList.GetItem(taskIndex);
            DeletedIndex = taskIndex;

            TodoList.Delete(taskIndex);
            Console.WriteLine($"Задача удалена");

            AppInfo.UndoStack.Push(this);
        }
        catch (System.ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Задачи с номером {TaskNumber} не существует.");
        }
    }

    public void Unexecute()  // Метод из IUndo
    {
        if (DeletedItem != null)
        {
            var items = new List<TodoItem>();
            for (int i = 0; i < TodoList.Count; i++)
            {
                if (i == DeletedIndex)
                {
                    items.Add(DeletedItem);
                }
                items.Add(TodoList.GetItem(i));
            }
            if (DeletedIndex >= TodoList.Count)
            {
                items.Add(DeletedItem);
            }

            while (TodoList.Count > 0)
            {
                TodoList.Delete(0);
            }
            foreach (var item in items)
            {
                TodoList.Add(item);
            }

            FileManager.SaveTodos(TodoList, TodoFilePath);
            Console.WriteLine($"Удаление задачи отменено");
        }
    }
}