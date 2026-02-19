using Application.Interfaces;

namespace Application.UseCase.TaskStateUseCases;

public class TaskStateUseCase(IStateRepository repository)
{
    private readonly IStateRepository _repo = repository;
}