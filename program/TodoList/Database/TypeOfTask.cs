namespace ShevricTodo.Database;

internal class TypeOfTask
{
	public int TypeId { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public virtual ICollection<TaskTodo>? Tasks { get; set; }
}
