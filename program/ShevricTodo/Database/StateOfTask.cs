namespace ShevricTodo.Database;

internal class StateOfTask
{
	public int StateId { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public virtual ICollection<TaskTodo>? Tasks { get; set; }
}
