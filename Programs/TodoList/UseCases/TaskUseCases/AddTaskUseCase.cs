using TodoList.Entity;
using TodoList.Interfaces;
using TodoList.Interfaces.Repositories;

namespace TodoList.UseCases.TaskUseCases;

public class AddTaskUseCase : ICommandWithUndo<TodoItem>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaskItemRepositories _taskItemRepositories;
    private readonly ICurrentProfile _currentProfile;
    public TodoItem? Value { get; private set; }
    public AddTaskUseCase(
        IUnitOfWork unitOfWork,
        ITaskItemRepositories taskItemRepositories,
        ICurrentProfile currentProfile
    )
    {
        _taskItemRepositories = taskItemRepositories;
        _unitOfWork = unitOfWork;
        _currentProfile = currentProfile;
    }
    public async Task Execute()
    {
        if (Value is null)
        {
            throw new ArgumentException();
        }
        await Execute(Value);
    }

    public async Task Execute(TodoItem value)
    {
        Value = value;
        if (Value.ProfileId != _currentProfile.Id)
        {
            throw new ArgumentException();
        }
        await _taskItemRepositories.AddAsync(Value);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task Unexecuted()
    {
        if (Value is null)
        {
            throw new ArgumentException();
        }
        await _taskItemRepositories.DeleteAsync(Value.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}