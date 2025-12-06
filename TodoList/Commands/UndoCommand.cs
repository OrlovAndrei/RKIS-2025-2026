using System;

namespace Todolist
{
    public class UndoCommand : ICommand
    {
        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
            {
                Console.WriteLine("Ошибка: необходимо войти в профиль");
                return;
            }

            if (AppInfo.UndoStack.Count == 0)
            {
                Console.WriteLine("Нет команд для отмены");
                return;
            }

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