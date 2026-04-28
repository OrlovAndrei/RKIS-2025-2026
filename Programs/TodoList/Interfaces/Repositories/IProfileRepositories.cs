using TodoList.Entity;

namespace TodoList.Interfaces.Repositories;

public interface IProfileRepositories : IBaseCrudRepositories<Profile, Guid>
{
    Task<IEnumerable<Profile>> All();
}