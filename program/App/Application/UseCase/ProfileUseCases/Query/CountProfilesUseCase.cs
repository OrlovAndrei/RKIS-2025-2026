using Application.Dto;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;

namespace Application.UseCase.ProfileUseCases.Query;

/// <summary>
/// Use case для подсчета профилей по критериям.
/// </summary>
public class CountProfilesUseCase : IQueryUseCase<int>
{
	private readonly IProfileRepository _repository;
	private readonly ProfileDto.ProfileSearchDto _searchDto;

	public CountProfilesUseCase(
		IProfileRepository repository,
		ProfileDto.ProfileSearchDto searchDto)
	{
		_repository = repository;
		_searchDto = searchDto;
	}

	/// <summary>
	/// Подсчитывает количество профилей, соответствующих критериям из DTO.
	/// </summary>
	/// <returns>Количество найденных профилей.</returns>
	public async Task<int> Execute()
	{
		var criteria = _searchDto.ToProfileCriteria();
		return await _repository.CountAsync(criteria);
	}
}
