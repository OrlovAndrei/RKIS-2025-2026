using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;

namespace TodoList
{
	public static class FileManager
	{
		public static void EnsureDataDirectory(string dirPath)
		{
			if (!Directory.Exists(dirPath))
				Directory.CreateDirectory(dirPath);
		}

		public static void SaveProfiles(List<Profile> profiles, string filePath)
		{
			if (profiles == null || profiles.Count == 0)
			{
				File.WriteAllText(filePath, "Id;Login;Password;FirstName;LastName;BirthYear");
				return;
			}

			var lines = new List<string> { "Id;Login;Password;FirstName;LastName;BirthYear" };
			foreach (var profile in profiles)
			{
				lines.Add($"{profile.Id};{profile.Login};{profile.Password};{profile.FirstName};{profile.LastName};{profile.BirthYear}");
			}
			File.WriteAllLines(filePath, lines);
		}

		public static List<Profile> LoadProfiles(string filePath)
		{
			var profiles = new List<Profile>();

			if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
				return profiles;

			var lines = File.ReadAllLines(filePath);
			if (lines.Length <= 1)
				return profiles;

			for (int i = 1; i < lines.Length; i++)
			{
				var line = lines[i];
				if (string.IsNullOrWhiteSpace(line))
					continue;

				var parts = line.Split(';');
				if (parts.Length < 6)
					continue;

				try
				{
					var profile = new Profile(
						Guid.Parse(parts[0]),
						parts[1],
						parts[2],
						parts[3],
						parts[4],
						int.Parse(parts[5])
					);
					profiles.Add(profile);
				}
				catch (FormatException)
				{
					continue;
				}
			}

			return profiles;
		}

		public static void SaveTodos(TodoList todos, string filePath)
		{
			if (todos == null) return;

			using var writer = new StreamWriter(filePath);
			var tasks = todos.tasks;

			writer.WriteLine("Index;Text;Status;LastUpdate");

			for (int i = 0; i < tasks.Count; i++)
			{
				var t = tasks[i];
				string textEscaped = t.Text.Replace("\n", "\\n").Replace("\"", "\"\"");
				writer.WriteLine($"{i};\"{textEscaped}\";{t.Status};{t.LastUpdate:O}");
			}
		}

		public static TodoList LoadTodos(string filePath)
		{
			var todoList = new TodoList();

			if (!File.Exists(filePath))
				return todoList;

			string[] lines = File.ReadAllLines(filePath);
			if (lines.Length <= 1)
				return todoList;

			for (int i = 1; i < lines.Length; i++)
			{
				string line = lines[i];
				if (string.IsNullOrWhiteSpace(line))
					continue;

				var parts = new List<string>();
				bool inQuotes = false;
				string currentPart = "";

				foreach (char c in line)
				{
					if (c == '"')
					{
						inQuotes = !inQuotes;
					}
					else if (c == ';' && !inQuotes)
					{
						parts.Add(currentPart);
						currentPart = "";
					}
					else
					{
						currentPart += c;
					}
				}
				parts.Add(currentPart);

				if (parts.Count < 4)
					continue;

				string textRaw = parts[1].Trim('"').Replace("\\n", "\n").Replace("\"\"", "\"");
				TodoStatus status = Enum.Parse<TodoStatus>(parts[2]);
				DateTime.TryParse(parts[3], null, DateTimeStyles.RoundtripKind, out DateTime date);

				var item = new TodoItem(textRaw, status, date);
				todoList.tasks.Add(item);
			}

			return todoList;
		}
	}
}