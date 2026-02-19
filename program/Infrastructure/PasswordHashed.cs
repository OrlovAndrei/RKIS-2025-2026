using Application.Interfaces;
using Infrastructure.Authentication;

namespace Infrastructure;

public class PasswordHashed : IPasswordHashed
{
	public string Hashed(string password)
	{
        return Encryption.CreateSHA256(password, password);
	}

	public bool Verify(string password, string hash)
	{
		return Encryption.CreateSHA256(password, password) == hash;
	}
}