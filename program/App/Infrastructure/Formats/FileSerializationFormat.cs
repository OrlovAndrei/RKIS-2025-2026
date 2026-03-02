namespace ShevricTodo.Formats;

public abstract class FileSerializationFormat
{
	public string? Path;
	internal void IsPathNull()
	{
		if (Path is null)
		{
			throw new NullReferenceException("Вы не задали значение пути при инициализации класса.");
		}
	}
	internal static void IsFileExist(string path)
	{
		if (!File.Exists(path))
		{
			throw new FileNotFoundException($"Файл {path} не найден.");
		}
	}
	public static string StringInfo(string path)
	{
		IsFileExist(path);
		FileInfo fileInfo = new(path);
		return string.Format("Файл: {0}\nBytes: {1}", path, fileInfo.Length);
	}
	public async Task<string> StringInfoAsync()
	{
		IsPathNull();
		return StringInfo(Path!);
	}
}
