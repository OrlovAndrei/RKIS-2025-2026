namespace TodoList;

public static class FileManager
{
    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
    }

    public static void SaveProfile(Profile profile, string filePath)
    {
        var data = $"{profile.FirstName} {profile.LastName} {profile.BirthYear}";
        File.WriteAllText(filePath, data);
    }

    public static Profile LoadProfile(string filePath)
    {
        var parts = File.ReadAllText(filePath).Split(' ');
        return new Profile(parts[0], parts[1], int.Parse(parts[2]));
    }
}