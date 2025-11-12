namespace TodoList;

public class FileManager
{
	public const string dataDirPath = "data";
	public static string profilePath = Path.Combine(dataDirPath, "profile.txt");
	public static readonly string TodoPath = Path.Combine(dataDirPath, "todos.csv");
	
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
	
	public static void SaveTodos(TodoList todoList)
	{
		List<string> lines = [];

		for (var i = 0; i < todoList.taskCount; i++)
		{
			var item = todoList.items[i];
			var text = EscapeCsv(item.Text);
			lines.Add($"{i};{text};{item.IsDone};{item.LastUpdate:O}");
		}

		File.WriteAllLines(TodoPath, lines);
		string EscapeCsv(string text)
			=> "\"" + text.Replace("\"", "\"\"").Replace("\n", "\\n") + "\"";
	}
}