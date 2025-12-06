namespace TodoList.commands;

public class RedoCommand : ICommand
{
    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
        {
            Console.WriteLine("Ошибка: нет активного профиля");
            return;
        }
        
        if (AppInfo.RedoStack.Count == 0)
        {
            Console.WriteLine("Нет действий для повторения");
            return;
        }

        var command = AppInfo.RedoStack.Pop();
        command.Execute();
        AppInfo.UndoStack.Push(command);
        
        if (AppInfo.CurrentProfileId.HasValue)
            FileManager.SaveTodos(AppInfo.CurrentProfileId.Value, AppInfo.GetCurrentTodoList());
    }

    public void Unexecute()
    {
        // Команда redo не требует отмены
    }
}