using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoApp.API.Data;
using TodoApp.API.DTOs;
using TodoApp.API.Models;

namespace TodoApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TodosController : ControllerBase
{
    private readonly AppDbContext _context;

    public TodosController(AppDbContext context)
    {
        _context = context;
    }

    private Guid GetCurrentProfileId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (userIdClaim == null) throw new UnauthorizedAccessException();
        return Guid.Parse(userIdClaim);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TodoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var profileId = GetCurrentProfileId();
        var todos = await _context.Todos
            .Where(t => t.ProfileId == profileId)
            .OrderBy(t => t.Id)
            .Select(t => new TodoResponseDto
            {
                Id = t.Id,
                Text = t.Text,
                Status = t.Status,
                LastUpdate = t.LastUpdate
            })
            .ToListAsync();

        return Ok(todos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TodoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var profileId = GetCurrentProfileId();
        var todo = await _context.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.ProfileId == profileId);

        if (todo == null) return NotFound();

        return Ok(new TodoResponseDto
        {
            Id = todo.Id,
            Text = todo.Text,
            Status = todo.Status,
            LastUpdate = todo.LastUpdate
        });
    }

    [HttpPost]
    [ProducesResponseType(typeof(TodoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] TodoCreateDto dto)
    {
        var profileId = GetCurrentProfileId();
        var todo = new TodoItem
        {
            Text = dto.Text,
            ProfileId = profileId,
            Status = TodoStatus.NotStarted,
            LastUpdate = DateTime.UtcNow
        };

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = todo.Id }, new TodoResponseDto
        {
            Id = todo.Id,
            Text = todo.Text,
            Status = todo.Status,
            LastUpdate = todo.LastUpdate
        });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TodoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] TodoUpdateDto dto)
    {
        var profileId = GetCurrentProfileId();
        var todo = await _context.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.ProfileId == profileId);

        if (todo == null) return NotFound();

        todo.UpdateText(dto.Text);
        await _context.SaveChangesAsync();

        return Ok(new TodoResponseDto
        {
            Id = todo.Id,
            Text = todo.Text,
            Status = todo.Status,
            LastUpdate = todo.LastUpdate
        });
    }

    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(TodoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] TodoStatusUpdateDto dto)
    {
        var profileId = GetCurrentProfileId();
        var todo = await _context.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.ProfileId == profileId);

        if (todo == null) return NotFound();

        todo.SetStatus(dto.Status);
        await _context.SaveChangesAsync();

        return Ok(new TodoResponseDto
        {
            Id = todo.Id,
            Text = todo.Text,
            Status = todo.Status,
            LastUpdate = todo.LastUpdate
        });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var profileId = GetCurrentProfileId();
        var todo = await _context.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.ProfileId == profileId);

        if (todo == null) return NotFound();

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}