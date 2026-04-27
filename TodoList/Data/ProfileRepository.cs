using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _context;
        private Profile? _currentProfile;

        public ProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Profile?> GetByIdAsync(Guid id)
        {
            return await _context.Profiles.FindAsync(id);
        }

        public async Task<Profile?> GetByLoginAsync(string login)
        {
            return await _context.Profiles.FirstOrDefaultAsync(p => p.Login == login);
        }

        public async Task<List<Profile>> GetAllAsync()
        {
            return await _context.Profiles.ToListAsync();
        }

        public async Task AddAsync(Profile profile)
        {
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Profile profile)
        {
            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile != null)
            {
                _context.Profiles.Remove(profile);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Profile?> AuthenticateAsync(string login, string password)
        {
            var profile = await GetByLoginAsync(login);
            if (profile != null && profile.Password == password)
                return profile;
            return null;
        }

        public Task<Profile?> GetCurrentProfileAsync() => Task.FromResult(_currentProfile);
        public Task SetCurrentProfileAsync(Profile profile) { _currentProfile = profile; return Task.CompletedTask; }
        public Task LogoutAsync() { _currentProfile = null; return Task.CompletedTask; }
    }
}