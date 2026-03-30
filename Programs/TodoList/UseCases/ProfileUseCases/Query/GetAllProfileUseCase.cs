using TodoList.Entity;
using TodoList.Interfaces;
using TodoList.Interfaces.Repositories;

namespace TodoList.UseCases.ProfileUseCases.Query;

public class GetAllProfileUseCase : IQueryCommand<IEnumerable<Profile>>
{
    private readonly IProfileRepositories _profileRepositories;
    public GetAllProfileUseCase(
        IProfileRepositories profileRepositories
    )
    {
        _profileRepositories = profileRepositories;
    }

	public async Task<IEnumerable<Profile>> Execute()
	{
		return await _profileRepositories.All();
	}

	Task ICommand.Execute()
	{
		return Execute();
	}
}