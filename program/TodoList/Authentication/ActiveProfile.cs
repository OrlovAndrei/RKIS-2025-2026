using ShevricTodo.Database;
using ShevricTodo.Formats;

namespace ShevricTodo.Authentication;

internal static class ActiveProfile
{
	public static async Task<bool> TruePassword(string password)
	{
		Profile profile = await ActiveProfile.Read()
			?? throw new ArgumentNullException("Нет подходящего аккаунта.");
		return profile.HashPassword ==
			await Encryption.CreatePasswordHash(password, profile.DateOfCreate);
	}
	public static async Task<Profile> GetActiveProfile()
	{
		return await Read()
			?? throw new NullReferenceException(
				"Не существует активного профиля. Попробуйте пересоздать профиль.");
	}
	public static readonly string PathToProfile = CreatePath.CreatePathToFileInSpecialFolder(directory: ProgramConst.AppName, fileName: "Profile.json");
	public static async Task Update(Profile updateToProfile)
	{
		Json<Profile>.Serialization(value: updateToProfile, path: PathToProfile);
	}
	public static async Task<Profile?> Read()
	{
		return Json<Profile>.Deserialization(path: PathToProfile);
	}
}
