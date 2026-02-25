using TodoList.Exceptions;

namespace TodoList
{
    public class UndoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.UndoStack.Count == 0)
                throw new InvalidCommandException("Нет действий для отмены.");

            var command = AppInfo.UndoStack.Pop();
            command.Unexecute();
            AppInfo.RedoStack.Push(command);
            Console.WriteLine("Отменено последнее действие.");
        }

        public void Unexecute() { }
    }
}