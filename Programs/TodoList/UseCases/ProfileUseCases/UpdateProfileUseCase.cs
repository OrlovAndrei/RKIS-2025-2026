using TodoList.Entity;
using TodoList.Interfaces;
using TodoList.Interfaces.Repositories;

namespace TodoList.UseCases.ProfileUseCases;

public class UpdateProfileUseCase : ICommandWithUndo<Profile>
{
    private IProfileRepositories _profileRepositories;
    private ICurrentProfile _currentProfile;
    private Profile? _profileOld;
    private Profile? _profileNew;
    private IUnitOfWork _unitOfWork;
    public Profile? Value { get; private set; }
    public UpdateProfileUseCase(
        IProfileRepositories profileRepositories,
        ICurrentProfile currentProfile,
        IUnitOfWork unitOfWork
    )
    {
        _currentProfile = currentProfile;
        _profileRepositories = profileRepositories;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute()
    {
        if (_profileNew is null)
        {
            throw new ArgumentException();
        }
        await Execute(_profileNew);
    }

    public async Task Execute(Profile value)
    {
        _profileNew = value;
        if (_profileNew.Id != _currentProfile.Id)
        {
            throw new ArgumentException();
        }
        _profileOld = await _profileRepositories.GetByIdAsync(_profileNew.Id)
            ?? throw new ArgumentException();
        await _profileRepositories.UpdateAsync(_profileNew);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task Unexecuted()
    {
        if (_profileOld is null)
        {
            throw new ArgumentException();
        }
        await _profileRepositories.UpdateAsync(_profileOld);
        await _unitOfWork.SaveChangesAsync();
        _profileOld = null;
    }
}