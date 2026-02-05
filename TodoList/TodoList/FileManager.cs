
public static class FileManager
{
	public static void EnsureDataDirectory(string dirPath)
	{
		if (!Directory.Exists(dirPath))
		{
			Directory.CreateDirectory(dirPath);
			Console.WriteLine($"Создана директория: {dirPath}");
		}
	}
	public static void SaveProfile(Profile profile, string filePath)
	{
		try
		{
			bool fileExists = File.Exists(filePath);
			var lines = new List<string>();

			if (fileExists)
			{
				lines = File.ReadAllLines(filePath).ToList();
				bool profileExists = false;
				for (int i = 0; i < lines.Count; i++)
				{
					if (i == 0) continue;
					string[] parts = lines[i].Split(';');
					if (parts.Length > 0 && Guid.TryParse(parts[0], out Guid lineId) && lineId == profile.Id)
					{
						lines[i] = $"{profile.Id};{profile.Login};{profile.Password};{profile.FirstName};{profile.LastName};{profile.BirthYear}";
						profileExists = true;
						break;
					}
				}
				if (!profileExists)
				{
					lines.Add($"{profile.Id};{profile.Login};{profile.Password};{profile.FirstName};{profile.LastName};{profile.BirthYear}");
				}
			}
			else
			{
				lines.Add("Id;Login;Password;FirstName;LastName;BirthYear");
				lines.Add($"{profile.Id};{profile.Login};{profile.Password};{profile.FirstName};{profile.LastName};{profile.BirthYear}");
			}

			File.WriteAllLines(filePath, lines);
			Console.WriteLine("Профиль сохранен");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка сохранения профиля: {ex.Message}");
		}
	}
	public static List<Profile> LoadProfiles(string filePath)
	{
		var profiles = new List<Profile>();
		try
		{
			if (!File.Exists(filePath))
			{
				Console.WriteLine("Файл профилей не найден");
				return profiles;
			}
			string[] lines = File.ReadAllLines(filePath);
			for (int i = 1; i < lines.Length; i++)
			{
				string line = lines[i];
				if (!string.IsNullOrEmpty(line))
				{
					string[] parts = line.Split(';');
					if (parts.Length == 6)
					{
						Guid id = Guid.Parse(parts[0]);
						string login = parts[1];
						string password = parts[2];
						string firstName = parts[3];
						string lastName = parts[4];

						if (int.TryParse(parts[5], out int birthYear))
						{
							var profile = new Profile(id, login, password, firstName, lastName, birthYear);
							profiles.Add(profile);
						}
					}
				}
			}

			Console.WriteLine($"Загружено профилей: {profiles.Count}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка загрузки профилей: {ex.Message}");
		}

		return profiles;
	}

	public static Profile LoadProfile(string filePath)
	{
		try
		{
			if (!File.Exists(filePath))
			{
				Console.WriteLine("Файл профиля не найден");
				return null;
			}
			string line = File.ReadAllText(filePath);
			if (!string.IsNullOrEmpty(line))
			{
				string[] parts = line.Split('|');
				if (parts.Length == 3)
				{
					string firstName = parts[0];
					string lastName = parts[1];
					if (int.TryParse(parts[2], out int birthYear))
					{
						return new Profile(firstName, lastName, birthYear);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка загрузки профиля: {ex.Message}");
		}
		return null;
	}
	public static void SaveUserTodos(Guid userId, TodoList todos, string dataDir)
	{
		try
		{
			if (string.IsNullOrEmpty(dataDir) || userId == Guid.Empty || todos == null)
			{
				return;
			}
			string filePath = Path.Combine(dataDir, $"todos_{userId}.csv");
			var lines = new List<string>
			{
				"Index;Text;Status;LastUpdate"
			};
			for (int i = 0; i < todos.Count; i++)
			{
				var item = todos.GetItem(i);
				string escapedText = item.GetText().Replace("\"", "\"\"").Replace("\n", "\\n").Replace("\r", "\\r");
				lines.Add($"{i};\"{escapedText}\";{item.GetStatus()};{item.GetLastUpdate():yyyy-MM-dd HH:mm:ss}");
			}
			File.WriteAllLines(filePath, lines);
			Console.WriteLine($"Задачи пользователя {userId} сохранены");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка сохранения задач: {ex.Message}");
		}
	}
	public static TodoList LoadUserTodos(Guid userId, string dataDir)
	{
		var todoList = new TodoList();
		try
		{
			string filePath = Path.Combine(dataDir, $"todos_{userId}.csv");
			if (!File.Exists(filePath))
			{
				Console.WriteLine($"Файл задач пользователя {userId} не найден. Будет создан новый.");
				return todoList;
			}
			string[] lines = File.ReadAllLines(filePath);
			for (int i = 1; i < lines.Length; i++)
			{
				string line = lines[i];
				if (!string.IsNullOrEmpty(line))
				{
					string[] parts = ParseCsvLine(line, ';');
					if (parts.Length == 4)
					{
						string text = parts[1].Replace("\"\"", "\"").Replace("\\n", "\n").Replace("\\r", "\r");
						TodoStatus status = (TodoStatus)Enum.Parse(typeof(TodoStatus), parts[2]);
						DateTime lastUpdate = DateTime.Parse(parts[3]);
						var todoItem = new TodoItem(text, status, lastUpdate);
						todoList.Add(todoItem);
					}
				}
			}
			Console.WriteLine($"Загружено задач пользователя {userId}: {todoList.Count}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка загрузки задач: {ex.Message}");
		}
		return todoList;
	}
	private static string[] ParseCsvLine(string line, char separator = ';')
	{
		var parts = new List<string>();
		int start = 0;
		bool inQuotes = false;
		for (int i = 0; i < line.Length; i++)
		{
			if (line[i] == '"')
			{
				inQuotes = !inQuotes;
			}
			else if (line[i] == separator && !inQuotes)
			{
				string part = line.Substring(start, i - start);
				if (part.StartsWith("\"") && part.EndsWith("\""))
				{
					part = part.Substring(1, part.Length - 2);
				}
				parts.Add(part);
				start = i + 1;
			}
		}
		if (start < line.Length)
		{
			string part = line.Substring(start);
			if (part.StartsWith("\"") && part.EndsWith("\""))
			{
				part = part.Substring(1, part.Length - 2);
			}
			parts.Add(part);
		}
		return parts.ToArray();
	}
}