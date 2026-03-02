namespace Infrastructure.Database.Entity;

public class TodoTaskEntity
{
	public string TaskId { get; set; } = null!;
	public int StateId { get; set; }
	public int PriorityLevel { get; set; }
	public string ProfileId { get; set; } = null!;
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public DateTime CreateAt { get; set; }
	public DateTime? Deadline { get; set; }
	public virtual ProfileEntity? TaskCreator { get; set; }
}
