using System;

namespace TodoList;

public class RedoCommand : ICommand
{
    public void Execute()
    {
        if (AppInfo.RedoStack.Count == 0)
        {
            Console.WriteLine("Нечего повторять.");
            return;
        }
        var command = AppInfo.RedoStack.Pop();
        command.Execute();
    }

    public void Unexecute()
    {
        Console.WriteLine("Операция redo не поддерживает отмену.");
    }
}