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
			var taskTexts = new List<string>();
			for (int i = 0; i < todos._count; i++)
			{
				var item = todos[i];
				if (!item.IsDone)
				{
					string text = item.Text.Replace("\n", " ").Replace("\r", " ").Trim();
					if (text.Contains(","))
					{
						text = $"\"{text}\"";
					}
					taskTexts.Add(text);
					Console.WriteLine($"[TODO] Добавляем задачу: {text}");
				}
			}
			string allTasksInOneLine = string.Join(", ", taskTexts);
			Console.WriteLine($"[SAVE] Записываем {taskTexts.Count} задач в одну строку в {filePath}");
			Console.WriteLine($"[CONTENT] {allTasksInOneLine}");
			try
			{
				File.WriteAllText(filePath, allTasksInOneLine, System.Text.Encoding.UTF8);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[ОШИБКА] Не удалось сохранить {filePath}: {ex.Message}");
			}
		}
		public static void SaveDoneTodos(TodoList todos, string doneFilePath)
		{
			Console.WriteLine($"[SAVE] Всего задач в списке (завершённые): {todos._count}");
			var doneTaskTexts = new List<string>();
			for (int i = 0; i < todos._count; i++)
			{
				var item = todos[i];
				if (item.IsDone)
				{
					string text = item.Text.Replace("\n", " ").Replace("\r", " ").Trim();
					if (text.Contains(","))
					{
						text = $"\"{text}\"";
					}
					doneTaskTexts.Add($"✓ {text}");
					Console.WriteLine($"[DONE] Добавляем выполненную задачу: {text}");
				}
			}
			string allDoneTasksInOneLine = string.Join(", ", doneTaskTexts);
			Console.WriteLine($"[SAVE] Записываем {doneTaskTexts.Count} выполненных задач в одну строку в {doneFilePath}");
			try
			{
				File.WriteAllText(doneFilePath, allDoneTasksInOneLine, System.Text.Encoding.UTF8);
			}
			catch (IOException ex)
			{
				Console.WriteLine($"[ОШИБКА] Не удалось сохранить {doneFilePath}: {ex.Message}");
			}
		}
		public static TodoList LoadTodos(string todoFilePath, string doneFilePath)
		{
			var todoList = new TodoList();
			if (File.Exists(todoFilePath))
			{
				try
				{
					string allTasksLine = File.ReadAllText(todoFilePath, System.Text.Encoding.UTF8).Trim();
					Console.WriteLine($"[LOAD] Загружаем строку с задачами: {allTasksLine}");
					if (!string.IsNullOrEmpty(allTasksLine))
					{
						var tasks = ParseCsvLine(allTasksLine);
						foreach (string taskText in tasks)
						{
							if (!string.IsNullOrWhiteSpace(taskText))
							{
								string cleanText = taskText.Trim();
								if (cleanText.StartsWith("(A)"))
									cleanText = cleanText.Substring(3).Trim();
								
								var item = new TodoItem(cleanText);
								item.LastUpdate = DateTime.Now;
								todoList.Add(item);
								Console.WriteLine($"[LOAD] Добавлена задача: {cleanText}");
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
					Console.WriteLine($"[LOAD] Загружаем строку с выполненными задачами: {allDoneTasksLine}");
					if (!string.IsNullOrEmpty(allDoneTasksLine))
					{
						var doneTasks = ParseCsvLine(allDoneTasksLine);
						foreach (string taskText in doneTasks)
						{
							if (!string.IsNullOrWhiteSpace(taskText))
							{
								string cleanText = taskText.Trim();
								if (cleanText.StartsWith("✓"))
									cleanText = cleanText.Substring(1).Trim();
								var item = new TodoItem(cleanText);
								item.IsDone = true;
								item.LastUpdate = DateTime.Now;
								todoList.Add(item);
								Console.WriteLine($"[LOAD] Добавлена выполненная задача: {cleanText}");
							}
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"[ОШИБКА] Не удалось загрузить выполненные задачи из {doneFilePath}: {ex.Message}");
				}
			}
			return todoList;
		}
		private static List<string> ParseCsvLine(string line)
		{
			var result = new List<string>();
			bool inQuotes = false;
			string currentField = "";
			foreach (char c in line)
			{
				if (c == '"')
				{
					inQuotes = !inQuotes;
				}
				else if (c == ',' && !inQuotes)
				{
					result.Add(currentField.Trim());
					currentField = "";
				}
				else
				{
					currentField += c;
				}
			}
			if (!string.IsNullOrEmpty(currentField))
			{
				result.Add(currentField.Trim());
			}
			return result;
		}
	}
}