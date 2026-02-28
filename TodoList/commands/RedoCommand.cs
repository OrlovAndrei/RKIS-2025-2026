using TodoList.Exceptions;

namespace TodoList.commands;

public class RedoCommand : ICommand
{
    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
            throw new AuthenticationException("Необходимо войти в профиль для повтора действий.");
        
        if (AppInfo.RedoStack.Count == 0)
            throw new InvalidCommandException("Нет действий для повтора.");

        var command = AppInfo.RedoStack.Pop();
        command.Execute();
        AppInfo.UndoStack.Push(command);
    }

    public void Unexecute() { }
}