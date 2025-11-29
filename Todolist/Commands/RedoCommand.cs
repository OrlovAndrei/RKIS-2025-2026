using System;

namespace Todolist.Commands
{
    internal class RedoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.RedoStack.Count == 0)
            {
                Console.WriteLine("Нет действий для повтора.");
                return;
            }

            ICommand lastCommand = AppInfo.RedoStack.Pop();
            lastCommand.Execute();
            AppInfo.UndoStack.Push(lastCommand);
            Console.WriteLine("Действие повторено.");
        }

        public void Unexecute()
        {
            // RedoCommand не должен сохраняться в стек
        }
    }
}

