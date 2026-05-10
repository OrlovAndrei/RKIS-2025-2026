using System;
using TodoList.Exceptions;

namespace TodoList.Commands
{
    public class UndoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.undoStack.Count == 0)
            {
                throw new InvalidCommandException("Нечего отменять.");
            }

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
                throw new InvalidCommandException("Команда не поддерживает отмену.");
            }
        }

        public void Unexecute() { }
    }
}