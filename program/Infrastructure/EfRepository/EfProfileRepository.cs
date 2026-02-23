using Application.Interfaces.Repository;
using Application.Specifications;
using Domain.Entities.ProfileEntity;
using Infrastructure.Database;
using Infrastructure.Database.Entity;
using Infrastructure.EfRepository.Mapper;
using LinqKit;
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
			var idExpr = profileCriteria.ProfileId.IsSatisfiedBy<ProfileEntity>(p => Guid.Parse(p.ProfileId));
			query = query.Where(idExpr);
		}
		if (profileCriteria.FirstName is not null)
		{
			var fnExpr = profileCriteria.FirstName.IsSatisfiedBy<ProfileEntity>(p => p.FirstName);
			query = query.Where(fnExpr);
		}
		if (profileCriteria.LastName is not null)
		{
			var lnExpr = profileCriteria.LastName.IsSatisfiedBy<ProfileEntity>(p => p.LastName);
			query = query.Where(lnExpr);
		}
		if (profileCriteria.DateOfBirth is not null)
		{
			var dobExpr = profileCriteria.DateOfBirth.IsSatisfiedBy<ProfileEntity>(p => p.DateOfBirth);
			query = query.Where(dobExpr);
		}
		if (profileCriteria.CreatedAt is not null)
		{
			var caExpr = profileCriteria.CreatedAt.IsSatisfiedBy<ProfileEntity>(p => p.CreatedAt);
			query = query.Where(caExpr);
		}
		return await Task.FromResult(query);
	}
	public async Task<IEnumerable<Profile>> FindAsync(ProfileCriteria profileCriteria)
	{
		var query = _context.Profiles.AsExpandable();
		query = await ApplyCriteriaAsync(query, profileCriteria);
		return (await query.ToArrayAsync()).Select(p => p.ToDomain()).ToArray();
	}
	public async Task<Profile?> FindSingleAsync(ProfileCriteria profileCriteria)
	{
		var query = _context.Profiles.AsExpandable();
		query = await ApplyCriteriaAsync(query, profileCriteria);
        var result = await query.FirstOrDefaultAsync();
        return result?.ToDomain();
    }
	public async Task<bool> ExistsAsync(ProfileCriteria profileCriteria)
    {
        var query = _context.Profiles.AsExpandable();
        query = await ApplyCriteriaAsync(query, profileCriteria);
        return await query.AnyAsync();
    }
	public Task<int> CountAsync(ProfileCriteria profileCriteria)
	{
		var query = _context.Profiles.AsExpandable();
		query = ApplyCriteriaAsync(query, profileCriteria).Result;
		return query.CountAsync();
	}
}