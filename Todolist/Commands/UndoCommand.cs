using System;
using Todolist.Exceptions;

namespace Todolist.Commands
{
    internal class UndoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.CurrentProfileId == Guid.Empty)
                throw new AuthenticationException("Необходимо войти в профиль для работы с задачами.");
            if (AppInfo.UndoStack.Count == 0)
                throw new InvalidArgumentException("Нет команд для отмены.");
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

