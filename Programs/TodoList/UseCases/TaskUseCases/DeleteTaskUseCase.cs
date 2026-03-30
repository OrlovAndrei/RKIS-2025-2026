using TodoList.Entity;
using TodoList.Interfaces;
using TodoList.Interfaces.Repositories;

namespace TodoList.UseCases.TaskUseCases;

public class DeleteTaskUseCase : ICommandWithUndo<uint>
{
    private readonly ITaskItemRepositories _taskItemRepositories;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentProfile _currentProfile;
    private TodoItem? _taskDeletion;
    public uint Value { get; private set; } = default;

    public DeleteTaskUseCase(
        ICurrentProfile currentProfile,
        ITaskItemRepositories taskItemRepositories,
        IUnitOfWork unitOfWork
        )
    {
        _currentProfile = currentProfile;
        _taskItemRepositories = taskItemRepositories;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute()
    {
        if (Value == default)
        {
            throw new ArgumentException();
        }
        await Execute(Value);
    }

    public async Task Unexecuted()
    {
        if (_taskDeletion is null)
        {
            throw new ArgumentException();
        }
        await _taskItemRepositories.AddAsync(_taskDeletion);
        _taskDeletion = null;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task Execute(uint value)
    {
        Value = value;
        _taskDeletion = await _taskItemRepositories.GetByIdAsync(Value)
            ?? throw new ArgumentException();
        if (_taskDeletion.ProfileId != _currentProfile.Id)
        {
            throw new ArgumentException();
        }
        await _taskItemRepositories.DeleteAsync(_taskDeletion.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}