using Microsoft.EntityFrameworkCore;

public class ProfileRepository : IProfileRepository
{
    public async Task<List<Profile>> GetAllAsync()
    {
        await using var ctx = new AppDbContext();
        return await ctx.Profiles.Include(p => p.Todos).ToListAsync();
    }

    public async Task<Profile?> GetByIdAsync(Guid id)
    {
        await using var ctx = new AppDbContext();
        return await ctx.Profiles.Include(p => p.Todos).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Profile?> GetByLoginAsync(string login)
    {
        await using var ctx = new AppDbContext();
        return await ctx.Profiles.FirstOrDefaultAsync(p => p.Login == login);
    }

    public async Task AddAsync(Profile profile)
    {
        await using var ctx = new AppDbContext();
        await ctx.Profiles.AddAsync(profile);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var ctx = new AppDbContext();
        var profile = await ctx.Profiles.FindAsync(id);
        if (profile != null)
        {
            ctx.Profiles.Remove(profile);
            await ctx.SaveChangesAsync();
        }
    }
}