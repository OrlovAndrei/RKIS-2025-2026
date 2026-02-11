using System;

namespace Todolist
{
    public class RedoCommand : ICommand
    {
        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
            {
                Console.WriteLine("Ошибка: необходимо войти в профиль");
                return;
            }

            if (AppInfo.RedoStack.Count == 0)
            {
                Console.WriteLine("Нет команд для повтора");
                return;
            }

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