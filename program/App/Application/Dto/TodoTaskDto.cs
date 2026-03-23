using System.Linq.Expressions;
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
	/// <summary>
	/// DTO для поиска задач по различным критериям.
	/// </summary>
	public record TodoTaskSearchDto(
		IUserContext UserContext,
		Guid? TaskId = null,
		int? StateId = null,
		int? PriorityLevelFrom = null,
		int? PriorityLevelTo = null,
		string? Name = null,
		string? Description = null,
		DateTime? CreatedAtFrom = null,
		DateTime? CreatedAtTo = null,
		DateTime? DeadlineFrom = null,
		DateTime? DeadlineTo = null);

	/// <summary>
	/// Преобразует TodoTaskSearchDto в TaskCriteria.
	/// </summary>
	public static Expression<Func<TodoTask, bool>> ToTaskCriteria(this TodoTaskSearchDto searchDto)
	{
		Expression<Func<TodoTask, bool>> criteria = t => t.ProfileId == searchDto.UserContext.UserId &&
			(!searchDto.TaskId.HasValue || t.TaskId == searchDto.TaskId.Value) &&
			(!searchDto.StateId.HasValue || t.State.StateId == searchDto.StateId.Value) &&
			(!searchDto.PriorityLevelFrom.HasValue || t.Priority.Level >= searchDto.PriorityLevelFrom.Value) &&
			(!searchDto.PriorityLevelTo.HasValue || t.Priority.Level <= searchDto.PriorityLevelTo.Value) &&
			(string.IsNullOrWhiteSpace(searchDto.Name) || t.Name.Contains(searchDto.Name)) &&
			(string.IsNullOrWhiteSpace(searchDto.Description) || (!string.IsNullOrWhiteSpace(t.Description) && t.Description.Contains(searchDto.Description))) &&
			(!searchDto.CreatedAtFrom.HasValue || t.CreatedAt >= searchDto.CreatedAtFrom.Value) &&
			(!searchDto.CreatedAtTo.HasValue || t.CreatedAt <= searchDto.CreatedAtTo.Value) &&
			(!searchDto.DeadlineFrom.HasValue || (t.Deadline >= searchDto.DeadlineFrom.Value)) &&
			(!searchDto.DeadlineTo.HasValue || (t.Deadline <= searchDto.DeadlineTo.Value));
		return criteria;
	}
}