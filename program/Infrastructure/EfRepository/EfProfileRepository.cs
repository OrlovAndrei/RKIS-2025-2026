using Domain;
using Application.Interfaces;
using Infrastructure.Database;
using Infrastructure.Mapper;

namespace Infrastructure.EfRepository;

public class EfProfileRepository : IProfileRepository
{
	private readonly TodoContext _context;
	public EfProfileRepository(TodoContext context)
	{
		_context = context;
	}
	public void Add(Profile profile)
	{
		_context.Profiles.Add(ProfileMapper.ToEntity(profile));
		_context.SaveChanges();
	}

	public void Delete(Guid id)
	{
		var profile = _context.Profiles.Find(id);
		if (profile is not null)
		{
			_context.Profiles.Remove(profile);
			_context.SaveChanges();
		}
		else
		{
			throw new KeyNotFoundException($"Profile with id {id} not found.");
		}
	}

	public IEnumerable<Profile> GetAll()
	{
		return _context.Profiles.Select(p => ProfileMapper.ToDomain(p)).AsEnumerable();
	}

	public Profile? GetById(Guid id)
	{
		return _context.Profiles.Find(id) is var profileEntity && profileEntity is not null
			? ProfileMapper.ToDomain(profileEntity)
			: null;
	}

	public void Update(Profile profile)
	{
		var existingProfile = _context.Profiles.Find(profile.ProfileId);
		if (existingProfile is not null)
		{
			existingProfile.FirstName = profile.FirstName;
			existingProfile.LastName = profile.LastName;
			existingProfile.DateOfBirth = profile.DateOfBirth;
			_context.SaveChanges();
		}
		else
		{
			throw new KeyNotFoundException($"Profile with id {profile.ProfileId} not found.");
		}
	}
}