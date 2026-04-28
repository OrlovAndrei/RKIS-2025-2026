namespace Application.Interfaces.Command;

/// <summary>
/// Главный интерфейс для всех операций (команд и запросов) в приложении.
/// Представляет абсолютно все use cases - как query, так и command операции.
/// </summary>
public interface IOperation
{
	/// <summary>
	/// Выполняет операцию.
	/// </summary>
	Task Execute();
}
