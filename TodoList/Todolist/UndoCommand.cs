using System;

namespace Todolist
{
    public class UndoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.UndoStack.Count > 0)
            {
                ICommand command = AppInfo.UndoStack.Pop();
                command.Unexecute();
                AppInfo.RedoStack.Push(command);
                Console.WriteLine("Отмена выполнена");
            }
            else
            {
                Console.WriteLine("Нет действий для отмены");
            }
        }

        public void Unexecute()
        {
        }
    }
}