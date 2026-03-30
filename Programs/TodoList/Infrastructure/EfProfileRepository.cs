using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoList.Database;
using TodoList.Entity;
using TodoList.Interfaces.Repositories;

namespace Infrastructure.EfRepository;

public class EfProfileRepository(ApplicationContext context) : IProfileRepositories
{
	private readonly ApplicationContext _context = context;

	public async Task AddAsync(Profile profile)
	{
		_context.Profiles.Add(profile);
	}

	public async Task DeleteAsync(Guid id)
	{
		var profile = _context.Profiles.Find(id);
		if (profile is not null)
		{
			_context.Profiles.Remove(profile);
		}
		else
		{
			throw new KeyNotFoundException($"Profile with id {id} not found.");
		}
	}

	public async Task<IEnumerable<Profile>> GetAllAsync()
	{
		return await _context.Profiles.Select(p => p).ToArrayAsync();
	}
	public async Task<Profile?> GetByIdAsync(Guid id)
	{
		return await _context.Profiles.FindAsync(id);
	}

	public async Task UpdateAsync(Profile profile)
	{
		var existingProfile = _context.Profiles.Find(profile.Id);
		if (existingProfile is not null)
		{
			existingProfile.UpdateFirstName(profile.FirstName);
			existingProfile.UpdateLastName(profile.LastName);
			existingProfile.UpdateDateOfBirth(profile.DateOfBirth);
		}
		else
		{
			throw new KeyNotFoundException($"Profile with id {profile.Id} not found.");
		}
	}
	public async Task<IEnumerable<Profile>> FindAsync(Expression<Func<Profile, bool>> predicate)
	{
		var query = _context.Profiles.Where(predicate);
		return await query.ToArrayAsync();
	}
	public async Task<Profile?> FindSingleAsync(Expression<Func<Profile, bool>> predicate)
	{
		var query = _context.Profiles.Where(predicate);
        return await query.FirstOrDefaultAsync();
    }
	public async Task<bool> ExistsAsync(Expression<Func<Profile, bool>> predicate)
    {
        var query = _context.Profiles.Where(predicate);
        return await query.AnyAsync();
    }
	public async Task<int> CountAsync(Expression<Func<Profile, bool>> predicate)
	{
		var query = _context.Profiles.AsParallel().Where(predicate.Compile()).AsQueryable();
		return await query.CountAsync();
	}

	public async Task<IEnumerable<Profile>> All()
	{
		return _context.Profiles.Select(p => p).AsEnumerable();
	}
}