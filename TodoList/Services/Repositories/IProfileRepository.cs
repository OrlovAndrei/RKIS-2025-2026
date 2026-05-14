using TodoList.Models;

namespace TodoList.Services.Repositories;

public interface IProfileRepository
{
    Task<IEnumerable<Profile>> GetAllAsync();
    Task<Profile?> GetByIdAsync(Guid id);
    Task<Profile?> GetByLoginAsync(string login);
    Task AddAsync(Profile profile);
    Task UpdateAsync(Profile profile);
    Task DeleteAsync(Guid id);
}