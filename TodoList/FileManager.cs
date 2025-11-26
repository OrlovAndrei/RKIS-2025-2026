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
    		var taskLines = new List<string>();
    		for (int i = 0; i < todos.Count; i++)
    		{
        		var item = todos[i];
        		if (!item.IsDone)
        		{
            		string text = item.Text.Replace("\n", " ").Replace("\r", " ").Trim();
            		string creationDate = item.CreationDate.ToString("yyyy-MM-ddTHH:mm:ss");
            		string status = item.Status.ToString();
            		string taskLine = $"{i};\"{text}\";{item.IsDone.ToString().ToLower()};{creationDate};{status}";
            		taskLines.Add(taskLine);
        		}
    		}
    		try
    		{
        		string allTasksInOneLine = string.Join(" ", taskLines);
        		File.WriteAllText(filePath, allTasksInOneLine, System.Text.Encoding.UTF8);
    		}
    		catch (IOException ex)
    		{
        		Console.WriteLine($"[ОШИБКА] Не удалось сохранить {filePath}: {ex.Message}");
    		}
		}
		public static TodoList LoadTodos(string todoFilePath, string doneFilePath)
		{
			var todoList = new TodoList();
			int loadedTasksCount = 0;
			int loadedDoneTasksCount = 0;

			if (File.Exists(todoFilePath))
			{
				try
				{
					string allTasksLine = File.ReadAllText(todoFilePath, System.Text.Encoding.UTF8).Trim();
					if (!string.IsNullOrEmpty(allTasksLine))
					{
						var tasks = SplitTasksLine(allTasksLine);
						foreach (string taskLine in tasks)
						{
							if (!string.IsNullOrWhiteSpace(taskLine))
							{
								var task = ParseTaskLine(taskLine);
								if (task != null)
								{
									task.IsDone = true;
									todoList.Add(task);
									loadedTasksCount++;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"[ОШИБКА] Не удалось загрузить задачи из {todoFilePath}: {ex.Message}");
				}
			}

			if (File.Exists(doneFilePath))
			{
				try
				{
					string allDoneTasksLine = File.ReadAllText(doneFilePath, System.Text.Encoding.UTF8).Trim();
					if (!string.IsNullOrEmpty(allDoneTasksLine))
					{
						var doneTasks = SplitTasksLine(allDoneTasksLine);
						foreach (string taskLine in doneTasks)
						{
							if (!string.IsNullOrWhiteSpace(taskLine))
							{
								var task = ParseTaskLine(taskLine);
								if (task != null)
								{
									task.IsDone = true;
									todoList.Add(task);
									loadedDoneTasksCount++;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"[ОШИБКА] Не удалось загрузить выполненные задачи из {doneFilePath}: {ex.Message}");
				}
			}
			Console.WriteLine($"Загружено задач: {loadedTasksCount} активных, {loadedDoneTasksCount} выполненных");

			return todoList;
		}
		private static List<string> SplitTasksLine(string line)
		{
			var tasks = new List<string>();
			bool inQuotes = false;
			string currentTask = "";
			foreach (char c in line)
			{
				if (c == '"')
				{
					inQuotes = !inQuotes;
					currentTask += c;
				}
				else if (c == ' ' && !inQuotes)
				{
					if (!string.IsNullOrEmpty(currentTask))
					{
						tasks.Add(currentTask);
						currentTask = "";
					}
				}
				else
				{
					currentTask += c;
				}
			}
			if (!string.IsNullOrEmpty(currentTask))
			{
				tasks.Add(currentTask);
			}
			return tasks;
		}
		private static TodoItem ParseTaskLine(string line)
		{
    		try
    		{
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
                		parts.Add(currentPart.Trim());
                		currentPart = "";
            		}
            		else
            		{
                		currentPart += c;
            		}
        		}
        		if (!string.IsNullOrEmpty(currentPart))
        		{
            		parts.Add(currentPart.Trim());
        		}
        		if (parts.Count >= 4)
        		{
            		string text = parts[1];
            		bool isDone = bool.Parse(parts[2]);
            		DateTime creationDate = DateTime.Parse(parts[3]);
            		TodoStatus status = TodoStatus.NotStarted;
            		if (parts.Count >= 5 && Enum.TryParse<TodoStatus>(parts[4], out var parsedStatus))
            		{
                		status = parsedStatus;
            		}
            		var item = new TodoItem(text, isDone, creationDate, status);
            		return item;
        		}
    		}
			catch (Exception ex)
    		{
        		Console.WriteLine($"[ОШИБКА ПАРСИНГА] Не удалось разобрать строку задачи: {line}");
        		Console.WriteLine($"[ДЕТАЛИ ОШИБКИ] {ex.Message}");
    		}
    		return null;
		}
		public static void PrintAllTasksInOneLine(TodoList todos)
		{
			var taskLines = new List<string>();
			for (int i = 0; i < todos.Count; i++)
			{
				var item = todos[i];
				taskLines.Add(item.GetFormattedInfo(i));
			}
			Console.WriteLine(string.Join(" ", taskLines));
		}

		public static void PrintPendingTasksInOneLine(TodoList todos)
		{
			var taskLines = new List<string>();
			for (int i = 0; i < todos.Count; i++)
			{
				var item = todos[i];
				if (!item.IsDone)
				{
					taskLines.Add(item.GetFormattedInfo(i));
				}
			}
			Console.WriteLine(string.Join(" ", taskLines));
		}

		public static void PrintCompletedTasksInOneLine(TodoList todos)
		{
			var taskLines = new List<string>();
			for (int i = 0; i < todos.Count; i++)
			{
				var item = todos[i];
				if (item.IsDone)
				{
					taskLines.Add(item.GetFormattedInfo(i));
				}
			}
			Console.WriteLine(string.Join(" ", taskLines));
		}
	}
}