using Application.Dto;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;

namespace Application.UseCase.ProfileUseCases;

/// <summary>
/// Use case для проверки существования профиля по критериям.
/// </summary>
public class CheckProfileExistsUseCase : IQueryUseCase<bool>
{
	private readonly IProfileRepository _repository;
	private readonly ProfileDto.ProfileSearchDto _searchDto;

	public CheckProfileExistsUseCase(
		IProfileRepository repository,
		ProfileDto.ProfileSearchDto searchDto)
	{
		_repository = repository;
		_searchDto = searchDto;
	}

	/// <summary>
	/// Проверяет существование профиля, соответствующего критериям из DTO.
	/// </summary>
	/// <returns>true если профиль существует, иначе false.</returns>
	public async Task<bool> Execute()
	{
		var criteria = _searchDto.ToProfileCriteria();
		return await _repository.ExistsAsync(criteria);
	}
}
