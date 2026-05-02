using Application.Dto;
using Application.Interfaces;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;
using Domain.Entities.ProfileEntity;

namespace Application.UseCase.ProfileUseCases;

public class AddNewProfileUseCase : ICommandWithUndo
{
	private readonly IProfileRepository _repo;
	private readonly IPasswordHasher _hashed;
	private readonly ProfileDto.ProfileCreateDto _profileCreate;
	private readonly Profile _newProfile;
	private readonly IUnitOfWork _unitOfWork;
	public AddNewProfileUseCase(
		IProfileRepository repository,
		IPasswordHasher hashed,
		ProfileDto.ProfileCreateDto profileCreate,
		IUnitOfWork unitOfWork
		)
	{
		_unitOfWork = unitOfWork;
		_repo = repository;
		_hashed = hashed;
		_profileCreate = profileCreate;
		_newProfile = _profileCreate.FromCreateDto(passwordHashed: _hashed);
	}
	public async Task Execute()
	{
		await _repo.AddAsync(_newProfile);
		await _unitOfWork.SaveChangesAsync();
	}
	public async Task Undo()
	{

		await _repo.DeleteAsync(_newProfile.ProfileId);
		await _unitOfWork.SaveChangesAsync();

	}
}