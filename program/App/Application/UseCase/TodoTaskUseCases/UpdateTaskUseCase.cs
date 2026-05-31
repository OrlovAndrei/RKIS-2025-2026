using Application.Dto;
using Application.Interfaces;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;
using Domain.Entities.TaskEntity;

namespace Application.UseCase.TodoTaskUseCases;

public class UpdateTaskUseCase : ICommandWithUndo
{
	private readonly ITodoTaskRepository _repo;
	private readonly TodoTask _oldTodoTask;
	private readonly TodoTaskDto.TodoTaskUpdateDto _updateDto;
	private readonly IUnitOfWork _unitOfWork;
	public UpdateTaskUseCase(
		IUnitOfWork unitOfWork,
		ITodoTaskRepository repository,
		TodoTaskDto.TodoTaskUpdateDto updateDto
	)
	{
		_unitOfWork = unitOfWork;
		_repo = repository;
		_oldTodoTask = _repo.GetByIdAsync(id: updateDto.TaskId).Result
			?? throw new Exception(message: "The task was not found.");
		_updateDto = updateDto;
	}
	public async Task Execute()
	{
		await _repo.UpdateAsync(_updateDto.FromUpdateDto());
		await _unitOfWork.SaveChangesAsync();
	}

	public async Task Undo()
	{
		await _repo.UpdateAsync(_oldTodoTask);
		await _unitOfWork.SaveChangesAsync();
	}
}