namespace TodoList.Server.Models;

public class ProfileDto
{
	public Guid Id { get; set; }
	public string Login { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public int BirthYear { get; set; }
}