using Application.Dto;
using Application.Interfaces;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;

namespace Application.UseCase.TodoTaskUseCases.Query;

/// <summary>
/// Use case для получения всех задач.
/// </summary>
public class GetAllTasksUseCase : IQueryUseCase<IEnumerable<TodoTaskDto.TodoTaskDetailsDto>>
{
	private readonly ITodoTaskRepository _repository;
	private readonly IUserContext _userContext;

	public GetAllTasksUseCase(ITodoTaskRepository repository, IUserContext userContext)
	{
		_repository = repository;
		_userContext = userContext;
	}

	/// <summary>
	/// Получает все задачи из репозитория.
	/// </summary>
	/// <returns>Коллекция всех задач в виде TodoTaskDetailsDto.</returns>
	public async Task<IEnumerable<TodoTaskDto.TodoTaskDetailsDto>> Execute()
	{
		var tasks = await _repository.GetAllAsync(_userContext);
		return tasks.Select(t => t.ToDetailsDto());
	}
}
