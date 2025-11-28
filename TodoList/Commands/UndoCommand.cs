using System;

namespace Todolist
{
    public class UndoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.UndoStack.Count == 0)
            {
                Console.WriteLine("Нет команд для отмены");
                return;
            }

            ICommand command = AppInfo.UndoStack.Pop();
            command.Unexecute();
            AppInfo.RedoStack.Push(command);
            
            FileManager.SaveTodos(AppInfo.Todos, Program.TodoFilePath);
        }

        public void Unexecute()
        {
            throw new NotImplementedException();
        }
    }
}