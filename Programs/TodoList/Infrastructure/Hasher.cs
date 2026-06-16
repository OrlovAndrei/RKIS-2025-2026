using TodoList.Interfaces;

namespace TodoList.Infrastructure;

public class Hasher : IHasher
{
    public string Hashed(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}