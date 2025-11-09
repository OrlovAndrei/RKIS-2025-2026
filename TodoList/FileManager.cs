using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace TodoApp
{
	public static class FileManager
	{
		public static void EnsureDataDirectory(string dirPath)
		{
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}
		}
		public static void SaveProfile(Profile profile, string filePath)
		{
			string line = $"{profile.FirstName};{profile.LastName};{profile.BirthYear}";
			File.WriteAllText(filePath, line, System.Text.Encoding.UTF8);
		}
		public static Profile LoadProfile(string filePath)
		{
			if (!File.Exists(filePath))
				return null;

			try
			{
				string line = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
				string[] parts = line.Split(';');
				if (parts.Length >= 3)
				{
					return new Profile(parts[0], parts[1], int.Parse(parts[2]));
				}
			}
			catch { }
			return null;
		}
		public static void SaveTodos(TodoList todos, string filePath)
		{
			Console.WriteLine($"[SAVE] Всего задач в списке (незавершённые): {todos._count}");
			var lines = new List<string>();
			for (int i = 0; i < todos._count; i++)
			{
				var item = todos[i];
				if (!item.IsDone)
				{
					string date = item.LastUpdate.ToString("yyyy-MM-dd");
					string text = item.Text.Replace("\n", " ").Replace("\r", " ");
					lines.Add($"(A) {text} {date}");
					string line = $"(A) {text} {date}";
					lines.Add(line);
					Console.WriteLine($"[TODO] Сохраняем строку: {line}");
				}
			}
			Console.WriteLine($"[SAVE] Записываем {lines.Count} строк в {filePath}");
			try
			{
				File.WriteAllLines(filePath, lines, System.Text.Encoding.UTF8);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[ОШИБКА] Не удалось сохранить {filePath}: {ex.Message}");
			}
		}
		public static void SaveDoneTodos(TodoList todos, string doneFilePath)
		{
			Console.WriteLine($"[SAVE] Всего задач в списке (завершённые): {todos._count}");
			var lines = new List<string>();
			for (int i = 0; i < todos._count; i++)
			{
				var item = todos[i];
				if (item.IsDone)
				{
					string date = item.LastUpdate.ToString("yyyy-MM-dd");
					string text = item.Text.Replace("\n", " ").Replace("\r", " ");
					string line = $"x {date} {text}";
					lines.Add($"x {date} {text}");
					Console.WriteLine($"[DONE] Сохраняем строку: {line}");
				}
			}
			Console.WriteLine($"[SAVE] Записываем {lines.Count} строк в {doneFilePath}");
			try
			{
				File.WriteAllLines(doneFilePath, lines, System.Text.Encoding.UTF8);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[ОШИБКА] Не удалось сохранить {doneFilePath}: {ex.Message}");
			}
		}
		public static TodoList LoadTodos(string todoFilePath, string doneFilePath)
		{
			var todoList = new TodoList();
			if (!File.Exists(todoFilePath))
			{
				foreach (var line in File.ReadAllLines(todoFilePath, System.Text.Encoding.UTF8))
				{
					if (string.IsNullOrWhiteSpace(line)) continue;
					var parts = line.Split(' ');
					if (parts.Length < 2) continue;

					string dateStr = parts[^1];
					DateTime lastUpdate;
					if (!DateTime.TryParseExact(dateStr, "yyyy-MM-dd", null,
						System.Globalization.DateTimeStyles.None, out lastUpdate))
						lastUpdate = DateTime.Now;

					string text = string.Join(" ", parts[..^1]);
					text = text.Replace("(A) ", "").Trim();

					var item = new TodoItem(text);
					item.LastUpdate = lastUpdate;
					todoList.Add(item);
				}
			}

			if (File.Exists(doneFilePath))
			{
				foreach (var line in File.ReadAllLines(doneFilePath, System.Text.Encoding.UTF8))
				{
					if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("x ")) continue;

					string dateStr = line.Substring(2, 10);
					DateTime lastUpdate;
					if (!DateTime.TryParseExact(dateStr, "yyyy-MM-dd", null,
						System.Globalization.DateTimeStyles.None, out lastUpdate))
						lastUpdate = DateTime.Now;

					string text = line.Substring(13);
					var item = new TodoItem(text);
					item.IsDone = true;
					item.LastUpdate = lastUpdate;
					todoList.Add(item);
				}
			}

			return todoList;
		}
	}
}
