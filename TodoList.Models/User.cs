namespace TodoList.Models;

public sealed class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Role { get; set; } = "User";
    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;
}
