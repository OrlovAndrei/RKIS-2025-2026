using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data
{
    public class ProfileRepository
    {
        public async Task<List<Profile>> GetAllAsync()
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
            return await context.Profiles.FirstOrDefaultAsync(p => p.Login == login);
        }

        public async Task AddAsync(Profile profile)
        {
            using var context = new AppDbContext();
            context.Profiles.Add(profile);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Profile profile)
        {
            using var context = new AppDbContext();
            context.Profiles.Update(profile);
            await context.SaveChangesAsync();
        }
    }
}