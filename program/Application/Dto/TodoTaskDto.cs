using Application.Interfaces;
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
	public static TodoTaskDetailsDto ToDetailsDto(this TodoTask todoTask) => new(
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
	public static TodoTaskShortDto ToShortDto(this TodoTask todoTask) => new(
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
	public static TodoTaskUpdateDto ToUpdateDto(this TodoTask todoTask) => new(
			TaskId: todoTask.TaskId,
			StateId: todoTask.StateId,
			Name: todoTask.Name,
			Description: todoTask.Description,
			Deadline: todoTask.Deadline
		);
	public static TodoTask FromUpdateDto(
		this TodoTaskUpdateDto todoTaskUpdateDto) => TodoTask.CreateUpdateObj(
			taskId: todoTaskUpdateDto.TaskId,
			stateId: todoTaskUpdateDto.StateId,
			name: todoTaskUpdateDto.Name,
			description: todoTaskUpdateDto.Description,
			deadline: todoTaskUpdateDto.Deadline
		);
	public record TodoTaskCreateDto(
		Guid StateId,
		ICurrentUserService CurrentUser,
		string Name,
		string? Description,
		DateTime? Deadline);
	public static TodoTaskCreateDto ToCreateDto(this TodoTask todoTask, ICurrentUserService currentUser) => new(
			StateId: todoTask.StateId,
			CurrentUser: currentUser,
			Name: todoTask.Name,
			Description: todoTask.Description,
			Deadline: todoTask.Deadline
		);
	public static TodoTask FromCreateDto(
		this TodoTaskCreateDto todoTaskCreateDto,
		ICurrentUserService currentUser)
	{
		Guid profileId = currentUser.UserId ?? throw new Exception(message: "You need to log in first.");
		return new(
			stateId: todoTaskCreateDto.StateId,
			profileId: profileId,
			name: todoTaskCreateDto.Name,
			description: todoTaskCreateDto.Description,
			deadline: todoTaskCreateDto.Deadline);
	}
}