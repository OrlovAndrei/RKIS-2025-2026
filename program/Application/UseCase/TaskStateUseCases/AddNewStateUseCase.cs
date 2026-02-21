using Application.Dto;
using Application.Interfaces;
using Domain;

namespace Application.UseCase.TaskStateUseCases;

public class AddNewStateUseCase : IUndoRedo
{
    private readonly ITodoTaskRepository _repo;
    private readonly ICurrentUserService _currentUser;
    private readonly TodoTaskDto.TodoTaskCreateDto _taskCreate;
    private readonly TodoTask _newTodoTask;
    public AddNewStateUseCase(
        ITodoTaskRepository repository,
        ICurrentUserService currentUser,
        TodoTaskDto.TodoTaskCreateDto taskCreate
    )
    {
        _repo = repository;
        _currentUser = currentUser;
        _taskCreate = taskCreate;
        _newTodoTask = _taskCreate.FromCreateDto(currentUser: _currentUser);
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