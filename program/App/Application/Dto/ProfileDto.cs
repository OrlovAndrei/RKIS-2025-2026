using System.Linq.Expressions;
using Application.Interfaces;
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
		DateTime? DateOfBirthTo = null);

	/// <summary>
	/// Преобразует ProfileSearchDto в ProfileCriteria.
	/// </summary>
	public static Expression<Func<Profile, bool>> ToProfileCriteria(this ProfileSearchDto searchDto)
	{
		// Применяем базовые критерии
		Expression<Func<Profile, bool>> criteria = p =>
			(!searchDto.ProfileId.HasValue || p.ProfileId == searchDto.ProfileId.Value) &&
			(string.IsNullOrWhiteSpace(searchDto.FirstName) || p.FirstName.Contains(searchDto.FirstName)) &&
			(string.IsNullOrWhiteSpace(searchDto.LastName) || p.LastName.Contains(searchDto.LastName)) &&
			(!searchDto.CreatedAtFrom.HasValue || p.CreatedAt >= searchDto.CreatedAtFrom) &&
			(!searchDto.CreatedAtTo.HasValue || p.CreatedAt <= searchDto.CreatedAtTo) &&
			(!searchDto.DateOfBirthFrom.HasValue || p.DateOfBirth >= searchDto.DateOfBirthFrom) &&
			(!searchDto.DateOfBirthTo.HasValue || p.DateOfBirth <= searchDto.DateOfBirthTo);
		// Применяем тип поиска для текстовых полей
		return criteria;
	}
}