using Application.Interfaces;
using Domain.Entities.TaskEntity;
using Domain.Interfaces;

namespace Application.UseCase.TodoTaskUseCases;

public class DeletionTaskUseCase : IUndoRedo
{
	private readonly ITodoTaskRepository _repo;
	private readonly Guid _idTodoTask;
	private readonly TodoTask _deletionTodoTask;
	public DeletionTaskUseCase(
		ITodoTaskRepository repository,
		Guid idTodoTask
	)
	{
		_repo = repository;
		_idTodoTask = idTodoTask;
		_deletionTodoTask = _repo.GetByIdAsync(id: idTodoTask).Result
			?? throw new Exception(message: "The task was not found.");
	}
	public async Task<int> Execute()
	{
		return await _repo.DeleteAsync(_idTodoTask);
	}

	public async Task<int> Undo()
	{
		return await _repo.AddAsync(_deletionTodoTask);
	}
}