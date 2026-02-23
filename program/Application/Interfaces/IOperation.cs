namespace Application.Interfaces;

/// <summary>
/// Главный интерфейс для всех операций (команд и запросов) в приложении.
/// Представляет абсолютно все use cases - как query, так и command операции.
/// </summary>
/// <typeparam name="TResult">Тип возвращаемого результата операции.</typeparam>
public interface IOperation<TResult>
{
	/// <summary>
	/// Выполняет операцию и возвращает результат.
	/// </summary>
	/// <returns>Результат выполнения операции типа TResult.</returns>
	Task<TResult> Execute();
}
