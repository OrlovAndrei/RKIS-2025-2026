using System;

public class RedoCommand : ICommand
{
    public void Execute()
    {
        if (AppInfo.RedoStack.Count == 0)
        {
            Console.WriteLine("Нет действий для повтора");
            return;
        }

        IUndo lastUndoneCommand = AppInfo.RedoStack.Pop();  // IUndo вместо ICommand
        ((ICommand)lastUndoneCommand).Execute();  // Приводим к ICommand для Execute()
        AppInfo.UndoStack.Push(lastUndoneCommand);
        Console.WriteLine("Действие повторено");
    }
}