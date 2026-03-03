using Application.Interfaces;
using Application.Interfaces.Command;
using Application.Interfaces.Repository;
using Domain.Entities.ProfileEntity;

namespace Application.UseCase.ProfileUseCases;

public class DeletionProfileUseCase : ICommandWithUndo
{
	private readonly IProfileRepository _repo;
	private readonly IPasswordHasher _hashed;
	private readonly Guid _idProfile;
	private readonly Profile _deletionProfile;
	private readonly bool _verifyPassword;
	public DeletionProfileUseCase(
		IProfileRepository repository,
		IPasswordHasher hashed,
		Guid idProfile,
		string password
		)
	{
		_repo = repository;
		_hashed = hashed;
		_idProfile = idProfile;
		_deletionProfile = _repo.GetByIdAsync(_idProfile).Result
			?? throw new Exception("This profile does not exist.");
		_verifyPassword = _hashed.VerifyAsync(
			password: password,
			hashedPassword: _deletionProfile.PasswordHash).Result;
	}

	public async Task<int> Execute()
	{
		return _verifyPassword
			? await _repo.DeleteAsync(_idProfile)
			: throw new ArgumentException(message: "Incorrect password.");
	}
	public Task<int> Undo()
	{
		return _repo.AddAsync(_deletionProfile);
	}
}