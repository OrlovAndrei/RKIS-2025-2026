namespace ShevricTodo.Database;

internal class Profile
{
	public int UserId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? UserName { get; set; }
	public DateOnly Birthday { get; set; }
	public string? HashPassword { get; set; }
	public virtual ICollection<Task>? Tasks { get; set; }
}
