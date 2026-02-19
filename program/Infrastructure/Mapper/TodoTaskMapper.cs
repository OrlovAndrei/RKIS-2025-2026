using Domain;
using Infrastructure.Database;

namespace Infrastructure.Mapper;

public static class TodoTaskMapper
{
    public static TodoTaskEntity ToEntity(this TodoTask todoTask) => new()
    {
        TaskId = todoTask.TaskId.ToString(),
        StateId = todoTask.StateId.ToString(),
        ProfileId = todoTask.ProfileId.ToString(),
        Name = todoTask.Name,
        Description = todoTask.Description,
        CreateAt = todoTask.CreatedAt,
        Deadline = todoTask.Deadline,
    };
    public static TodoTask ToDomain(this TodoTaskEntity todoTaskEntity)
    {
        return TodoTask.Restore(
            taskId: Guid.Parse(todoTaskEntity.TaskId),
            stateId: Guid.Parse(todoTaskEntity.StateId),
            profileId: Guid.Parse(todoTaskEntity.ProfileId),
            name: todoTaskEntity.Name,
            description: todoTaskEntity.Description,
            createdAt: todoTaskEntity.CreateAt,
            deadline: todoTaskEntity.Deadline
        );
    }
}