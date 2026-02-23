using Application.Dto;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.UseCase.TodoTaskUseCases;

/// <summary>
/// Use case для получения одной задачи по критериям.
/// </summary>
public class GetTaskUseCase : IQueryUseCase<TodoTaskDto.TodoTaskDetailsDto?>
{
	private readonly ITodoTaskRepository _repository;
	private readonly TodoTaskDto.TodoTaskSearchDto _searchDto;

	public GetTaskUseCase(
		ITodoTaskRepository repository,
		TodoTaskDto.TodoTaskSearchDto searchDto)
	{
		_repository = repository;
		_searchDto = searchDto;
	}

	/// <summary>
	/// Выполняет поиск одной задачи по критериям из DTO.
	/// </summary>
	/// <returns>Найденная задача в виде TodoTaskDetailsDto или null.</returns>
	public async Task<TodoTaskDto.TodoTaskDetailsDto?> Execute()
	{
		var criteria = _searchDto.ToTaskCriteria();
		var task = await _repository.FindSingleAsync(criteria);
		return task?.ToDetailsDto();
	}
}
