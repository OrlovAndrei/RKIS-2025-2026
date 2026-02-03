namespace ShevricTodo.Database;

internal class TaskTodo
{
	public int TaskId { get; set; }
	public int TypeId { get; set; }
	public int StateId { get; set; }
	public int UserId { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public DateTime DateOfCreate { get; set; }
	public DateTime? DateOfStart { get; set; }
	public DateTime? DateOfEnd { get; set; }
	public DateTime? Deadline { get; set; }
	public virtual Profile? TaskCreator { get; set; }
	public virtual TypeOfTask? TypeOfTask { get; set; }
	public virtual StateOfTask? StateOfTask { get; set; }
}
