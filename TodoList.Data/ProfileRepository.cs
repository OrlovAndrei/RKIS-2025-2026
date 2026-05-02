using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Models;

namespace TodoList.Services
{
    public class ProfileRepository : IProfileRepository
    {
        public async Task<IEnumerable<Profile>> GetAllAsync()
        {
            using var context = new AppDbContext();
            return await context.Profiles.ToListAsync();
        }

        public async Task<Profile?> GetByIdAsync(Guid id)
        {
            using var context = new AppDbContext();
            return await context.Profiles.FindAsync(id);
        }

        public async Task<Profile?> GetByLoginAsync(string login)
        {
            using var context = new AppDbContext();
            return await context.Profiles
                .FirstOrDefaultAsync(p => p.Login == login);
        }

        public async Task<Profile> AddAsync(Profile profile)
        {
            using var context = new AppDbContext();
            context.Profiles.Add(profile);
            await context.SaveChangesAsync();
            return profile;
        }

        public async Task<bool> UpdateAsync(Profile profile)
        {
            using var context = new AppDbContext();
            var existing = await context.Profiles.FindAsync(profile.Id);
            
            if (existing == null) return false;

            existing.Login = profile.Login;
            existing.Password = profile.Password;
            existing.FirstName = profile.FirstName;
            existing.LastName = profile.LastName;
            existing.BirthYear = profile.BirthYear;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var context = new AppDbContext();
            var profile = await context.Profiles.FindAsync(id);
            
            if (profile == null) return false;

            context.Profiles.Remove(profile);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
