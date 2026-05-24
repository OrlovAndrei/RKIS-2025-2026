public static class UserHandlers
{
	public static IResult GetAll(List<User> users) => Results.Ok(users);

	public static IResult GetById(int id, List<User> users)
	{
		var user = users.FirstOrDefault(u => u.Id == id);
		return user is null
			? Results.NotFound($"Пользователь с id={id} не найден")
			: Results.Ok(user);
	}

	public static IResult Create(UserDto dto, List<User> users, ref int nextId)
	{
		if (string.IsNullOrWhiteSpace(dto.Name))
			return Results.BadRequest("Имя пользователя обязательно.");
		var user = new User(nextId++, dto.Name.Trim(), dto.Email);
		users.Add(user);
		return Results.Created($"/users/{user.Id}", user);
	}

	public static IResult Update(int id, UserDto dto, List<User> users)
	{
		var index = users.FindIndex(u => u.Id == id);
		if (index == -1)
			return Results.NotFound($"Пользователь с id={id} не найден");
		if (string.IsNullOrWhiteSpace(dto.Name))
			return Results.BadRequest("Имя пользователя обязательно.");
		users[index] = users[index] with { Name = dto.Name.Trim(), Email = dto.Email };
		return Results.Ok(users[index]);
	}

	public static IResult Delete(int id, List<User> users)
	{
		var user = users.FirstOrDefault(u => u.Id == id);
		if (user is null)
			return Results.NotFound($"Пользователь с id={id} не найден");
		users.Remove(user);
		return Results.NoContent();
	}
}