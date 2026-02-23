using Application.Dto;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;

namespace Application.UseCase.ProfileUseCases;

/// <summary>
/// Use case для поиска профилей по критериям.
/// </summary>
public class FindProfilesUseCase : IQueryUseCase<IEnumerable<ProfileDto.ProfileDetailsDto>>
{
	private readonly IProfileRepository _repository;
	private readonly ProfileDto.ProfileSearchDto _searchDto;

	public FindProfilesUseCase(
		IProfileRepository repository,
		ProfileDto.ProfileSearchDto searchDto)
	{
		_repository = repository;
		_searchDto = searchDto;
	}

	/// <summary>
	/// Выполняет поиск профилей по критериям из DTO.
	/// </summary>
	/// <returns>Коллекция найденных профилей в виде ProfileDetailsDto.</returns>
	public async Task<IEnumerable<ProfileDto.ProfileDetailsDto>> Execute()
	{
		var criteria = _searchDto.ToProfileCriteria();
		var profiles = await _repository.FindAsync(criteria);
		return profiles.Select(p => p.ToDetailsDto());
	}
}
