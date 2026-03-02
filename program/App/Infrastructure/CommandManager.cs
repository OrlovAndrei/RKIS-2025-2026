using Application.Interfaces;
using Application.Interfaces.Command;

namespace Infrastructure;

public class CommandManager : ICommandManager
{
    private readonly Stack<ICommandWithUndo> _undoStack = new();
    private readonly Stack<ICommandWithUndo> _redoStack = new();

    public async Task<int> ExecuteCommandAsync(ICommandWithUndo command)
    {
        int result = await command.Execute();
        if (result != 0)
        {
            _undoStack.Push(command);
            _redoStack.Clear();
        }
        return result;
    }

    public async Task<int> Redo()
    {
        if (_redoStack.Count > 0)
        {
            var command = _redoStack.Peek();
            int result = await command.Execute();
            if (result != 0)
            {
                _undoStack.Push(command);
                _redoStack.Pop();
            }
            return result;
        }
        throw new StackOverflowException(message: "The stack is empty.");
    }

    public async Task<int> Undo()
    {
        if (_undoStack.Count > 0)
        {
            var command = _undoStack.Peek();
            int result = await command.Undo();
            if (result != 0)
            {
                _redoStack.Push(command);
                _undoStack.Pop();
            }
            return result;
        }
        throw new StackOverflowException(message: "The stack is empty.");
    }
}