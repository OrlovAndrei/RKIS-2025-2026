using Domain.Entities.TaskEntity;
using Infrastructure.Database.Entity;

namespace Infrastructure.EfRepository.Mapper;

public static class TodoTaskMapper
{
	public static TodoTaskEntity ToEntity(this TodoTask todoTask) => new()
	{
		TaskId = todoTask.TaskId.ToString(),
		StateId = todoTask.State.StateId,
		PriorityLevel = todoTask.Priority.Level,
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
			stateId: todoTaskEntity.StateId,
			priorityLevel: todoTaskEntity.PriorityLevel,
			profileId: Guid.Parse(todoTaskEntity.ProfileId),
			name: todoTaskEntity.Name,
			description: todoTaskEntity.Description,
			createdAt: todoTaskEntity.CreateAt,
			deadline: todoTaskEntity.Deadline
		);
	}
}