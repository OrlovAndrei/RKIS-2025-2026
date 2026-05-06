namespace TodoList.Api.DTOs;

public sealed class LoginResponse
{
    public string Token { get; set; } = "";
    public int UserId { get; set; }
    public Guid ProfileId { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
}
