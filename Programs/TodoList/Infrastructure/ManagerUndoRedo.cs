using TodoList.Interfaces;

namespace TodoList.Infrastructure;

public class ManagerUndoRedo : IControllerUndoRedo
{
    public Stack<ICommandWithUndo<object>> UndoStack { get; init; }
    public Stack<ICommandWithUndo<object>> RedoStack { get; init; }
    public ManagerUndoRedo()
    {
        UndoStack = new();
        RedoStack = new();
    }
    public async Task Execute(ICommand command)
    {
        await command.Execute();
    }
    public async Task Execute(ICommandWithUndo<object> command, object obj)
    {
        await command.Execute(obj);
        UndoStack.Push(command);
    }
    public async Task Redo()
    {
        if (RedoStack.Count > 0)
        {
            var command = RedoStack.Pop();
            await command.Execute();
            UndoStack.Push(command);
            return;
        }
        throw new SystemException();
    }
    public async Task Undo()
    {
        if (UndoStack.Count > 0)
        {
            var command = UndoStack.Pop();
            await command.Unexecuted();
            RedoStack.Push(command);
            return;
        }
        throw new SystemException();
    }
}