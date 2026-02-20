using Domain;

namespace Application.Dto;

public static class TodoTaskDto
{
	public record TodoTaskDetailsDto(
	Guid TaskId,
	Guid StateId,
	Guid ProfileId,
	string Name,
	string? Description,
	DateTime CreatedAt,
	DateTime? Deadline);
	public static TodoTaskDetailsDto ToDetailsDto(TodoTask todoTask) => new(
			TaskId: todoTask.TaskId,
			StateId: todoTask.StateId,
			ProfileId: todoTask.ProfileId,
			Name: todoTask.Name,
			Description: todoTask.Description,
			CreatedAt: todoTask.CreatedAt,
			Deadline: todoTask.Deadline
		);
	public record TodoTaskShortDto(
		Guid TaskId,
		string Name,
		DateTime? Deadline);
	public static TodoTaskShortDto ToShortDto(TodoTask todoTask) => new(
			TaskId: todoTask.TaskId,
			Name: todoTask.Name,
			Deadline: todoTask.Deadline
		);
	public record TodoTaskUpdateDto(
		Guid TaskId,
		Guid StateId,
		string Name,
		string? Description,
		DateTime? Deadline);
	public static TodoTaskUpdateDto ToUpdateDto(TodoTask todoTask) => new(
			TaskId: todoTask.TaskId,
			StateId: todoTask.StateId,
			Name: todoTask.Name,
			Description: todoTask.Description,
			Deadline: todoTask.Deadline
		);
	public record TodoTaskCreateDto(
		Guid StateId,
		string Name,
		string? Description,
		DateTime? Deadline);
	public static TodoTaskCreateDto ToCreateDto(TodoTask todoTask) => new(
			StateId: todoTask.StateId,
			Name: todoTask.Name,
			Description: todoTask.Description,
			Deadline: todoTask.Deadline
		);
}