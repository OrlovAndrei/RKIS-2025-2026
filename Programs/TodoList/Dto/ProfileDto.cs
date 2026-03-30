namespace TodoList.Dto;

public static class ProfileDto
{
    public record Create(
        string Login,
        string? FirstName,
        string? LastName,
        DateOnly DateOfBirth,
        string Password
    );
}