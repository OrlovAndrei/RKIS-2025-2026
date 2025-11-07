using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace TodoList1
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
			catch
			{
				return null;
			}
			return null;
		}
		public static void SaveTodos(TodoList todos, string filePath)
		{
			var lines = new List<string> { "Index;Text;IsDone;LastUpdate" }; ;

			for (int i = 0; i < todos._count; i++)
			{
				var item = todos[i];
				string text = item.Text
					.Replace("\n", "\\n")
					.Replace("\r", "\\r")
					.Replace(";", "\\;");
				string line = $"{i};{text};{item.IsDone};{item.LastUpdate:yyyy-MM-ddTHH:mm:ss}";
				lines.Add(line);
			}

			File.WriteAllLines(filePath, lines, System.Text.Encoding.UTF8);
		}

		public static TodoList LoadTodos(string filePath)
		{
			if (!File.Exists(filePath))
				return null;

			try
			{
				var lines = File.ReadAllLines(filePath, System.Text.Encoding.UTF8)
					.Skip(1)
					.Where(l => !string.IsNullOrWhiteSpace(l))
					.ToList();

				var todoList = new TodoList();

				foreach (var line in lines)
				{
					string[] parts = line.Split(';', StringSplitOptions.RemoveEmptyEntries);
					if (parts.Length < 4) continue;

					string text = parts[1].Trim('"')
						.Replace("\\n", "\n")
						.Replace("\\r", "\r")
						.Replace("\\;", ";");

					var item = new TodoItem(text);
					item.IsDone = bool.Parse(parts[2]);
					item.LastUpdate = DateTime.Parse(parts[3]);

					todoList.Add(item);
				}

				return todoList;
			}
			catch
			{
				return null;
			}
		}
	}
}
