using Application.Dto;
using Application.Interfaces;
using Domain;

namespace Application.UseCase.TaskStateUseCases;

public class UpdateStateUseCase : IUndoRedo
{
    private readonly ITodoTaskRepository _repo;
    private readonly TodoTask _oldTodoTask;
    private readonly TodoTaskDto.TodoTaskUpdateDto _updateDto;
    public UpdateStateUseCase(
        ITodoTaskRepository repository,
        TodoTaskDto.TodoTaskUpdateDto updateDto
    )
    {
        _repo = repository;
        _oldTodoTask = _repo.GetByIdAsync(id: updateDto.TaskId).Result
            ?? throw new Exception(message: "The task was not found.");
        _updateDto = updateDto;
    }
    public async Task<int> Execute()
    {
        return await _repo.UpdateAsync(_updateDto.FromUpdateDto());
    }

    public async Task<int> Undo()
    {
        return await _repo.UpdateAsync(_oldTodoTask);
    }
}