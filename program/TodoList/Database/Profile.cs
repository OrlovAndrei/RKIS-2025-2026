namespace ShevricTodo.Database;

internal class Profile
{
	public int UserId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? UserName { get; set; }
	public DateTime? DateOfCreate { get; set; }
	public DateTime? Birthday { get; set; }
	public string? HashPassword { get; set; }
	public virtual ICollection<TaskTodo>? Tasks { get; set; }
}
