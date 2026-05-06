using TodoList.Models;

namespace TodoListDesktop.Services;

public sealed class LoginRequest
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public sealed class RegisterRequest
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public int BirthYear { get; set; }
}

public sealed class LoginResponse
{
    public string Token { get; set; } = "";
    public int UserId { get; set; }
    public Guid ProfileId { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
}

public sealed class TodoItemResponse
{
    public int Id { get; set; }
    public string Text { get; set; } = "";
    public TodoStatus Status { get; set; }
    public DateTime LastUpdate { get; set; }
}

public sealed class CreateTodoRequest
{
    public string Text { get; set; } = "";
}

public sealed class UpdateTodoRequest
{
    public string Text { get; set; } = "";
    public TodoStatus Status { get; set; }
}

public sealed class SetStatusRequest
{
    public TodoStatus Status { get; set; }
}
