namespace Domain.Interfaces;

public interface IPasswordHashed
{
    Task<string> HashedAsync(string password, DateTime createAt);
    Task<bool> VerifyAsync(string password, DateTime createAt, string hashedPassword);
}