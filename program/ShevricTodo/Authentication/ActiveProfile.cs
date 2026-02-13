using ShevricTodo.Database;
using ShevricTodo.Formats;

namespace ShevricTodo.Authentication;

internal static class ActiveProfile
{
	internal static readonly string PathToProfile =
		CreatePath.CreatePathToFileInSpecialFolder(directory: ProgramConst.AppName, fileName: "Profile.json");
	public static async Task<bool> PasswordCheck(string password)
	{
		Profile profile = await Read()
			?? throw new ArgumentNullException("Нет подходящего аккаунта.");
		return profile.HashPassword ==
			await Encryption.CreatePasswordHash(password, profile.DateOfCreate);
	}
	internal static async Task<Profile> GetActiveProfile() => await Read()
			?? throw new NullReferenceException(
				"Не существует активного профиля. Попробуйте пересоздать профиль.");
	internal static async Task Update(Profile updateToProfile) => Json<Profile>.Serialization(value: updateToProfile, path: PathToProfile);
	internal static async Task<Profile?> Read() => Json<Profile>.Deserialization(path: PathToProfile);
}
