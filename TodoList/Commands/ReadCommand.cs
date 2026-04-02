using System;

namespace TodoList;

public class ReadCommand : ICommand
{
    private readonly int _index;

    public ReadCommand(int index)
    {
        _index = index;
    }

    public void Execute()
    {
        if (AppInfo.CurrentProfileId == null)
        {
            Console.WriteLine("Нет активного профиля.");
            return;
        }
        AppInfo.CurrentTodoList?.Read(_index);
    }

    public void Unexecute() { }
}