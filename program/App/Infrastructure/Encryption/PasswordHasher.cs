using Application.Interfaces;

namespace Infrastructure.Encryption;

public class PasswordHasher : IPasswordHasher
{
	public async Task<string> HashedAsync(string password)
	{
		return await Encryption.CreateSHA256(password, password);
	}
	public async Task<bool> VerifyAsync(string password, string hash)
	{
		return await HashedAsync(password: password) == hash;
	}
}