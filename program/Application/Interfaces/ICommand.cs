namespace Application.Interfaces;

/// <summary>
/// Интерфейс для command use case с возвращаемым результатом типа TResult.
/// Command операции изменяют состояние системы и возвращают результат.
/// </summary>
/// <typeparam name="TResult">Тип возвращаемого результата.</typeparam>
public interface ICommand<TResult> : IOperation<TResult>
{
}
