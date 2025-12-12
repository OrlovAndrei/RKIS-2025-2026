using System;

namespace Todolist.Commands
{
    internal class RedoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.RedoStack.Count == 0)
            {
                Console.WriteLine("Нет команд для повтора.");
                return;
            }

            ICommand lastCommand = AppInfo.RedoStack.Pop();
            lastCommand.Execute();
            AppInfo.UndoStack.Push(lastCommand);
            Console.WriteLine("Команда выполнена повторно.");
        }

        public void Unexecute()
        {
            // для Redo откат не нужен
        }
    }
}

