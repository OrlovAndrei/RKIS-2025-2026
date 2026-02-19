namespace Infrastructure.Database;

public class TodoTaskEntity
{
	public string TaskId { get; set; } = null!;
	public string StateId { get; set; } = null!;
	public string ProfileId { get; set; } = null!;
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public DateTime CreateAt { get; set; }
	public DateTime? Deadline { get; set; }
	public virtual ProfileEntity? TaskCreator { get; set; }
	public virtual TaskStateEntity? StateOfTask { get; set; }
}
