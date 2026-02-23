using Application.Interfaces;
using Domain.Entities.ProfileEntity;
using Domain.Interfaces;

namespace Application.UseCase.ProfileUseCases;

public class ChangeProfileUseCase : IUndoRedo
{
	private readonly IProfileRepository _repo;
	private readonly IPasswordHasher _hashed;
	private readonly Profile _newProfile;
	private readonly Guid? _oldProfileId;
	private readonly IUserContext _userContext;
	private readonly bool _verifyPassword;
	public ChangeProfileUseCase(
		IProfileRepository repository,
		IPasswordHasher hashed,
		IUserContext userContext,
		Guid newProfile,
		string password
		)
	{
		_repo = repository;
		_hashed = hashed;
		_userContext = userContext;
		_oldProfileId = _userContext.UserId;
		_newProfile = _repo.GetByIdAsync(newProfile).Result
			?? throw new Exception("This profile does not exist.");
		_verifyPassword = _hashed.VerifyAsync(
			password: password,
			hashedPassword: _newProfile.PasswordHash).Result;
	}
	public async Task<int> Execute()
	{
		if (_verifyPassword)
		{
			_userContext.Set(_newProfile.ProfileId);
			return 1;
		}
		else
		{
			return 0;
		}
	}

	public async Task<int> Undo()
	{
		_userContext.Set(_oldProfileId);
		return 1;
	}
}