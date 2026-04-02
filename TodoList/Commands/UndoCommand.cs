using System;

namespace TodoList;

public class UndoCommand : ICommand
{
    public void Execute()
    {
        if (AppInfo.UndoStack.Count == 0)
        {
            Console.WriteLine("Нечего отменять.");
            return;
        }
        var command = AppInfo.UndoStack.Pop();
        command.Unexecute();
        AppInfo.RedoStack.Push(command);
    }

    public void Unexecute()
    {
        Console.WriteLine("Операция undo не поддерживает отмену.");
    }
}