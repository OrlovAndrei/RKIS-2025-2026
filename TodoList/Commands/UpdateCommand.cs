using System;

namespace TodoList;

public class UpdateCommand : ICommand
{
    private readonly int _index;
    private readonly string _newText;
    private string _oldText;

    public UpdateCommand(int index, string newText)
    {
        _index = index;
        _newText = newText;
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

        _oldText = todoList[_index].Text;
        todoList.Update(_index, _newText);
        Console.WriteLine($"Задача {_index + 1} обновлена");
        FileManager.SaveTodosForUser(AppInfo.CurrentProfileId.Value, todoList);

        AppInfo.UndoStack.Push(this);
        AppInfo.RedoStack.Clear();
    }

    public void Unexecute()
    {
        if (_oldText != null && AppInfo.CurrentTodoList != null && _index >= 0 && _index < AppInfo.CurrentTodoList.Count)
        {
            AppInfo.CurrentTodoList.Update(_index, _oldText);
            Console.WriteLine($"Отменено обновление задачи {_index + 1}");
            FileManager.SaveTodosForUser(AppInfo.CurrentProfileId.Value, AppInfo.CurrentTodoList);
        }
    }
}