using static System.Environment;

namespace ShevricTodo;

internal static class CreatePath
{
	/// <summary>
	/// Creates a directory with the specified name in the designated special folder.
	/// </summary>
	/// <remarks>If the specified directory does not exist, it will be created. This method ensures that the
	/// directory is created in a location appropriate for application data storage.</remarks>
	/// <param name="directoryName">The name of the directory to create within the special folder. This parameter cannot be null or empty.</param>
	/// <param name="specialFolder">The special folder in which to create the directory. Defaults to the application data folder if not specified.</param>
	/// <returns>The full path of the created directory. If the directory already exists, returns the existing directory's path.</returns>
	public static string CreateDirectoryInSpecialFolder(
		string directoryName,
		SpecialFolder specialFolder = SpecialFolder.ApplicationData
		)
	{
		string path = Path.Combine(GetFolderPath(specialFolder), directoryName);
		DirectoryInfo? directoryInfo = new(path); // Инициализируем объект класса для создания директории
		if (!directoryInfo.Exists) Directory.CreateDirectory(path); // Если директория не существует, то мы её создаём по пути fullPath
		return path;
	}
	/// <summary>
	/// Creates a directory within the specified special folder, optionally including multiple nested directories as
	/// defined by the provided names.
	/// </summary>
	/// <remarks>This method allows for the creation of nested directories by specifying multiple names in the
	/// 'directoryNames' parameter. If the specified special folder does not exist, it will be created as needed.</remarks>
	/// <param name="specialFolder">The special folder in which to create the directory. Defaults to the application data folder if not specified.</param>
	/// <param name="directoryNames">An array of directory names to create within the special folder. At least one directory name must be provided.</param>
	/// <returns>The full path of the created directory, including any nested directories specified.</returns>
	/// <exception cref="ArgumentException">Thrown if no directory names are provided.</exception>
	public static string CreateDirectoryInSpecialFolder(
		SpecialFolder specialFolder = SpecialFolder.ApplicationData,
		params string[] directoryNames
		)
	{
		string directory = string.Empty;
		string path = string.Empty;
		bool firstCycle = true;
		if (directoryNames.Length == 0)
		{
			throw new ArgumentException();
		}
		foreach (string partsOfThePath in directoryNames)
		{
			directory = firstCycle
				? partsOfThePath
				: Path.Combine(directory, partsOfThePath);
			firstCycle = firstCycle && false;
			path = CreateDirectoryInSpecialFolder(
				directoryName: directory, specialFolder: specialFolder);
		}
		return path;
	}
	/// <summary>
	/// Creates a full file path within a specified special folder, optionally including additional subdirectories.
	/// </summary>
	/// <remarks>Any necessary subdirectories within the special folder will be created before returning the file
	/// path.</remarks>
	/// <param name="fileName">The name of the file for which to generate the path. Cannot be null or empty.</param>
	/// <param name="specialFolder">The special folder in which to create the file path. Defaults to the application data folder if not specified.</param>
	/// <param name="directory">An optional array of subdirectory names to include in the path. These directories will be created within the
	/// specified special folder if they do not exist.</param>
	/// <returns>A string containing the full path to the specified file within the designated special folder and subdirectories.</returns>
	public static string CreatePathToFileInSpecialFolder(
		string fileName,
		SpecialFolder specialFolder = SpecialFolder.ApplicationData,
		params string[] directory)
	{
		string directoryPath = CreateDirectoryInSpecialFolder(
			directoryNames: directory, specialFolder: specialFolder);
		string pathToFile = Path.Combine(directoryPath, fileName);
		return pathToFile;
	}
}
