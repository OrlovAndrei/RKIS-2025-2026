using System;
using Todolist.Exceptions;

namespace Todolist
{
    public class UndoCommand : ICommand
    {
        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
                throw new AuthenticationException("Необходимо войти в профиль");

            if (AppInfo.UndoStack.Count == 0)
                throw new InvalidCommandException("Нет команд для отмены");

            ICommand command = AppInfo.UndoStack.Pop();
            command.Unexecute();
            AppInfo.RedoStack.Push(command);
        }

        public void Unexecute()
        {
            throw new NotImplementedException();
        }
    }
}