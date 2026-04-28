using System.Linq.Expressions;
using Application.Interfaces.Repository;
using Domain.Entities.ProfileEntity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EfRepository;

public class EfProfileRepository(TodoContext context) : IProfileRepository
{
	private readonly TodoContext _context = context;

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
		var existingProfile = _context.Profiles.Find(profile.ProfileId);
		if (existingProfile is not null)
		{
			existingProfile.UpdateFirstName(profile.FirstName);
			existingProfile.UpdateLastName(profile.LastName);
			existingProfile.UpdateDateOfBirth(profile.DateOfBirth);
		}
		else
		{
			throw new KeyNotFoundException($"Profile with id {profile.ProfileId} not found.");
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
	public Task<int> CountAsync(Expression<Func<Profile, bool>> predicate)
	{
		var query = _context.Profiles.Where(predicate);
		return query.CountAsync();
	}
}