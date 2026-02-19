using Domain;
using Infrastructure.Database;

namespace Infrastructure.Mapper;

public static class ProfileMapper
{
	public static ProfileEntity ToEntity(this Profile profile) => new()
	{
		ProfileId = profile.ProfileId.ToString(),
		FirstName = profile.FirstName,
		LastName = profile.LastName,
		DateOfBirth = profile.DateOfBirth,
		CreatedAt = profile.CreatedAt,
		PasswordHash = profile.PasswordHash
	};
	public static Profile ToDomain(this ProfileEntity profileEntity)
	{
		return Profile.Restore(
		profileId: Guid.Parse(profileEntity.ProfileId),
		profileEntity.FirstName,
		profileEntity.LastName,
		profileEntity.DateOfBirth,
		profileEntity.CreatedAt,
		profileEntity.PasswordHash
	);
	}
}