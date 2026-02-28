namespace TodoList.classes;

public class FileManager
{
	public const string DataDirPath = "data";
	public static readonly string TodoPath = Path.Combine(DataDirPath, "todos.csv");
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
	
	public static void SaveTodos(TodoList todoList)
	{
		using var writer = new StreamWriter(TodoPath, false);

		for (var i = 0; i < todoList.Items.Count; i++)
		{
			var item = todoList.Items[i];
			var text = EscapeCsv(item.Text).Replace(";", "");
			writer.WriteLine($"{i};{text};{item.Status};{item.LastUpdate:O}");
		}
		string EscapeCsv(string text)
			=> "\"" + text.Replace("\"", "\"\"").Replace("\n", "\\n") + "\"";
	}
	
	public static TodoList LoadTodos()
	{
		var list = new TodoList();

		var lines = File.ReadAllLines(TodoPath);
		foreach (var line in lines)
		{
			var parts = line.Split(';');

			var text = UnescapeCsv(parts[1]);
			var status = Enum.Parse<TodoStatus>(parts[2]);
			var lastUpdate = DateTime.Parse(parts[3]);

			list.Add(new TodoItem(text, status, lastUpdate));
		}

		return list;
		
		string UnescapeCsv(string text)
			=> text.Trim('"').Replace("\\n", "\n").Replace("\"\"", "\"");
	}
}