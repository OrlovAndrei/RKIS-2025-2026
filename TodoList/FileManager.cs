namespace TodoList;

public class FileManager
{
	public const string dataDirPath = "data";
	public static string profilePath = Path.Combine(dataDirPath, "profile.txt");
	
	public static void EnsureDataDirectory(string dirPath)
	{
		if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
	}
	
	public static void SaveProfile(Profile profile)
	{
		File.WriteAllText(profilePath, $"{profile.FirstName} {profile.LastName} {profile.BirthYear}");
	}
	
	public static Profile LoadProfile()
	{
		var lines = File.ReadAllText(profilePath).Split();
		return new Profile(lines[0], lines[1], int.Parse(lines[2]));
	}
}