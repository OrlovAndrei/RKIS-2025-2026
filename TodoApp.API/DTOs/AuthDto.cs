using System.ComponentModel.DataAnnotations;

namespace TodoApp.API.DTOs;

public class RegisterRequest
{
    [Required, MaxLength(50)]
    public string Login { get; set; } = string.Empty;

    [Required, MinLength(4)]
    public string Password { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Range(1900, 2100)]
    public int BirthYear { get; set; }
}

public class LoginRequest
{
    [Required]
    public string Login { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public Guid ProfileId { get; set; }
}