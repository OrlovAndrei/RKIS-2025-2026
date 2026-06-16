using TodoList.Entity;
using TodoList.Interfaces;
using TodoList.Interfaces.Repositories;

namespace TodoList.UseCases.TaskUseCases;

public class UpdateTaskUseCase : ICommandWithUndo<TodoItem>
{
    private readonly ITaskItemRepositories _taskItemRepositories;
    private readonly ICurrentProfile _currentProfile;
    private TodoItem? _todoItemOld;
    private readonly IUnitOfWork _unitOfWork;
    public TodoItem? Value { get; private set; }
    public UpdateTaskUseCase(
        ITaskItemRepositories taskItemRepositories,
        ICurrentProfile currentProfile,
        IUnitOfWork unitOfWork
    )
    {
        _taskItemRepositories = taskItemRepositories;
        _currentProfile = currentProfile;
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
        if (_todoItemOld is null)
        {
            throw new ArgumentException();
        }
        await _taskItemRepositories.UpdateAsync(_todoItemOld);
        await _unitOfWork.SaveChangesAsync();
        _todoItemOld = null;
    }

    public async Task Execute(TodoItem value)
    {
        Value = value;
        _todoItemOld = await _taskItemRepositories.GetByIdAsync(Value.Id)
            ?? throw new ArgumentException();
        if (Value.ProfileId != _todoItemOld.ProfileId)
        {
            throw new ArgumentException();
        }
        if (Value.ProfileId != _currentProfile.Id)
        {
            throw new ArgumentException();
        }
        await _taskItemRepositories.UpdateAsync(Value);
        await _unitOfWork.SaveChangesAsync();
    }
}