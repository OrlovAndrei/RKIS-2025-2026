using Application.Interfaces;
using Domain;
using Infrastructure.Database;
using Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EfRepository;

public class EfProfileRepository(TodoContext context) : IProfileRepository
{
	private readonly TodoContext _context = context;

	public async Task<int> AddAsync(Profile profile)
	{
		_context.Profiles.Add(ProfileMapper.ToEntity(profile));
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
		return await _context.Profiles.Select(p => ProfileMapper.ToDomain(p)).ToArrayAsync();
	}

	public async Task<Profile?> GetByIdAsync(Guid id)
	{
		return await _context.Profiles.FindAsync(id) is var profileEntity && profileEntity is not null
			? ProfileMapper.ToDomain(profileEntity)
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
}