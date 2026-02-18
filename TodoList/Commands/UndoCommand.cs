namespace TodoList.Commands
{
    public class UndoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.undoStack.Count > 0)
            {
                ICommand lastCommand = AppInfo.undoStack.Pop();
                
                if (lastCommand is IUndo undoableCommand)
                {
                    undoableCommand.Unexecute();
                    AppInfo.redoStack.Push(lastCommand);
                    Console.WriteLine("Отмена выполнена");
                }
                else
                {
                    AppInfo.undoStack.Push(lastCommand);
                    Console.WriteLine("Ошибка: команда не поддерживает отмену");
                }
            }
            else
            {
                Console.WriteLine("Нечего отменять");
            }
        }

        public void Unexecute() { }
    }
}