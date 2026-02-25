using Application.Interfaces;
using Application.Specifications;
using Application.Specifications.Criteria;
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
		DateTime? DeadlineTo = null,
		SearchTypes? SearchType = SearchTypes.Contains);

	/// <summary>
	/// Преобразует TodoTaskSearchDto в TaskCriteria.
	/// </summary>
	public static TaskCriteria ToTaskCriteria(this TodoTaskSearchDto searchDto)
	{
		var criteria = new TaskCriteria();

		Guid userId = (Guid)(searchDto.UserContext.UserId is not null ? searchDto.UserContext.UserId : throw new Exception());
		criteria += TaskCriteria.ByProfileId(userId);

		// Применяем базовые критерии
		if (searchDto.TaskId.HasValue)
		{
			criteria += TaskCriteria.ByTaskId(searchDto.TaskId.Value);
		}

		if (searchDto.StateId.HasValue)
		{
			criteria += TaskCriteria.ByStateId(searchDto.StateId.Value);
		}

		if (searchDto.PriorityLevelFrom.HasValue || searchDto.PriorityLevelTo.HasValue)
		{
			criteria += TaskCriteria.ByPriorityLevel(searchDto.PriorityLevelFrom, searchDto.PriorityLevelTo);
		}

		if (!string.IsNullOrWhiteSpace(searchDto.Name))
		{
			criteria += TaskCriteria.ByName(searchDto.Name);
		}

		if (!string.IsNullOrWhiteSpace(searchDto.Description))
		{
			criteria += TaskCriteria.ByDescription(searchDto.Description);
		}

		if (searchDto.CreatedAtFrom.HasValue || searchDto.CreatedAtTo.HasValue)
		{
			criteria += TaskCriteria.ByCreatedAt(searchDto.CreatedAtFrom, searchDto.CreatedAtTo);
		}

		if (searchDto.DeadlineFrom.HasValue || searchDto.DeadlineTo.HasValue)
		{
			criteria += TaskCriteria.ByDeadline(searchDto.DeadlineFrom, searchDto.DeadlineTo);
		}

		// Применяем тип поиска для текстовых полей
		criteria = searchDto.SearchType switch
		{
			SearchTypes.Contains => criteria.Contains(),
			SearchTypes.StartsWith => criteria.StartsWith(),
			SearchTypes.Equals => criteria.Equals(),
			SearchTypes.EndsWith => criteria.EndWith(),
			_ => criteria.Equals()
		};

		return criteria;
	}
}