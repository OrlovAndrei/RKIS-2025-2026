namespace TodoList.commands;

public class UndoCommand : ICommand
{
    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
        {
            Console.WriteLine("Ошибка: нет активного профиля");
            return;
        }
        
        if (AppInfo.UndoStack.Count == 0)
        {
            Console.WriteLine("Нет действий для отмены");
            return;
        }

        var command = AppInfo.UndoStack.Pop();
        command.Unexecute();
        AppInfo.RedoStack.Push(command);
    }

    public void Unexecute()
    {
        // Команда undo не требует отмены
    }
}