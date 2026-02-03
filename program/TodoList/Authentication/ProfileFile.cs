using ShevricTodo.Database;
using ShevricTodo.Formats;

namespace ShevricTodo.Authentication;

internal class ProfileFile
{
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
