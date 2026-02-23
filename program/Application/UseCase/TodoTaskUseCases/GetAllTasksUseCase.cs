using Application.Dto;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;

namespace Application.UseCase.TodoTaskUseCases;

/// <summary>
/// Use case для получения всех задач.
/// </summary>
public class GetAllTasksUseCase : IQueryUseCase<IEnumerable<TodoTaskDto.TodoTaskDetailsDto>>
{
	private readonly ITodoTaskRepository _repository;

	public GetAllTasksUseCase(ITodoTaskRepository repository)
	{
		_repository = repository;
	}

	/// <summary>
	/// Получает все задачи из репозитория.
	/// </summary>
	/// <returns>Коллекция всех задач в виде TodoTaskDetailsDto.</returns>
	public async Task<IEnumerable<TodoTaskDto.TodoTaskDetailsDto>> Execute()
	{
		var tasks = await _repository.GetAllAsync();
		return tasks.Select(t => t.ToDetailsDto());
	}
}
