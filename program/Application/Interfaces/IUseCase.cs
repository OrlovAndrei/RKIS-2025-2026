namespace Application.Interfaces;

/// <summary>
/// Интерфейс для use case с возвращаемым результатом типа TResult.
/// Используется для всех типов операций (запросы, команды).
/// </summary>
/// <typeparam name="TResult">Тип возвращаемого результата.</typeparam>
public interface IUseCase<TResult> : IOperation<TResult>
{
}
