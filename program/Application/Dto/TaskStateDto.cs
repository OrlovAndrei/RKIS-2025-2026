using Domain;

namespace Application.Dto;

public static class TaskStateDto
{
	public record TaskStateDetailsDto(
		Guid StateId,
		string Name,
		string? Description,
		bool IsCompleted);
	public static TaskStateDetailsDto ToDetailsDto(this TaskState taskState) => new(
		StateId: taskState.StateId,
		Name: taskState.Name,
		Description: taskState.Description,
		IsCompleted: taskState.IsCompleted);
	public record TaskStateShortDto(
		string Name,
		string? Description);
	public static TaskStateShortDto ToShortDto(this TaskState taskState) => new(
		Name: taskState.Name,
		Description: taskState.Description);
	public record TaskStateUpdateDto(
		string Name,
		string? Description,
		bool IsCompleted);
	public static TaskStateUpdateDto ToUpdateDto(this TaskState taskState) => new(
		Name: taskState.Name,
		Description: taskState.Description,
		IsCompleted: taskState.IsCompleted);
	public record TaskStateCreateDto(
		string Name,
		string? Description,
		bool IsCompleted);
	public static TaskStateCreateDto ToCreateDto(this TaskState taskState) => new(
		Name: taskState.Name,
		Description: taskState.Description,
		IsCompleted: taskState.IsCompleted);
	public static TaskState FromCreateDto(this TaskStateCreateDto taskStateCreateDto) => new(
		name: taskStateCreateDto.Name,
		description: taskStateCreateDto.Description,
		isCompleted: taskStateCreateDto.IsCompleted);
}