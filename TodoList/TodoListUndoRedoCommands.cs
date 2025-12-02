using System;

namespace TodoList
{
    /// <summary>
    /// Команда отмены последнего действия.
    /// </summary>
    internal class UndoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.UndoStack.Count == 0)
            {
                Console.WriteLine("Нет действий для отмены.");
                return;
            }

            var command = AppInfo.UndoStack.Pop();
            command.Unexecute();
            AppInfo.RedoStack.Push(command);
        }

        public void Unexecute()
        {
            // Отмена undo не требуется
        }
    }

    /// <summary>
    /// Команда повтора последнего отменённого действия.
    /// </summary>
    internal class RedoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.RedoStack.Count == 0)
            {
                Console.WriteLine("Нет действий для повтора.");
                return;
            }

            var command = AppInfo.RedoStack.Pop();
            command.Execute();
            AppInfo.UndoStack.Push(command);
        }

        public void Unexecute()
        {
            // Отмена redo не требуется
        }
    }
}


