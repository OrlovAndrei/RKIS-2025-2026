using Application.Dto;
using Application.Interfaces;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;

namespace Application.UseCase.TodoTaskUseCases.Query;

/// <summary>
/// Use case для проверки существования задачи по критериям.
/// </summary>
public class CheckTaskExistsUseCase : IQueryUseCase<bool>
{
	private readonly ITodoTaskRepository _repository;
	private readonly TodoTaskDto.TodoTaskSearchDto _searchDto;

	public CheckTaskExistsUseCase(
		ITodoTaskRepository repository,
		TodoTaskDto.TodoTaskSearchDto searchDto)
	{
		_repository = repository;
		_searchDto = searchDto;
	}

	/// <summary>
	/// Проверяет существование задачи, соответствующей критериям из DTO.
	/// </summary>
	/// <returns>true если задача существует, иначе false.</returns>
	public async Task<bool> Execute()
	{
		var criteria = _searchDto.ToTaskCriteria();
		return await _repository.ExistsAsync(criteria);
	}
}
