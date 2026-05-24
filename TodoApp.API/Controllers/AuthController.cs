using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApp.API.Data;
using TodoApp.API.DTOs;
using TodoApp.API.Models;

namespace TodoApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }


    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (await _context.Profiles.AnyAsync(p => p.Login == request.Login))
            return BadRequest(new { message = "Login already exists" });

        var profile = new Profile(
            request.Login,
            request.Password,
            request.FirstName,
            request.LastName,
            request.BirthYear
        );

        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(profile);
        return Ok(new AuthResponse
        {
            Token = token,
            Login = profile.Login,
            FullName = profile.FullName,
            ProfileId = profile.Id
        });
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var profile = await _context.Profiles
            .FirstOrDefaultAsync(p => p.Login == request.Login);

        if (profile == null || !profile.CheckPassword(request.Password))
            return Unauthorized(new { message = "Invalid login or password" });

        var token = GenerateJwtToken(profile);
        return Ok(new AuthResponse
        {
            Token = token,
            Login = profile.Login,
            FullName = profile.FullName,
            ProfileId = profile.Id
        });
    }

    private string GenerateJwtToken(Profile profile)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not configured");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, profile.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, profile.Login),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"] ?? "60"));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}