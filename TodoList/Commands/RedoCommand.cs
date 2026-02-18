namespace TodoList.Commands
{
    public class RedoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.redoStack.Count > 0)
            {
                ICommand lastUndoneCommand = AppInfo.redoStack.Pop();
                lastUndoneCommand.Execute();
                
                if (lastUndoneCommand is IUndo)
                {
                    AppInfo.undoStack.Push(lastUndoneCommand);
                }
                
                Console.WriteLine("Повтор выполнен");
            }
            else
            {
                Console.WriteLine("Нечего повторять");
            }
        }

        public void Unexecute() { }
    }
}