public interface IProfileRepository
{
    Task<List<Profile>> GetAllAsync();
    Task<Profile?> GetByIdAsync(Guid id);
    Task<Profile?> GetByLoginAsync(string login);
    Task AddAsync(Profile profile);
    Task DeleteAsync(Guid id);
}