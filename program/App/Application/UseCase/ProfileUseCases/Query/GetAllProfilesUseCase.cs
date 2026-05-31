using Application.Dto;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;

namespace Application.UseCase.ProfileUseCases.Query;

/// <summary>
/// Use case для получения всех профилей.
/// </summary>
public class GetAllProfilesUseCase : IQueryUseCase<IEnumerable<ProfileDto.ProfileDetailsDto>>
{
	private readonly IProfileRepository _repository;

	public GetAllProfilesUseCase(IProfileRepository repository)
	{
		_repository = repository;
	}

	/// <summary>
	/// Получает все профили из репозитория.
	/// </summary>
	/// <returns>Коллекция всех профилей в виде ProfileDetailsDto.</returns>
	public async Task<IEnumerable<ProfileDto.ProfileDetailsDto>> Execute()
	{
		var profiles = await _repository.GetAllAsync();
		return profiles.Select(p => p.ToDetailsDto());
	}
}
