using Application.Dto;
using Application.Interfaces;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;
using Domain.Entities.ProfileEntity;

namespace Application.UseCase.ProfileUseCases;

public class UpdateProfileUseCase : ICommandWithUndo
{
	private readonly IProfileRepository _repo;
	private readonly IPasswordHasher _hashed;
	private readonly Profile _oldProfile;
	private readonly ProfileDto.ProfileUpdateDto _updateDto;
	private readonly bool _verifyPassword;
	private readonly IUnitOfWork _unitOfWork;
	public UpdateProfileUseCase(
		IUnitOfWork unitOfWork,
		IProfileRepository repository,
		IPasswordHasher hashed,
		ProfileDto.ProfileUpdateDto profileUpdate,
		string password
		)
	{
		_unitOfWork = unitOfWork;
		_repo = repository;
		_hashed = hashed;
		_updateDto = profileUpdate;
		_oldProfile = _repo.GetByIdAsync(_updateDto.ProfileId).Result
			?? throw new Exception("This profile does not exist.");
		_verifyPassword = _hashed.VerifyAsync(
			password: password,
			hashedPassword: _oldProfile.PasswordHash).Result;
	}
	public async Task Execute()
	{
		if (!_verifyPassword)
		{
			throw new ArgumentException(message: "Incorrect password.");
		}
		await _repo.UpdateAsync(_updateDto.FromUpdateDto());
		await _unitOfWork.SaveChangesAsync();
	}

	public async Task Undo()
	{
		await _repo.UpdateAsync(_oldProfile);
		await _unitOfWork.SaveChangesAsync();
	}
}