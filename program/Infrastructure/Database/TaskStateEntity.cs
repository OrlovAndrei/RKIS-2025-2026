namespace Infrastructure.Database;

public class TaskStateEntity
{
	public string StateId { get; set; } = null!;
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public bool IsCompleted { get; set; }
	public virtual ICollection<TodoTaskEntity>? Tasks { get; set; }
}
