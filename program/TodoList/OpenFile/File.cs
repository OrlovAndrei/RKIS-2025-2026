namespace TodoList;

public enum TypeFile
{
	Standard,
	Config,
	Temporary,
	Index,
	IndexAndTemporary
}

public partial class OpenFile
{
	public string FullPath { get; private set; }
	public string NameFile { get; private set; }
	public string DirectoryName { get; private set; }
	/// <summary>
	/// Окончание для файла конфигурации
	/// </summary>
	public static readonly string PrefConfigFile = "_conf";
	/// <summary>
	/// Окончание для временного файла
	/// </summary>
	private static readonly string PrefTemporaryFile = "_temp";
	/// <summary>
	/// Окончание файла для работы с индексами
	/// </summary>
	private static readonly string PrefIndex = "_index";

	public OpenFile(string nameFile, TypeFile typeFile = TypeFile.Standard, string directoryName = "RKIS-TodoList")
	{
		NameFile = typeFile switch
		{
			TypeFile.Standard => nameFile,
			TypeFile.Config => nameFile + PrefConfigFile,
			TypeFile.Temporary => nameFile + PrefTemporaryFile,
			TypeFile.Index => nameFile + PrefIndex,
			TypeFile.IndexAndTemporary => nameFile + PrefIndex + PrefTemporaryFile,
			_ => nameFile
		};
		DirectoryName = directoryName;
		FullPath = CreatePath();
	}
	private string CreatePath(string extension = "csv")
	{
		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DirectoryName);
		DirectoryInfo? directory = new(path); // Инициализируем объект класса для создания директории
		if (!directory.Exists) Directory.CreateDirectory(path); // Если директория не существует, то мы её создаём по пути fullPath
		return Path.Combine(path, $"{NameFile}.{extension}");
	}
}