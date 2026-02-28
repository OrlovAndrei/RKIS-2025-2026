using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Text;

namespace TodoList
{
	public static class FileManager
	{
		private const char Separator = ';';
		private const string NewLineReplacement = "\\n";

		public static void EnsureDataDirectory(string dirPath)
		{
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}
		}

		public static void SaveProfile(Profile profile, string filePath)
		{
			try
			{
				string jsonString = JsonSerializer.Serialize(profile, new JsonSerializerOptions { WriteIndented = true });
				File.WriteAllText(filePath, jsonString);
				Console.WriteLine($"Данные пользователя сохранены: {filePath}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка сохранения профиля: {ex.Message}");
			}
		}

		public static Profile LoadProfile(string filePath)
		{
			if (!File.Exists(filePath)) return null;

			try
			{
				string jsonString = File.ReadAllText(filePath);
				return JsonSerializer.Deserialize<Profile>(jsonString);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка загрузки профиля: {ex.Message}");
				return null;
			}
		}

		public static void SaveTasks(TodoList todoList, string filePath)
		{
			try
			{
				StringBuilder csvContent = new StringBuilder();
				foreach (var item in todoList)
				{
					string escapedText = EscapeTextForCsv(item.Text);
					string line = $"{escapedText}{Separator}{item.Status}{Separator}{item.LastUpdate:o}";
					csvContent.AppendLine(line);
				}
				File.WriteAllText(filePath, csvContent.ToString(), Encoding.UTF8);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при сохранении задач: {ex.Message}");
			}
		}

		public static TodoList LoadTasks(string filePath)
		{
			TodoList todoList = new TodoList();
			if (!File.Exists(filePath)) return todoList;

			try
			{
				string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
				foreach (string line in lines)
				{
					if (string.IsNullOrWhiteSpace(line)) continue;

					string[] parts = ParseCsvLine(line);
					if (parts.Length >= 3)
					{
						string text = UnescapeTextFromCsv(parts[0]);

						if (!Enum.TryParse(parts[1], out TodoStatus status))
						{
							status = TodoStatus.NotStarted;
						}

						if (!DateTime.TryParse(parts[2], out DateTime lastUpdate))
						{
							lastUpdate = DateTime.Now;
						}

						todoList.Add(new TodoItem(text, status, lastUpdate));
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка загрузки задач: {ex.Message}");
			}

			return todoList;
		}

		private static string EscapeTextForCsv(string data)
		{
			if (string.IsNullOrEmpty(data)) return string.Empty;
			string escapedData = data.Replace("\n", NewLineReplacement);
			escapedData = escapedData.Replace("\"", "\"\"");
			if (escapedData.Contains(Separator.ToString()) || escapedData.Contains("\""))
			{
				escapedData = $"\"{escapedData}\"";
			}
			return escapedData;
		}

		private static string UnescapeTextFromCsv(string data)
		{
			if (string.IsNullOrEmpty(data)) return string.Empty;
			if (data.StartsWith("\"") && data.EndsWith("\""))
			{
				data = data.Substring(1, data.Length - 2);
			}
			string unescapedData = data.Replace("\"\"", "\"");
			return unescapedData.Replace(NewLineReplacement, "\n");
		}

		private static string[] ParseCsvLine(string line)
		{
			List<string> parts = new List<string>();
			StringBuilder sb = new StringBuilder();
			bool inQuotes = false;

			for (int i = 0; i < line.Length; i++)
			{
				char c = line[i];
				if (c == '\"')
				{
					if (inQuotes && i + 1 < line.Length && line[i + 1] == '\"')
					{
						sb.Append('\"');
						i++;
					}
					else
					{
						inQuotes = !inQuotes;
					}
				}
				else if (c == Separator && !inQuotes)
				{
					parts.Add(sb.ToString());
					sb.Clear();
				}
				else
				{
					sb.Append(c);
				}
			}
			parts.Add(sb.ToString());
			return parts.ToArray();
		}
	}
}