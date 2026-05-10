using System;
using Todolist.Exceptions;

namespace Todolist
{
    public class RedoCommand : ICommand
    {
        public void Execute()
        {
            try
            {
                if (AppInfo.CurrentProfile == null)
                    throw new AuthenticationException("Необходимо войти в профиль для повтора действий.");

                if (AppInfo.RedoStack.Count == 0)
                {
                    Console.WriteLine("Нет действий для повтора.");
                    return;
                }

                var command = AppInfo.RedoStack.Pop();
                command.Execute();
                
                Console.WriteLine("Повторено последнее отмененное действие.");
            }
            catch (Exception ex) when (!(ex is AuthenticationException))
            {
                throw;
            }
        }
    }
}