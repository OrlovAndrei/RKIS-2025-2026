using System;

public class UndoCommand : ICommand
{
    public void Execute()
    {
        if (AppInfo.UndoStack.Count == 0)
        {
            Console.WriteLine("Нет действий для отмены");
            return;
        }

        IUndo lastCommand = AppInfo.UndoStack.Pop();  // IUndo вместо ICommand
        lastCommand.Unexecute();
        AppInfo.RedoStack.Push(lastCommand);
        Console.WriteLine("Действие отменено");
    }
}