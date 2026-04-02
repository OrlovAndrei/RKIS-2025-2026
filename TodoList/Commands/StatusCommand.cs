using System;

namespace TodoList;

public class StatusCommand : ICommand
{
    private readonly int _index;
    private readonly TodoStatus _newStatus;
    private TodoStatus _oldStatus;

    public StatusCommand(int index, TodoStatus newStatus)
    {
        _index = index;
        _newStatus = newStatus;
    }

    public void Execute()
    {
        if (AppInfo.CurrentProfileId == null)
        {
            Console.WriteLine("Нет активного профиля.");
            return;
        }

        var todoList = AppInfo.CurrentTodoList;
        if (_index < 0 || _index >= todoList.Count)
        {
            Console.WriteLine("Ошибка: неверный индекс");
            return;
        }

        _oldStatus = todoList[_index].Status;
        todoList.SetStatus(_index, _newStatus);
        Console.WriteLine($"Задача {_index + 1} получила статус {_newStatus}");
        FileManager.SaveTodosForUser(AppInfo.CurrentProfileId.Value, todoList);

        AppInfo.UndoStack.Push(this);
        AppInfo.RedoStack.Clear();
    }

    public void Unexecute()
    {
        if (AppInfo.CurrentTodoList != null && _index >= 0 && _index < AppInfo.CurrentTodoList.Count)
        {
            AppInfo.CurrentTodoList.SetStatus(_index, _oldStatus);
            Console.WriteLine($"Отменено изменение статуса задачи {_index + 1}");
            FileManager.SaveTodosForUser(AppInfo.CurrentProfileId.Value, AppInfo.CurrentTodoList);
        }
    }
}