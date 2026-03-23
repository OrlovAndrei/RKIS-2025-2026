using Application.Interfaces;
using Application.Interfaces.Command;

namespace Infrastructure;

public class CommandManager : ICommandManager
{
    private readonly Stack<ICommandWithUndo> _undoStack = new();
    private readonly Stack<ICommandWithUndo> _redoStack = new();

    public async Task ExecuteCommandAsync(ICommandWithUndo command)
    {
        await command.Execute();
        _undoStack.Push(command);
        _redoStack.Clear();
    }

    public async Task Redo()
    {
        if (_redoStack.Count > 0)
        {
            var command = _redoStack.Peek();
            await command.Execute();
            _undoStack.Push(command);
            _redoStack.Pop();
        }
        throw new StackOverflowException(message: "The stack is empty.");
    }

    public async Task Undo()
    {
        if (_undoStack.Count > 0)
        {
            var command = _undoStack.Peek();
            await command.Undo();
            _redoStack.Push(command);
            _undoStack.Pop();
        }
        throw new StackOverflowException(message: "The stack is empty.");
    }
}