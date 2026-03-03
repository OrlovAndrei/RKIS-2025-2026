namespace Application.Interfaces;

public interface IPasswordHasher
{
	Task<string> HashedAsync(string password);
	Task<bool> VerifyAsync(string password, string hashedPassword);
}