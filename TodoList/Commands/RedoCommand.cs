using System;

namespace Todolist
{
    public class RedoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.RedoStack.Count == 0)
            {
                Console.WriteLine("Нет команд для повтора");
                return;
            }

            ICommand command = AppInfo.RedoStack.Pop();
            command.Execute();
            AppInfo.UndoStack.Push(command);
            
            FileManager.SaveTodos(AppInfo.Todos, Program.TodoFilePath);
        }

        public void Unexecute()
        {
            throw new NotImplementedException();
        }
    }
}