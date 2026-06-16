namespace TodoList.Dto;

public static class LoginDto
{
    public record Login(
        string Password,
        Guid ProfileId
    );
}