namespace Infrastructure.Database;

public class ProfileEntity
{
	public string ProfileId { get; set; } = null!;
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public DateTime CreatedAt { get; set; }
	public DateTime DateOfBirth { get; set; }
	public string PasswordHash { get; set; } = null!;
	public virtual ICollection<TodoTaskEntity>? Tasks { get; set; }
}
