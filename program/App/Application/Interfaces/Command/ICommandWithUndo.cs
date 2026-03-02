namespace Application.Interfaces.Command;

/// <summary>
/// Интерфейс для command use case с поддержкой отмены операции.
/// </summary>
public interface ICommandWithUndo : ICommandUseCase
{
	/// <summary>
	/// Отменяет выполненную операцию.
	/// </summary>
	/// <returns>Количество затронутых записей при отмене.</returns>
	Task<int> Undo();
}
