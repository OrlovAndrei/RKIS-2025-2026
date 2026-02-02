namespace ShevricTodo.Database;

internal class Task
{
	public int TaskId { get; set; }
	public string? Type { get; set; }
	public string? State { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public DateTime DateOfCreate { get; set; }
	public DateTime DateOfStart { get; set; }
	public DateTime DateOfEnd { get; set; }
	public DateTime Deadline { get; set; }
	public virtual Profile? TaskCreator { get; set; }
}
