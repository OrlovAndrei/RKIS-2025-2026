using Application.Interfaces;
using Domain.Entities.TaskEntity;
using Domain.Specifications;

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
		Guid? TaskId = null,
		int? StateId = null,
		Guid? ProfileId = null,
		int? PriorityLevelFrom = null,
		int? PriorityLevelTo = null,
		string? Name = null,
		string? Description = null,
		DateTime? CreatedAtFrom = null,
		DateTime? CreatedAtTo = null,
		DateTime? DeadlineFrom = null,
		DateTime? DeadlineTo = null,
		string? SearchType = null);

	/// <summary>
	/// Преобразует TodoTaskSearchDto в TaskCriteria.
	/// SearchType может быть: "Contains", "StartsWith", "Equals", "EndsWith".
	/// По умолчанию используется "Equals".
	/// </summary>
	public static TaskCriteria ToTaskCriteria(this TodoTaskSearchDto searchDto)
	{
		var criteria = new TaskCriteria();

		// Применяем базовые критерии
		if (searchDto.TaskId.HasValue)
		{
			criteria = criteria.Add(TaskCriteria.ByTaskId(searchDto.TaskId.Value));
		}

		if (searchDto.StateId.HasValue)
		{
			criteria = criteria.Add(TaskCriteria.ByStateId(searchDto.StateId.Value));
		}

		if (searchDto.PriorityLevelFrom.HasValue || searchDto.PriorityLevelTo.HasValue)
		{
			criteria = criteria.Add(TaskCriteria.ByPriorityLevel(searchDto.PriorityLevelFrom, searchDto.PriorityLevelTo));
		}

		if (searchDto.ProfileId.HasValue)
		{
			criteria = criteria.Add(TaskCriteria.ByProfileId(searchDto.ProfileId.Value));
		}

		if (!string.IsNullOrWhiteSpace(searchDto.Name))
		{
			criteria = criteria.Add(TaskCriteria.ByName(searchDto.Name));
		}

		if (!string.IsNullOrWhiteSpace(searchDto.Description))
		{
			criteria = criteria.Add(TaskCriteria.ByDescription(searchDto.Description));
		}

		if (searchDto.CreatedAtFrom.HasValue || searchDto.CreatedAtTo.HasValue)
		{
			criteria = criteria.Add(TaskCriteria.ByCreatedAt(searchDto.CreatedAtFrom, searchDto.CreatedAtTo));
		}

		if (searchDto.DeadlineFrom.HasValue || searchDto.DeadlineTo.HasValue)
		{
			criteria = criteria.Add(TaskCriteria.ByDeadline(searchDto.DeadlineFrom, searchDto.DeadlineTo));
		}

		// Применяем тип поиска для текстовых полей
		var searchType = searchDto.SearchType?.ToLower() ?? "equals";
		criteria = searchType switch
		{
			"contains" => criteria.Contains(),
			"startswith" => criteria.StartsWith(),
			"equals" => criteria.Equals(),
			"endswith" => criteria.EndWith(),
			_ => criteria.Equals()
		};

		return criteria;
	}
}