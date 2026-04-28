using Application.Interfaces.Command;

namespace Application.Interfaces;

public interface ICommandManager
{
    Task ExecuteCommandAsync(ICommandWithUndo command);
    Task Undo();
    Task Redo();
}