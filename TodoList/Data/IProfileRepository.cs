using TodoApp.Models;

namespace TodoApp.Data
{
    public interface IProfileRepository
    {
        Task<Profile?> GetByIdAsync(Guid id);
        Task<Profile?> GetByLoginAsync(string login);
        Task<List<Profile>> GetAllAsync();
        Task AddAsync(Profile profile);
        Task UpdateAsync(Profile profile);
        Task DeleteAsync(Guid id);
        Task<Profile?> AuthenticateAsync(string login, string password);
        Task<Profile?> GetCurrentProfileAsync();
        Task SetCurrentProfileAsync(Profile profile);
        Task LogoutAsync();
    }
}