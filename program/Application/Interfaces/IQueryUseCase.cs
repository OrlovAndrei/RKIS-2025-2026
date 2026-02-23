namespace Application.Interfaces;

/// <summary>
/// Интерфейс для query use case (поиск, получение данных).
/// Query операции не изменяют состояние системы.
/// </summary>
/// <typeparam name="TResult">Тип возвращаемого результата.</typeparam>
public interface IQueryUseCase<TResult> : IOperation<TResult>
{
}
