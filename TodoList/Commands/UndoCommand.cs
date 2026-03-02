using System;
using Todolist.Exceptions;

namespace Todolist
{
    public class UndoCommand : ICommand
    {
        public void Execute()
        {
            try
            {
                if (AppInfo.CurrentProfile == null)
                    throw new AuthenticationException("Необходимо войти в профиль для отмены действий.");

                if (AppInfo.UndoStack.Count == 0)
                {
                    Console.WriteLine("Нет действий для отмены.");
                    return;
                }

                var command = AppInfo.UndoStack.Pop();
                command.Unexecute();
                AppInfo.RedoStack.Push(command);
                
                Console.WriteLine("Отменено последнее действие.");
            }
            catch (Exception ex) when (!(ex is AuthenticationException))
            {
                throw;
            }
        }
    }
}