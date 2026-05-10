using System;
using TodoList.Exceptions;

namespace TodoList.Commands
{
    public class RedoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.redoStack.Count == 0)
            {
                throw new InvalidCommandException("Нечего повторять.");
            }

            ICommand lastUndoneCommand = AppInfo.redoStack.Pop();
            lastUndoneCommand.Execute();
            
            if (lastUndoneCommand is IUndo)
            {
                AppInfo.undoStack.Push(lastUndoneCommand);
            }
            
            Console.WriteLine("Повтор выполнен");
        }

        public void Unexecute() { }
    }
}