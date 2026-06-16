using TodoList.Entity;
using TodoList.Interfaces;
using TodoList.Interfaces.Repositories;

namespace TodoList.UseCases.ProfileUseCases;

public class AddProfileUseCase : ICommandWithUndo<Profile>
{
    private readonly IProfileRepositories _profileRepositories;
    private readonly IUnitOfWork _unitOfWork;
    public Profile? Value { get; private set; }
    public AddProfileUseCase(
        IProfileRepositories profileRepositories,
        IUnitOfWork unitOfWork
    )
    {
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

    public async Task Execute(Profile value)
    {
        Value = value;
        await _profileRepositories.AddAsync(Value);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task Unexecuted()
    {
        if (Value is null)
        {
            throw new ArgumentException();
        }
        await _profileRepositories.DeleteAsync(Value.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}