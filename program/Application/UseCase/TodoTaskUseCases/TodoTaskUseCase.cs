using Domain.Interfaces;

namespace Application.UseCase.TodoTaskUseCases;

public class TodoTaskUseCase(ITodoTaskRepository repository)
{
	private readonly ITodoTaskRepository _repo = repository;
}