using Application.Dto;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;

namespace Application.UseCase.TodoTaskUseCases;

/// <summary>
/// Use case для подсчета задач по критериям.
/// </summary>
public class CountTasksUseCase : IQueryUseCase<int>
{
	private readonly ITodoTaskRepository _repository;
	private readonly TodoTaskDto.TodoTaskSearchDto _searchDto;

	public CountTasksUseCase(
		ITodoTaskRepository repository,
		TodoTaskDto.TodoTaskSearchDto searchDto)
	{
		_repository = repository;
		_searchDto = searchDto;
	}

	/// <summary>
	/// Подсчитывает количество задач, соответствующих критериям из DTO.
	/// </summary>
	/// <returns>Количество найденных задач.</returns>
	public async Task<int> Execute()
	{
		var criteria = _searchDto.ToTaskCriteria();
		return await _repository.CountAsync(criteria);
	}
}
