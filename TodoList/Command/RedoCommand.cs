using System;

public class RedoCommand : ICommand
{
    public void Execute()
    {
        if (AppInfo.RedoStack.Count == 0)
        {
            throw new InvalidCommandException("Нет действий для повтора");
        }

        IUndo lastUndoneCommand = AppInfo.RedoStack.Pop();
        ((ICommand)lastUndoneCommand).Execute();
        AppInfo.UndoStack.Push(lastUndoneCommand);
        Console.WriteLine("Действие повторено");
    }
}