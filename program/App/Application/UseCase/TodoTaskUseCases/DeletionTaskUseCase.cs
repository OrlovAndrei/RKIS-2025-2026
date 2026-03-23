using Application.Interfaces;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;
using Domain.Entities.TaskEntity;

namespace Application.UseCase.TodoTaskUseCases;

public class DeletionTaskUseCase : ICommandWithUndo
{
	private readonly ITodoTaskRepository _repo;
	private readonly Guid _idTodoTask;
	private readonly TodoTask _deletionTodoTask;
	private readonly IUnitOfWork _unitOfWork;
	public DeletionTaskUseCase(
		ITodoTaskRepository repository,
		IUnitOfWork unitOfWork,
		Guid idTodoTask
	)
	{
		_unitOfWork = unitOfWork;
		_repo = repository;
		_idTodoTask = idTodoTask;
		_deletionTodoTask = _repo.GetByIdAsync(id: idTodoTask).Result
			?? throw new Exception(message: "The task was not found.");
	}
	public async Task Execute()
	{
		await _repo.DeleteAsync(_idTodoTask);
		await _unitOfWork.SaveChangesAsync();
	}

	public async Task Undo()
	{
		await _repo.AddAsync(_deletionTodoTask);
		await _unitOfWork.SaveChangesAsync();
	}
}