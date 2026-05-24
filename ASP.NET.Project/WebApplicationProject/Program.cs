var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseHttpsRedirection();

var summaries = new[]
{
	"Freezing", "Bracing", "Chilly", "Cool", "Mild",
	"Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var users = new List<User>();
var nextId = 1;

app.MapGet("/users", () => Results.Ok(users));
app.MapGet("/users/{id}", (int id) =>
{
	var user = users.FirstOrDefault(u => u.Id == id);
	return user is null
		? Results.NotFound($"Пользователь с id={id} не найден")
		: Results.Ok(user);
});
app.MapPost("/users", (UserDto dto) =>
{
	if (string.IsNullOrWhiteSpace(dto.Name))
		return Results.BadRequest("Имя пользователя обязательно.");
	var user = new User(nextId++, dto.Name.Trim(), dto.Email);
	users.Add(user);
	return Results.Created($"/users/{user.Id}", user);
});
app.MapPut("/users/{id}", (int id, UserDto dto) =>
{
	var index = users.FindIndex(u => u.Id == id);
	if (index == -1)
		return Results.NotFound($"Пользователь с id={id} не найден");
	if (string.IsNullOrWhiteSpace(dto.Name))
		return Results.BadRequest("Имя пользователя обязательно.");
	users[index] = users[index] with { Name = dto.Name.Trim(), Email = dto.Email };
	return Results.Ok(users[index]);
});
app.MapDelete("/users/{id}", (int id) =>
{
	var user = users.FirstOrDefault(u => u.Id == id);
	if (user is null)
		return Results.NotFound($"Пользователь с id={id} не найден");
	users.Remove(user);
	return Results.NoContent();
});
app.MapGet("/weatherforecast", () =>
{
	var forecast = Enumerable.Range(1, 5).Select(index =>
		new WeatherForecast
		(
			DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
			Random.Shared.Next(-20, 55),
			summaries[Random.Shared.Next(summaries.Length)]
		))
		.ToArray();
	return forecast;
});
app.Run();

record User(int Id, string Name, string Email);
record UserDto(string Name, string Email);

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}