using TodoList.Models;

namespace TodoList.Data
{
    public interface IProfileRepository
    {
        Task<IEnumerable<Profile>> GetAllAsync();
        Task<Profile?> GetByIdAsync(Guid id);
        Task<Profile?> GetByLoginAsync(string login);
        Task<Profile> AddAsync(Profile profile);
        Task<bool> UpdateAsync(Profile profile);
        Task<bool> DeleteAsync(Guid id);
    }
}
