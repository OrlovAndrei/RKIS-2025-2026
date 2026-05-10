using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoList.Api.DTOs;
using TodoList.Data;
using TodoList.Models;

namespace TodoList.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/todos")]
public sealed class TodosController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;

    public TodosController(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemResponse>>> GetAll()
    {
        var profileId = GetProfileId();
        var todos = await _todoRepository.GetAllAsync(profileId);
        return Ok(todos.Select(ToResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoItemResponse>> GetById(int id)
    {
        var item = await _todoRepository.GetByIdAsync(id, GetProfileId());
        return item == null ? NotFound() : Ok(ToResponse(item));
    }

    [HttpPost]
    public async Task<ActionResult<TodoItemResponse>> Create(CreateTodoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest("Text is required.");
        }

        var item = new TodoItem(request.Text.Trim())
        {
            ProfileId = GetProfileId()
        };

        var created = await _todoRepository.AddAsync(item);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToResponse(created));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TodoItemResponse>> Update(int id, UpdateTodoRequest request)
    {
        var profileId = GetProfileId();
        var item = await _todoRepository.GetByIdAsync(id, profileId);
        if (item == null)
        {
            return NotFound();
        }

        item.Text = request.Text.Trim();
        item.Status = request.Status;
        item.LastUpdate = DateTime.Now;

        await _todoRepository.UpdateAsync(item);
        return Ok(ToResponse(item));
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> SetStatus(int id, SetStatusRequest request)
    {
        var updated = await _todoRepository.SetStatusAsync(id, request.Status, GetProfileId());
        return updated ? Ok() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _todoRepository.DeleteAsync(id, GetProfileId());
        return deleted ? NoContent() : NotFound();
    }

    private Guid GetProfileId()
    {
        var claimValue = User.FindFirstValue("profileId");
        if (!Guid.TryParse(claimValue, out var profileId))
        {
            throw new InvalidOperationException("Profile claim is missing.");
        }

        return profileId;
    }

    private static TodoItemResponse ToResponse(TodoItem item)
    {
        return new TodoItemResponse
        {
            Id = item.Id,
            Text = item.Text,
            Status = item.Status,
            LastUpdate = item.LastUpdate
        };
    }
}
