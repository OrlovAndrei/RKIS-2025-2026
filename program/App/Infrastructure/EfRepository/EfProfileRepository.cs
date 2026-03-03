using Application.Interfaces.Repository;
using Application.Specifications;
using Domain.Entities.ProfileEntity;
using Infrastructure.Database;
using Infrastructure.Database.Entity;
using Infrastructure.EfRepository.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EfRepository;

public class EfProfileRepository(TodoContext context) : IProfileRepository
{
	private readonly TodoContext _context = context;

	public async Task<int> AddAsync(Profile profile)
	{
		_context.Profiles.Add(profile.ToEntity());
		return await _context.SaveChangesAsync();
	}

	public async Task<int> DeleteAsync(Guid id)
	{
		var profile = _context.Profiles.Find(id);
		if (profile is not null)
		{
			_context.Profiles.Remove(profile);
			return await _context.SaveChangesAsync();
		}
		else
		{
			throw new KeyNotFoundException($"Profile with id {id} not found.");
		}
	}

	public async Task<IEnumerable<Profile>> GetAllAsync()
	{
		return await _context.Profiles.Select(p => p.ToDomain()).ToArrayAsync();
	}
	public async Task<Profile?> GetByIdAsync(Guid id)
	{
		return await _context.Profiles.FindAsync(id.ToString()) is var profileEntity && profileEntity is not null
			? profileEntity.ToDomain()
			: null;
	}

	public async Task<int> UpdateAsync(Profile profile)
	{
		var existingProfile = _context.Profiles.Find(profile.ProfileId);
		if (existingProfile is not null)
		{
			existingProfile.FirstName = profile.FirstName;
			existingProfile.LastName = profile.LastName;
			existingProfile.DateOfBirth = profile.DateOfBirth;
			return await _context.SaveChangesAsync();
		}
		else
		{
			throw new KeyNotFoundException($"Profile with id {profile.ProfileId} not found.");
		}
	}
	private static async Task<IQueryable<ProfileEntity>> ApplyCriteriaAsync(
		IQueryable<ProfileEntity> query,
		ProfileCriteria profileCriteria)
	{
		if (profileCriteria.ProfileId is not null)
		{
			query = query.Where(p => p.ProfileId == profileCriteria.ProfileId.Value.ToString());
		}
		if (profileCriteria.FirstName is not null)
		{
			query = query.Where(p => p.FirstName.Contains(profileCriteria.FirstName.Value));
		}
		if (profileCriteria.LastName is not null)
		{
			query = query.Where(p => p.LastName.Contains(profileCriteria.LastName.Value));
		}
		if (profileCriteria.DateOfBirth is not null)
		{
			query = query.Where(p => p.DateOfBirth >= profileCriteria.DateOfBirth.Value.From &&
				p.DateOfBirth <= profileCriteria.DateOfBirth.Value.To);
		}
		if (profileCriteria.CreatedAt is not null)
		{
			query = query.Where(p => p.CreatedAt >= profileCriteria.CreatedAt.Value.From &&
				p.CreatedAt <= profileCriteria.CreatedAt.Value.To);
		}
		return await Task.FromResult(query);
	}
	public async Task<IEnumerable<Profile>> FindAsync(ProfileCriteria profileCriteria)
	{
		var query = _context.Profiles.AsQueryable();
		query = await ApplyCriteriaAsync(query, profileCriteria);
		return (await query.ToArrayAsync()).Select(p => p.ToDomain()).ToArray();
	}
	public async Task<Profile?> FindSingleAsync(ProfileCriteria profileCriteria)
	{
		var query = _context.Profiles.AsQueryable();
		query = await ApplyCriteriaAsync(query, profileCriteria);
        var result = await query.FirstOrDefaultAsync();
        return result?.ToDomain();
    }
	public async Task<bool> ExistsAsync(ProfileCriteria profileCriteria)
    {
        var query = _context.Profiles.AsQueryable();
        query = await ApplyCriteriaAsync(query, profileCriteria);
        return await query.AnyAsync();
    }
	public Task<int> CountAsync(ProfileCriteria profileCriteria)
	{
		var query = _context.Profiles.AsQueryable();
		query = ApplyCriteriaAsync(query, profileCriteria).Result;
		return query.CountAsync();
	}
}