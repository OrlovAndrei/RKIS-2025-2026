using System;
using Todolist.Exceptions;

namespace Todolist
{
    public class RedoCommand : ICommand
    {
        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
                throw new AuthenticationException("Необходимо войти в профиль");

            if (AppInfo.RedoStack.Count == 0)
                throw new InvalidCommandException("Нет команд для повтора");

            ICommand command = AppInfo.RedoStack.Pop();
            command.Execute();
            AppInfo.UndoStack.Push(command);
        }

        public void Unexecute()
        {
            throw new NotImplementedException();
        }
    }
}