using Application.Interfaces.Command;

namespace Application.Interfaces;

public interface ICommandManager
{
    Task<int> ExecuteCommandAsync(ICommandWithUndo command);
    Task<int> Undo();
    Task<int> Redo();
}