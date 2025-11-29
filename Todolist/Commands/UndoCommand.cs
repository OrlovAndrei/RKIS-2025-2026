using System;

namespace Todolist.Commands
{
    internal class UndoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.UndoStack.Count == 0)
            {
                Console.WriteLine("Нет действий для отмены.");
                return;
            }

            ICommand lastCommand = AppInfo.UndoStack.Pop();
            lastCommand.Unexecute();
            AppInfo.RedoStack.Push(lastCommand);
            Console.WriteLine("Действие отменено.");
        }

        public void Unexecute()
        {
            // UndoCommand не должен сохраняться в стек
        }
    }
}

