using System;
using System.IO;
using System.Collections.Generic;

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
			using (StreamWriter writer = new StreamWriter(filePath))
			{
				writer.WriteLine($"{profile.GetType().GetField("_firstName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(profile)}|" +
							   $"{profile.GetType().GetField("_lastName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(profile)}|" +
							   $"{profile.GetType().GetField("_birthYear", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(profile)}");
			}
			Console.WriteLine("Профиль сохранен");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка сохранения профиля: {ex.Message}");
		}
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
			using (StreamReader reader = new StreamReader(filePath))
			{
				string line = reader.ReadLine();
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
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка загрузки профиля: {ex.Message}");
		}
		return null;
	}
	public static void SaveTodos(TodoList todos, string filePath)
	{
		try
		{
			using (StreamWriter writer = new StreamWriter(filePath))
			{
				writer.WriteLine("Text|IsDone|LastUpdate");
				for (int i = 0; i < todos.Count; i++)
				{
					var item = todos.GetItem(i);
					string escapedText = item.GetText().Replace("\"", "\"\"").Replace("\n", "\\n").Replace("\r", "\\r");
					writer.WriteLine($"\"{escapedText}\"|{item.GetIsDone()}|{item.GetLastUpdate():yyyy-MM-dd HH:mm:ss}");
				}
			}
			Console.WriteLine("Задачи сохранены");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка сохранения задач: {ex.Message}");
		}
	}
	public static TodoList LoadTodos(string filePath)
	{
		var todoList = new TodoList();

		try
		{
			if (!File.Exists(filePath))
			{
				Console.WriteLine("Файл задач не найден");
				return todoList;
			}
			using (StreamReader reader = new StreamReader(filePath))
			{
				string header = reader.ReadLine();
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (!string.IsNullOrEmpty(line))
					{
						string[] parts = ParseCsvLine(line);
						if (parts.Length == 3)
						{
							string text = parts[0].Replace("\"\"", "\"").Replace("\\n", "\n").Replace("\\r", "\r");
							bool isDone = bool.Parse(parts[1]);
							DateTime lastUpdate = DateTime.Parse(parts[2]);
							var todoItem = new TodoItem(text);
							if (isDone)
							{
								todoItem.MarkDone();
							}
							var lastUpdateField = todoItem.GetType().GetField("_lastUpdate",
								System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
							lastUpdateField?.SetValue(todoItem, lastUpdate);

							todoList.Add(todoItem);
						}
					}
				}
			}
			Console.WriteLine("Задачи загружены");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка загрузки задач: {ex.Message}");
		}

		return todoList;
	}
	private static string[] ParseCsvLine(string line)
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
			else if (line[i] == '|' && !inQuotes)
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
