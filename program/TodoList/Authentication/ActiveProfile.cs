using ShevricTodo.Database;

namespace ShevricTodo.Authentication;

internal class ActiveProfile
{
	public async Task<bool> TruePassword(string password)
	{
		Profile profile = await ProfileFile.Read()
			?? throw new ArgumentNullException("Нет подходящего аккаунта.");
		return profile.HashPassword ==
			await Encryption.CreatePasswordHash(password, profile.DateOfCreate);
	}
}
