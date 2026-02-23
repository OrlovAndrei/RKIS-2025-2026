using Application.Interfaces;
using Application.Specifications;
using Domain.Entities.ProfileEntity;
namespace Application.Dto;

public static class ProfileDto
{
	public record ProfileDetailsDto(
		Guid ProfileId,
		string FirstName,
		string LastName,
		DateTime DateOfBirth,
		DateTime CreatedAt);
	public static ProfileDetailsDto ToDetailsDto(this Profile profile) => new(
		ProfileId: profile.ProfileId,
		FirstName: profile.FirstName,
		LastName: profile.LastName,
		DateOfBirth: profile.DateOfBirth,
		CreatedAt: profile.CreatedAt
	);
	public record ProfileShortDto(
		Guid ProfileId,
		string FirstName,
		string LastName);
	public static ProfileShortDto ToShortDto(this Profile profile) => new(
		ProfileId: profile.ProfileId,
		FirstName: profile.FirstName,
		LastName: profile.LastName
	);
	public record ProfileUpdateDto(
		Guid ProfileId,
		string FirstName,
		string LastName,
		DateTime DateOfBirth);
	public static ProfileUpdateDto ToUpdateDto(this Profile profile) => new(
		ProfileId: profile.ProfileId,
		FirstName: profile.FirstName,
		LastName: profile.LastName,
		DateOfBirth: profile.DateOfBirth
	);
	public static Profile FromUpdateDto(this ProfileUpdateDto profileUpdateDto) =>
		Profile.CreateUpdateObj(
			profileId: profileUpdateDto.ProfileId,
			firstName: profileUpdateDto.FirstName,
			lastName: profileUpdateDto.LastName,
			dateOfBirth: profileUpdateDto.DateOfBirth);
	public record ProfileCreateDto(
		string FirstName,
		string LastName,
		DateTime DateOfBirth,
		string Password);
	public static ProfileCreateDto ToCreateDto(this Profile profile) => new(
		FirstName: profile.FirstName,
		LastName: profile.LastName,
		DateOfBirth: profile.DateOfBirth,
		Password: profile.PasswordHash
	);
	public static Profile FromCreateDto(
		this ProfileCreateDto profileCreateDto,
		IPasswordHasher passwordHashed) => new(
		firstName: profileCreateDto.FirstName,
		lastName: profileCreateDto.LastName,
		dateOfBirth: profileCreateDto.DateOfBirth,
		passwordHash: passwordHashed.HashedAsync(
			password: profileCreateDto.Password).Result
		);

	/// <summary>
	/// DTO для поиска профилей по различным критериям.
	/// </summary>
	public record ProfileSearchDto(
		Guid? ProfileId = null,
		string? FirstName = null,
		string? LastName = null,
		DateTime? CreatedAtFrom = null,
		DateTime? CreatedAtTo = null,
		DateTime? DateOfBirthFrom = null,
		DateTime? DateOfBirthTo = null,
		SearchType? SearchType = SearchType.Contains);

	/// <summary>
	/// Преобразует ProfileSearchDto в ProfileCriteria.
	/// </summary>
	public static ProfileCriteria ToProfileCriteria(this ProfileSearchDto searchDto)
	{
		var criteria = new ProfileCriteria();

		// Применяем базовые критерии
		if (searchDto.ProfileId.HasValue)
		{
			criteria += ProfileCriteria.ByProfileId(searchDto.ProfileId.Value);
		}

		if (!string.IsNullOrWhiteSpace(searchDto.FirstName))
		{
			criteria += ProfileCriteria.ByFirstName(searchDto.FirstName);
		}

		if (!string.IsNullOrWhiteSpace(searchDto.LastName))
		{
			criteria += ProfileCriteria.ByLastName(searchDto.LastName);
		}

		if (searchDto.CreatedAtFrom.HasValue || searchDto.CreatedAtTo.HasValue)
		{
			criteria += ProfileCriteria.ByCreatedAt(searchDto.CreatedAtFrom, searchDto.CreatedAtTo);
		}

		if (searchDto.DateOfBirthFrom.HasValue || searchDto.DateOfBirthTo.HasValue)
		{
			criteria += ProfileCriteria.ByDateOfBirth(searchDto.DateOfBirthFrom, searchDto.DateOfBirthTo);
		}

		// Применяем тип поиска для текстовых полей
		return searchDto.SearchType switch
		{
			SearchType.Contains => criteria.Contains(),
			SearchType.StartsWith => criteria.StartsWith(),
			SearchType.Equals => criteria.Equals(),
			SearchType.EndsWith => criteria.EndWith(),
			_ => criteria.Equals()
		};
	}
}