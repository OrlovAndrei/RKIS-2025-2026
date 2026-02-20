using Domain.Interfaces;

namespace Infrastructure;

public class PasswordHashed : IPasswordHashed
{
	public async Task<string> HashedAsync(string password, DateTime createAt)
	{
        return await Encryption.CreatePasswordHash(password, createAt);
	}
	public async Task<bool> VerifyAsync(string password, DateTime createAt, string hash)
	{
		return await Encryption.CreatePasswordHash(password, createAt) == hash;
	}
}