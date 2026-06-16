namespace TodoList.Infrastructure;

public static class FileManager
{
	public static void EnsureDataDirectory(string dirPath)
	{
		if (!Directory.Exists(dirPath))
		{
			Directory.CreateDirectory(dirPath);
		}
	}

	public static void EnsureDataFile(string path)
	{
		if (!File.Exists(path))
		{
			new FileInfo(path).Create();
		}
	}
}