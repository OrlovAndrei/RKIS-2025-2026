using Domain;
using Infrastructure.Database;

namespace Infrastructure.Mapper;

public static class TaskStateMapper
{
    public static TaskStateEntity ToEntity(this TaskState taskState) => new()
    {
        StateId = taskState.StateId.ToString(),
        Name = taskState.Name,
        Description = taskState.Description,
        IsCompleted = taskState.IsCompleted
    };
    public static TaskState ToDomain(this TaskStateEntity taskStateEntity)
    {
        return TaskState.Restore(
            stateId: Guid.Parse(taskStateEntity.StateId),
            name: taskStateEntity.Name,
            description: taskStateEntity.Description,
            isCompleted: taskStateEntity.IsCompleted
        );
    }
}