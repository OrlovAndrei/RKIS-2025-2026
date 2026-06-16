using TodoList.Dto;
using TodoList.Entity;
using TodoList.Interfaces;
using TodoList.Interfaces.Repositories;

namespace TodoList.UseCases.ProfileUseCases;

public class DeleteProfileUseCase : ICommandWithUndo<LoginDto.Login>
{
    private readonly IProfileRepositories _profileRepositories;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentProfile _currentProfile;
    private readonly IHasher _hasher;
    private Profile? DeleteProfile { get; set; }
    public LoginDto.Login? Value { get; private set; }
    public DeleteProfileUseCase(
        ICurrentProfile currentProfile,
        IHasher hasher,
        IProfileRepositories profileRepositories,
        IUnitOfWork unitOfWork
    )
    {
        _currentProfile = currentProfile;
        _hasher = hasher;
        _profileRepositories = profileRepositories;
        _unitOfWork = unitOfWork;
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
        if (DeleteProfile is null)
        {
            throw new ArgumentException();
        }
        await _profileRepositories.AddAsync(DeleteProfile);
        await _unitOfWork.SaveChangesAsync();
        DeleteProfile = null;
    }

    public async Task Execute(LoginDto.Login value)
    {
        Value = value;
        DeleteProfile = await _profileRepositories.GetByIdAsync(Value.ProfileId) ?? throw new ArgumentException();
        if (DeleteProfile.Id != _currentProfile.Id)
        {
            throw new ArgumentException();
        }
        if (!_hasher.Verify(Value.Password, DeleteProfile.PasswordHash))
        {
            throw new ArgumentException();
        }
        await _profileRepositories.DeleteAsync(Value.ProfileId);
        await _unitOfWork.SaveChangesAsync();
    }
}