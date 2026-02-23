using Application.Dto;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.UseCase.ProfileUseCases;

/// <summary>
/// Use case для получения одного профиля по критериям.
/// </summary>
public class GetProfileUseCase : IQueryUseCase<ProfileDto.ProfileDetailsDto?>
{
	private readonly IProfileRepository _repository;
	private readonly ProfileDto.ProfileSearchDto _searchDto;

	public GetProfileUseCase(
		IProfileRepository repository,
		ProfileDto.ProfileSearchDto searchDto)
	{
		_repository = repository;
		_searchDto = searchDto;
	}

	/// <summary>
	/// Выполняет поиск одного профиля по критериям из DTO.
	/// </summary>
	/// <returns>Найденный профиль в виде ProfileDetailsDto или null.</returns>
	public async Task<ProfileDto.ProfileDetailsDto?> Execute()
	{
		var criteria = _searchDto.ToProfileCriteria();
		var profile = await _repository.FindSingleAsync(criteria);
		return profile?.ToDetailsDto();
	}
}
