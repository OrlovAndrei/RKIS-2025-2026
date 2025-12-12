using System;

namespace Todolist.Commands
{
    internal class UndoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.UndoStack.Count == 0)
            {
                Console.WriteLine("Нет команд для отмены.");
                return;
            }

            ICommand lastCommand = AppInfo.UndoStack.Pop();
            lastCommand.Unexecute();
            AppInfo.RedoStack.Push(lastCommand);
            Console.WriteLine("Команда отменена.");
        }

        public void Unexecute()
        {
            // для Undo откат не нужен
        }
    }
}

