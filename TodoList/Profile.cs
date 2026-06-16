namespace TodoList;
public record Profile
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public int BirthYear { get; set; }
	public string Login {  get; set; }
	public string Password { get; set; }
	public Guid Id { get; set; }
}

