namespace TodoList.Server.Models;
public class SyncRequest
{
	public Guid UserId { get; set; }
	public List<ProfileDto>? Profiles { get; set; }
	public List<TodoItemDto>? Todos { get; set; }
	public DateTime LastSync { get; set; }
}
