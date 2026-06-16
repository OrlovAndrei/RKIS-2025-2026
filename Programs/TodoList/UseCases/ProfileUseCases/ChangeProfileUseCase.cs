using TodoList.Dto;
using TodoList.Entity;
using TodoList.Interfaces;
using TodoList.Interfaces.Repositories;

namespace TodoList.UseCases.ProfileUseCases;

public class ChangeProfileUseCase : ICommandWithUndo<LoginDto.Login>
{
    private readonly IProfileRepositories _profileRepositories;
    private Profile? ProfileSearch { get; set; }
    private readonly IHasher _hasher;
    private readonly ICurrentProfile _currentProfile;
    private Guid? OldProfileId { get; set; }
	public LoginDto.Login? Value { get; private set; }
	public ChangeProfileUseCase(
        IProfileRepositories profileRepositories,
        ICurrentProfile currentProfile,
        IHasher hasher
    )
    {
        _profileRepositories = profileRepositories;
        _currentProfile = currentProfile;
        _hasher = hasher;
    }
    public async Task Execute()
    {
        if (Value is null)
        {
            throw new ArgumentException();
        }
        await Execute(Value);
    }
    public async Task Unexecuted()
    {
        if (ProfileSearch is null)
        {
            throw new ArgumentException();
        }
        await _currentProfile.Set(OldProfileId ?? throw new ArgumentException());
        ProfileSearch = null;
    }

	public async Task Execute(LoginDto.Login value)
	{
        Value = value;
		ProfileSearch = await _profileRepositories.GetByIdAsync(Value.ProfileId) ?? throw new ArgumentException();
        if (!_hasher.Verify(Value.Password, ProfileSearch.PasswordHash))
        {
            throw new ArgumentException();
        }
        OldProfileId ??= _currentProfile.Id;
        await _currentProfile.Set(Value.ProfileId);
	}
}