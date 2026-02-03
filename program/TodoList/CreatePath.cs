using static System.Environment;

namespace ShevricTodo;

internal static class CreatePath
{
	/// <summary>
	/// Создает стандартную директорию в специальной папке
	/// </summary>
	/// <param name="directoryName"></param>
	/// <param name="specialFolder"></param>
	/// <returns></returns>
	public static string CreateDirectoryInSpecialFolder(
		string directoryName,
		SpecialFolder specialFolder = SpecialFolder.ApplicationData
		)
	{
		string path = Path.Combine(GetFolderPath(SpecialFolder.ApplicationData), directoryName);
		DirectoryInfo? directoryInfo = new(path); // Инициализируем объект класса для создания директории
		if (!directoryInfo.Exists) Directory.CreateDirectory(path); // Если директория не существует, то мы её создаём по пути fullPath
		return path;
	}
	/// <summary>
	/// Создает вложенные директории в специальной папке
	/// </summary>
	/// <param name="specialFolder"></param>
	/// <param name="directoryNames"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
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
	/// Создает путь к файлу но не сам файл, только директорию
	/// </summary>
	/// <param name="directoryName"></param>
	/// <param name="fileName"></param>
	/// <param name="specialFolder"></param>
	/// <returns></returns>
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
