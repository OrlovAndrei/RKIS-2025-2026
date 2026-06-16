namespace TodoList.commands;

public class RedoCommand : ICommand
{
    public void Execute()
    {
        if (AppInfo.RedoStack.Count > 0)
        {
            ICommand command = AppInfo.RedoStack.Pop();
            command.Execute();
            AppInfo.UndoStack.Push(command);
            Console.WriteLine("Команда повторена");
        }
        else
        {
            Console.WriteLine("Нет команд для повтора");
        }
    }
    public void Unexecute()
    {
        Console.WriteLine("Отмена для команды redo не поддерживается");
    }
}