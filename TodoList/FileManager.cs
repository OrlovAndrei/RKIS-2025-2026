using System;
using System.IO;
using System.Text.Json;
using System.Linq;
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
				Console.WriteLine($"Данные пользователя сохранены в: {filePath}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка сохранения профиля: {ex.Message}");
			}
		}

		public static Profile LoadProfile(string filePath)
		{
			if (!File.Exists(filePath))
			{
				return null;
			}

			try
			{
				string jsonString = File.ReadAllText(filePath);
				Profile profile = JsonSerializer.Deserialize<Profile>(jsonString);
				Console.WriteLine($"Данные пользователя загружены из: {filePath}");
				return profile;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка загрузки профиля. Файл будет проигнорирован: {ex.Message}");
				return null;
			}
		}

		public static void SaveTodos(TodoList todos, string filePath)
		{
			try
			{
				var lines = todos.GetAllItems()
					.Select(item =>
					{
						string escapedText = EscapeTextForCsv(item.Text);
						string isDone = item.IsDone.ToString();
						string date = item.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss");

						return $"\"{escapedText}\"{Separator}{isDone}{Separator}{date}";
					})
					.ToList();

				lines.Insert(0, $"Text{Separator}IsDone{Separator}LastUpdate");

				File.WriteAllLines(filePath, lines);
				Console.WriteLine($"Задачи сохранены в: {filePath}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка сохранения задач: {ex.Message}");
			}
		}

		public static TodoList LoadTodos(string filePath)
		{
			var todoList = new TodoList();
			if (!File.Exists(filePath))
			{
				return todoList;
			}

			try
			{
				var lines = File.ReadAllLines(filePath);

				foreach (var line in lines.Skip(1))
				{
					string[] parts = ParseCsvLine(line);

					if (parts.Length != 3)
					{
						Console.WriteLine($"Пропущена некорректная строка CSV: {line}");
						continue;
					}

					string text = UnescapeTextFromCsv(parts[0]);
					bool isDone = bool.TryParse(parts[1], out bool done) && done;

					DateTime lastUpdate = DateTime.TryParseExact(parts[2], "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime date)
										  ? date
										  : DateTime.Now;

					var tempItem = new TodoItem(text);
					if (isDone) tempItem.MarkDone();

					todoList.Add(tempItem);
				}
				Console.WriteLine($"Задачи загружены из: {filePath}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка загрузки задач. Список будет пустым: {ex.Message}");
				return new TodoList();
			}

			return todoList;
		}

		private static string EscapeTextForCsv(string data)
		{
			if (data == null) return string.Empty;

			string escapedData = data.Replace("\n", NewLineReplacement);

			escapedData = escapedData.Replace("\"", "\"\"");

			return escapedData;
		}

		private static string UnescapeTextFromCsv(string data)
		{
			if (data == null) return string.Empty;

			if (data.StartsWith("\"") && data.EndsWith("\""))
			{
				data = data.Substring(1, data.Length - 2);
			}

			string unescapedData = data.Replace("\"\"", "\"");

			unescapedData = unescapedData.Replace(NewLineReplacement, "\n");

			return unescapedData;
		}

		private static string[] ParseCsvLine(string line)
		{
			var parts = new List<string>();
			var sb = new StringBuilder();
			bool inQuotes = false;

			for (int i = 0; i < line.Length; i++)
			{
				char c = line[i];

				if (c == '"' && i == 0)
				{
					inQuotes = true;
					continue;
				}

				if (c == '"' && inQuotes)
				{
					if (i + 1 < line.Length && line[i + 1] == '"')
					{
						sb.Append('"');
						i++;
						continue;
					}

					inQuotes = false;
					continue;
				}

				if (c == Separator && !inQuotes)
				{
					parts.Add(sb.ToString());
					sb.Clear();
					continue;
				}

				sb.Append(c);
			}
			parts.Add(sb.ToString());

			return parts.ToArray();
		}
	}
}