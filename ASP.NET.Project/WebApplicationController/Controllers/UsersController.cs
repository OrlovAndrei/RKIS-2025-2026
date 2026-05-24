using Microsoft.AspNetCore.Mvc;

namespace MyWebAPI_Controllers.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
	private static readonly List<User> _users = new();
	private static int _nextId = 1;
	[HttpGet]
	public ActionResult<IEnumerable<User>> GetAll([FromQuery] string? search, [FromQuery] int page = 1)
	{
		var result = string.IsNullOrEmpty(search)
			? _users
			: _users.Where(u => u.Name.Contains(search)).ToList();
		var paged = result.Skip((page - 1) * 10).Take(10);
		return Ok(paged);
	}
	[HttpGet("{id}")]
	public ActionResult<User> GetById([FromRoute] int id)
	{
		var user = _users.FirstOrDefault(u => u.Id == id);
		return user is null ? NotFound() : Ok(user);
	}
	[HttpPost]
	public ActionResult<User> Create([FromBody] UserDto dto)
	{
		if (string.IsNullOrWhiteSpace(dto.Name))
			return BadRequest("Имя пользователя обязательно.");

		var user = new User(_nextId++, dto.Name.Trim(), dto.Email);
		_users.Add(user);
		return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
	}
	[HttpPut("{id}")]
	public ActionResult<User> Update([FromRoute] int id, [FromBody] UserDto dto)
	{
		var index = _users.FindIndex(u => u.Id == id);
		if (index == -1)
			return NotFound();

		if (string.IsNullOrWhiteSpace(dto.Name))
			return BadRequest("Имя пользователя обязательно.");

		_users[index] = new User(id, dto.Name.Trim(), dto.Email);
		return Ok(_users[index]);
	}
	[HttpDelete("{id}")]
	public IActionResult Delete([FromRoute] int id)
	{
		var user = _users.FirstOrDefault(u => u.Id == id);
		if (user is null)
			return NotFound();

		_users.Remove(user);
		return NoContent();
	}
}
public record User(int Id, string Name, string Email);
public record UserDto(string Name, string Email);