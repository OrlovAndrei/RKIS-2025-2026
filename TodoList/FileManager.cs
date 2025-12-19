namespace TodoList;

public class FileManager
{
	public const string DataDirPath = "data";
	public static readonly string ProfilePath = Path.Combine(DataDirPath, "profile.txt");
	
	public static void EnsureDataDirectory(string dirPath)
	{
		if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
	}

	public static void SaveProfile(Profile profile)
	{
		File.WriteAllText(ProfilePath, $"{profile.FirstName} {profile.LastName} {profile.BirthYear}");
	}

	public static Profile LoadProfile()
	{
		var lines = File.ReadAllText(ProfilePath).Split();
		return new Profile(lines[0], lines[1], int.Parse(lines[2]));
	}
}