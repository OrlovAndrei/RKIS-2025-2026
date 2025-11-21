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

        ICommand lastUndoneCommand = AppInfo.RedoStack.Pop();
        lastUndoneCommand.Execute();
        AppInfo.UndoStack.Push(lastUndoneCommand);
        Console.WriteLine("Действие повторено");
    }

    public void Unexecute()
    {
      
    }
}
