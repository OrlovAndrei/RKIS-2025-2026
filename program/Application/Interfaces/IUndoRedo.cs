namespace Application.Interfaces;

/// <summary>
/// Интерфейс для use case с поддержкой отмены/повтора операций.
/// </summary>
public interface IUndoRedo : ICommandWithUndo
{
}