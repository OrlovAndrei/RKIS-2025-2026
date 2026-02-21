using Application.Interfaces;
using Domain;
using Domain.Interfaces;

namespace Application.UseCase.ProfileUseCases;

public class ChangeProfileUseCase : IUndoRedo
{
    private readonly IProfileRepository _repo;
    private readonly IPasswordHashed _hashed;
    private readonly Profile _newProfile;
    private readonly Guid? _oldProfileId;
    private readonly ICurrentUserService _currentUser;
    private readonly bool _verifyPassword;
    public ChangeProfileUseCase(
        IProfileRepository repository,
        IPasswordHashed hashed,
        ICurrentUserService currentUser,
        Guid newProfile,
        string password
        )
    {
        _repo = repository;
        _hashed = hashed;
        _currentUser = currentUser;
        _oldProfileId = _currentUser.UserId;
        _newProfile = _repo.GetByIdAsync(newProfile).Result
            ?? throw new Exception("This profile does not exist.");
        _verifyPassword = _hashed.VerifyAsync(
            password: password,
            _newProfile.CreatedAt,
            hashedPassword: _newProfile.PasswordHash).Result;
    }
    public async Task<int> Execute()
    {
        if (_verifyPassword)
        {
            _currentUser.Set(_newProfile.ProfileId);
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public async Task<int> Undo()
    {
        _currentUser.Set(_oldProfileId);
        return 1;
    }
}