using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Api.DTOs;
using TodoList.Api.Services;
using TodoList.Data;
using TodoList.Models;

namespace TodoList.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly JwtTokenService _tokenService;

    public AuthController(JwtTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Email, username and password are required.");
        }

        await using var context = new AppDbContext();
        var email = request.Email.Trim();
        var username = request.Username.Trim();

        var userExists = await context.Users.AnyAsync(user =>
            user.Email == email || user.Username == username);
        if (userExists)
        {
            return Conflict("User already exists.");
        }

        var profile = new Profile(
            Guid.NewGuid(),
            email,
            "",
            string.IsNullOrWhiteSpace(request.FirstName) ? username : request.FirstName.Trim(),
            request.LastName.Trim(),
            request.BirthYear);

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = PasswordHasher.Hash(request.Password),
            Role = "User",
            ProfileId = profile.Id,
            Profile = profile
        };

        context.Profiles.Add(profile);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Ok(ToLoginResponse(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        await using var context = new AppDbContext();
        var email = request.Email.Trim();
        var user = await context.Users.FirstOrDefaultAsync(item => item.Email == email);

        if (user == null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid email or password.");
        }

        return Ok(ToLoginResponse(user));
    }

    private LoginResponse ToLoginResponse(User user)
    {
        return new LoginResponse
        {
            Token = _tokenService.CreateToken(user),
            UserId = user.Id,
            ProfileId = user.ProfileId,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }
}
