using System;

public class UndoCommand : ICommand
{
    public void Execute()
    {
        if (AppInfo.UndoStack.Count == 0)
        {
            throw new InvalidCommandException("Нет действий для отмены");
        }

        IUndo lastCommand = AppInfo.UndoStack.Pop();
        lastCommand.Unexecute();
        AppInfo.RedoStack.Push(lastCommand);
        Console.WriteLine("Действие отменено");
    }
}