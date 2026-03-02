using Application.Dto;
using Application.Interfaces;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;
using Domain.Entities.TaskEntity;

namespace Application.UseCase.TodoTaskUseCases;

public class AddNewTaskUseCase : ICommandWithUndo
{
	private readonly ITodoTaskRepository _repo;
	private readonly IUserContext _userContext;
	private readonly TodoTaskDto.TodoTaskCreateDto _taskCreate;
	private readonly TodoTask _newTodoTask;
	public AddNewTaskUseCase(
		ITodoTaskRepository repository,
		IUserContext userContext,
		TodoTaskDto.TodoTaskCreateDto taskCreate
	)
	{
		_repo = repository;
		_userContext = userContext;
		_taskCreate = taskCreate;
		_newTodoTask = _taskCreate.FromCreateDto(userContext: _userContext);
	}
	public async Task<int> Execute()
	{
		return await _repo.AddAsync(_newTodoTask);
	}

	public async Task<int> Undo()
	{
		return await _repo.DeleteAsync(_newTodoTask.TaskId);
	}
}