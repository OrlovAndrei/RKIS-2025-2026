var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

var users = new List<User>();
var nextId = 1;

var usersGroup = app.MapGroup("/users");

usersGroup.MapGet("", () => UserHandlers.GetAll(users));
usersGroup.MapGet("/{id}", (int id) => UserHandlers.GetById(id, users));
usersGroup.MapPost("", (UserDto dto) => UserHandlers.Create(dto, users, ref nextId));
usersGroup.MapPut("/{id}", (int id, UserDto dto) => UserHandlers.Update(id, dto, users));
usersGroup.MapDelete("/{id}", (int id) => UserHandlers.Delete(id, users));

app.Run();

public record User(int Id, string Name, string Email);
public record UserDto(string Name, string Email);