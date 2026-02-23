using Application.Interfaces;
using Domain.Entities.TaskEntity;

namespace Application.Dto;

public static class TodoTaskDto
{
	public record TodoTaskDetailsDto(
	Guid TaskId,
	string NameState,
	string DescriptionState,
	string NamePriority,
	string Name,
	string? Description,
	DateTime CreatedAt,
	DateTime? Deadline);
	public static TodoTaskDetailsDto ToDetailsDto(this TodoTask todoTask) => new(
			TaskId: todoTask.TaskId,
			NameState: todoTask.State.Name,
			DescriptionState: todoTask.State.Description,
			NamePriority: todoTask.Priority.Name,
			Name: todoTask.Name,
			Description: todoTask.Description,
			CreatedAt: todoTask.CreatedAt,
			Deadline: todoTask.Deadline
		);
	public record TodoTaskShortDto(
		Guid TaskId,
		string Name,
		DateTime? Deadline);
	public static TodoTaskShortDto ToShortDto(this TodoTask todoTask) => new(
			TaskId: todoTask.TaskId,
			Name: todoTask.Name,
			Deadline: todoTask.Deadline
		);
	public record TodoTaskUpdateDto(
		Guid TaskId,
		TaskState State,
		TaskPriority Priority,
		string Name,
		string? Description,
		DateTime? Deadline);
	public static TodoTaskUpdateDto ToUpdateDto(this TodoTask todoTask) => new(
			TaskId: todoTask.TaskId,
			State: todoTask.State,
			Priority: todoTask.Priority,
			Name: todoTask.Name,
			Description: todoTask.Description,
			Deadline: todoTask.Deadline
		);
	public static TodoTask FromUpdateDto(
		this TodoTaskUpdateDto todoTaskUpdateDto) => TodoTask.CreateUpdateObj(
			taskId: todoTaskUpdateDto.TaskId,
			state: todoTaskUpdateDto.State,
			priority: todoTaskUpdateDto.Priority,
			name: todoTaskUpdateDto.Name,
			description: todoTaskUpdateDto.Description,
			deadline: todoTaskUpdateDto.Deadline
		);
	public record TodoTaskCreateDto(
		TaskState? State,
		TaskPriority? Priority,
		IUserContext UserContext,
		string Name,
		string? Description,
		DateTime? Deadline);
	public static TodoTaskCreateDto ToCreateDto(this TodoTask todoTask, IUserContext userContext) => new(
			State: todoTask.State,
			Priority: todoTask.Priority,
			UserContext: userContext,
			Name: todoTask.Name,
			Description: todoTask.Description,
			Deadline: todoTask.Deadline
		);
	public static TodoTask FromCreateDto(
		this TodoTaskCreateDto todoTaskCreateDto,
		IUserContext userContext)
	{
		Guid profileId = userContext.UserId ?? throw new Exception(message: "You need to log in first.");
		return new(
			profileId: profileId,
			name: todoTaskCreateDto.Name,
			description: todoTaskCreateDto.Description,
			deadline: todoTaskCreateDto.Deadline,
			state: todoTaskCreateDto.State,
			priority: todoTaskCreateDto.Priority);
	}
}