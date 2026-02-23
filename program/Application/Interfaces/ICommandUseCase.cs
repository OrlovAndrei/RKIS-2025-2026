namespace Application.Interfaces;

/// <summary>
/// Интерфейс для command use case (добавление, обновление, удаление).
/// Command операции изменяют состояние системы и возвращают количество затронутых записей.
/// </summary>
public interface ICommandUseCase : IOperation<int>
{
}
