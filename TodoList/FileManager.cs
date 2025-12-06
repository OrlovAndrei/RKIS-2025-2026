using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace TodoApp.Commands
{
	public static class FileManager
	{
		public static void EnsureDataDirectory(string dirPath)
		{
			if (!Directory.Exists(dirPath))
				Directory.CreateDirectory(dirPath);
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
					string login = "default_login"; 
					string password = "default_password";
					string firstName = parts[0];
					string lastName = parts[1];
					int birthYear = int.Parse(parts[2]);

					return new Profile(login, password, firstName, lastName, birthYear);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[ОШИБКА] Не удалось загрузить профиль: {ex.Message}");
			}
			return null;
		}
		public static void SaveTodos(TodoList todos, string filePath)
		{
			var taskLines = new List<string>();
			for (int i = 0; i < todos.Count; i++)
			{
				var item = todos[i];
				string text = item.Text.Replace("\n", " ").Replace("\r", " ").Trim();
				string creationDate = item.CreationDate.ToString("yyyy-MM-ddTHH:mm:ss");
				string status = item.Status.ToString();
				bool isDone = item.IsDone;

				string taskLine = $"{i};\"{text}\";{isDone.ToString().ToLower()};{creationDate};{status}";
				taskLines.Add(taskLine);
			}

			try
			{
				File.WriteAllLines(filePath, taskLines, Encoding.UTF8);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[ОШИБКА] Не удалось сохранить {filePath}: {ex.Message}");
			}
		}
		private static void PrintTasks(TodoList todos, bool? isDoneFilter = null)
		{
			var taskLines = todos
				.Where(t => isDoneFilter == null || t.IsDone == isDoneFilter)
				.Select((t, index) => t.GetFormattedInfo(index))
				.ToList();
			Console.WriteLine(string.Join("\n", taskLines));
		}


		public static void PrintAllTasksInOneLine(TodoList todos) => PrintTasks(todos);
		public static void PrintPendingTasksInOneLine(TodoList todos) => PrintTasks(todos, false);
		public static void PrintCompletedTasksInOneLine(TodoList todos) => PrintTasks(todos, true);
		public static void SaveAllProfiles(List<Profile> profiles, string filePath)
		{
			try
			{
				var lines = profiles.Select(p => $"{p.Id};{p.Login};{p.Password};{p.FirstName};{p.LastName};{p.BirthYear}");
				File.WriteAllLines(filePath, lines, System.Text.Encoding.UTF8);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[ОШИБКА] Не удалось сохранить профили: {ex.Message}");
			}
		}
		public static List<Profile> LoadAllProfiles(string filePath)
		{
			var profiles = new List<Profile>();
			if (!File.Exists(filePath)) return profiles;

			try
			{
				var lines = File.ReadAllLines(filePath, System.Text.Encoding.UTF8);
				foreach (var line in lines)
				{
					if (string.IsNullOrWhiteSpace(line)) continue;
					var parts = line.Split(';');
					if (parts.Length >= 6)
					{
						var profile = new Profile
						{
							Id = Guid.Parse(parts[0]),
							Login = parts[1],
							Password = parts[2],
							FirstName = parts[3],
							LastName = parts[4],
							BirthYear = int.Parse(parts[5])
						};
						profiles.Add(profile);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[ОШИБКА] Не удалось загрузить профили: {ex.Message}");
			}
			return profiles;
		}
		public static void SaveTodosForUser(TodoList todos, string filePath)
		{
			var taskLines = new List<string>();
			for (int i = 0; i < todos.Count; i++)
			{
				var item = todos[i];
				string text = item.Text.Replace("\n", " ").Replace("\r", " ").Trim();
				string creationDate = item.CreationDate.ToString("yyyy-MM-ddTHH:mm:ss");
				string status = item.Status.ToString();
				string taskLine = $"{i};\"{text}\";{item.IsDone.ToString().ToLower()};{creationDate};{status}";
				taskLines.Add(taskLine);
			}

			try
			{
				File.WriteAllLines(filePath, taskLines, Encoding.UTF8);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[ОШИБКА] Не удалось сохранить {filePath}: {ex.Message}");
			}
		}
		public static TodoList LoadTodosForUser(string filePath)
		{
			var todoList = new TodoList();
			if (!File.Exists(filePath))
				return todoList;

			try
			{
				var lines = File.ReadAllLines(filePath, Encoding.UTF8);
				foreach (string line in lines)
				{
					if (string.IsNullOrWhiteSpace(line)) continue;
					var task = ParseTaskLine(line);
					if (task != null)
						todoList.Add(task);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[ОШИБКА] Не удалось загрузить задачи из {filePath}: {ex.Message}");
			}

			return todoList;
		}
		private static TodoItem ParseTaskLine(string line)
		{
			try
			{
				var parts = line.Split(';');
				if (parts.Length < 3)
				{
					Console.WriteLine($"[ОШИБКА ПАРСИНГА] Недостаточно полей в строке: {line}");
					return null;
				}
				string text = parts[1].Trim('"');
				bool isDone;
				if (!bool.TryParse(parts[2], out isDone))
				{
					Console.WriteLine($"[ОШИБКА ПАРСИНГА] Некорректное значение IsDone в строке: {line}");
					return null;
				}
				DateTime creationDate;
				if (!DateTime.TryParseExact(parts[3], "yyyy-MM-ddTHH:mm:ss",
					CultureInfo.InvariantCulture, DateTimeStyles.None, out creationDate))

				{
					Console.WriteLine($"[ОШИБКА ПАРСИНГА] Некорректная дата в строке: {line}");
					return null;
				}
				TodoStatus status = Enum.TryParse<TodoStatus>(parts[4], out var parsedStatus)
					? parsedStatus
					: TodoStatus.NotStarted;

				return new TodoItem(text, isDone, creationDate, status);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[ОШИБКА ПАРСИНГА] Не удалось разобрать строку задачи: {line}. Ошибка: {ex.Message}");
				return null;
			}
		}

	}
}