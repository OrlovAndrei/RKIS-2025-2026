using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data
{
    public class ProfileRepository
    {
        public async Task<List<Profile>> GetAllAsync()
        {
            using var context = new AppDbContext();
            return await context.Profiles
                .AsNoTracking()
                .OrderBy(p => p.Login)
                .ToListAsync();
        }

        public async Task<Profile?> GetByIdAsync(Guid id)
        {
            using var context = new AppDbContext();
            return await context.Profiles.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Profile?> GetByLoginAsync(string login)
        {
            using var context = new AppDbContext();
            return await context.Profiles.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Login.ToLower() == login.ToLower());
        }

        public async Task AddAsync(Profile profile)
        {
            using var context = new AppDbContext();
            profile.Id = Guid.NewGuid();
            context.Profiles.Add(profile);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Profile profile)
        {
            using var context = new AppDbContext();
            var existing = await context.Profiles.FirstOrDefaultAsync(p => p.Id == profile.Id);
            if (existing != null)
            {
                existing.Login = profile.Login;
                existing.Password = profile.Password;
                existing.FirstName = profile.FirstName;
                existing.LastName = profile.LastName;
                existing.BirthYear = profile.BirthYear;
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid profileId)
        {
            using var context = new AppDbContext();
            var profile = await context.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
            if (profile != null)
            {
                context.Profiles.Remove(profile);
                await context.SaveChangesAsync();
            }
        }

        public async Task ReplaceAllAsync(IEnumerable<Profile> profiles)
        {
            using var context = new AppDbContext();
            context.Profiles.RemoveRange(context.Profiles);
            await context.SaveChangesAsync();
            
            foreach (var profile in profiles)
            {
                context.Profiles.Add(profile);
            }
            await context.SaveChangesAsync();
        }
    }
}