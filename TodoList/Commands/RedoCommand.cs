using TodoList.Exceptions;

namespace TodoList
{
    public class RedoCommand : ICommand
    {
        public void Execute()
        {
            if (AppInfo.RedoStack.Count == 0)
                throw new InvalidCommandException("Нет действий для повтора.");

            var command = AppInfo.RedoStack.Pop();
            command.Execute();
            Console.WriteLine("Повторено последнее отмененное действие.");
        }

        public void Unexecute() { }
    }
}