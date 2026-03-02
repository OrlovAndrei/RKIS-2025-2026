using Application.Dto;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;

namespace Application.UseCase.TodoTaskUseCases.Query;

/// <summary>
/// Use case для поиска задач по критериям.
/// </summary>
public class FindTasksUseCase : IQueryUseCase<IEnumerable<TodoTaskDto.TodoTaskDetailsDto>>
{
	private readonly ITodoTaskRepository _repository;
	private readonly TodoTaskDto.TodoTaskSearchDto _searchDto;

	public FindTasksUseCase(
		ITodoTaskRepository repository,
		TodoTaskDto.TodoTaskSearchDto searchDto)
	{
		_repository = repository;
		_searchDto = searchDto;
	}

	/// <summary>
	/// Выполняет поиск задач по критериям из DTO.
	/// </summary>
	/// <returns>Коллекция найденных задач в виде TodoTaskDetailsDto.</returns>
	public async Task<IEnumerable<TodoTaskDto.TodoTaskDetailsDto>> Execute()
	{
		var criteria = _searchDto.ToTaskCriteria();
		var tasks = await _repository.FindAsync(criteria);
		return tasks.Select(t => t.ToDetailsDto());
	}
}
