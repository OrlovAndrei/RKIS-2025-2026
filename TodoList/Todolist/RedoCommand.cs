using System;

namespace Todolist
{
    public class RedoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.RedoStack.Count > 0)
            {
                ICommand command = AppInfo.RedoStack.Pop();
                command.Execute();
                AppInfo.UndoStack.Push(command);
                Console.WriteLine("Повтор выполнена");
            }
            else
            {
                Console.WriteLine("Нет действий для повтора");
            }
        }

        public void Unexecute()
        {
        }
    }
}