using Domain;

namespace Application.Dto;

public class TaskStateDto
{
    public record TaskStateDetailsDto(
		Guid StateId,
		string Name,
		string? Description,
		bool IsCompleted);
	public static TaskStateDetailsDto ToDetailsDto(TaskState taskState) => new(
		StateId: taskState.StateId,
		Name: taskState.Name,
		Description: taskState.Description,
		IsCompleted: taskState.IsCompleted);
    public record TaskStateShortDto(
		string Name,
		string? Description);
	public static TaskStateShortDto ToShortDto(TaskState taskState) => new(
		Name: taskState.Name,
		Description: taskState.Description);
    public record TaskStateUpdateDto(
		string Name,
		string? Description,
		bool IsCompleted);
	public static TaskStateUpdateDto ToUpdateDto(TaskState taskState) => new(
		Name: taskState.Name,
		Description: taskState.Description,
		IsCompleted: taskState.IsCompleted);
    public record TaskStateCreateDto(
		string Name,
		string? Description,
		bool IsCompleted);
	public static TaskStateCreateDto ToCreateDto(TaskState taskState) => new(
		Name: taskState.Name,
		Description: taskState.Description,
		IsCompleted: taskState.IsCompleted);
}