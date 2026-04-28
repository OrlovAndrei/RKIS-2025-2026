using System;
using Todolist.Exceptions;

namespace Todolist.Commands
{
    internal class RedoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.CurrentProfileId == Guid.Empty)
                throw new AuthenticationException("Необходимо войти в профиль для работы с задачами.");
            if (AppInfo.RedoStack.Count == 0)
                throw new InvalidArgumentException("Нет команд для повтора.");

            IUndo lastCommand = AppInfo.RedoStack.Pop();
            lastCommand.Execute();
            AppInfo.UndoStack.Push(lastCommand);
            Console.WriteLine("Команда выполнена повторно.");
        }
    }
}
