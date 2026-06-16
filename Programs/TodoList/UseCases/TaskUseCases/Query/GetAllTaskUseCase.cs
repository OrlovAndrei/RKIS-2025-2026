using TodoList.Entity;
using TodoList.Interfaces;
using TodoList.Interfaces.Repositories;

namespace TodoList.UseCases.TaskUseCases.Query;

public class GetAllTaskUseCase : IQueryCommand<IEnumerable<TodoItem>>
{
    private readonly ITaskItemRepositories _taskItemRepositories;
    private readonly ICurrentProfile _currentProfile;
    public GetAllTaskUseCase(
        ITaskItemRepositories taskItemRepositories,
        ICurrentProfile currentProfile
    )
    {
        _taskItemRepositories = taskItemRepositories;
        _currentProfile = currentProfile;
    }

	public async Task<IEnumerable<TodoItem>> Execute()
	{
		return await _taskItemRepositories.FindAsync(t => t.ProfileId == _currentProfile.Id);
	}

	Task ICommand.Execute()
	{
		return Execute();
	}
}