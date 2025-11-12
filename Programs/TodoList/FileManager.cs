using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Todolist
{
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

		public static void EnsureFilesExist(string profilePath, string todoPath)
		{
			if (!File.Exists(profilePath))
				File.WriteAllText(profilePath, "");

			if (!File.Exists(todoPath))
				File.WriteAllText(todoPath, "Index;Text;IsDone;LastUpdate\n");
		}

		public static void SaveProfile(Profile profile, string filePath)
		{
			try
			{
				using (StreamWriter writer = new StreamWriter(filePath))
				{
					writer.WriteLine(profile.FirstName ?? "");
					writer.WriteLine(profile.LastName ?? "");
					writer.WriteLine(profile.BirthYear);
				}
				Console.WriteLine($"Профиль сохранен: {filePath}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при сохранении профиля: {ex.Message}");
			}
		}

		public static Profile LoadProfile(string filePath)
		{
			var profile = new Profile();

			if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
			{
				Console.WriteLine("Файл профиля не найден или пуст. Создание нового профиля.");
				return profile;
			}

			try
			{
				using (StreamReader reader = new StreamReader(filePath))
				{
					profile.FirstName = reader.ReadLine() ?? "";
					profile.LastName = reader.ReadLine() ?? "";

					if (int.TryParse(reader.ReadLine(), out int birthYear))
					{
						profile.BirthYear = birthYear;
					}
				}
				Console.WriteLine($"Профиль загружен: {filePath}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при загрузке профиля: {ex.Message}");
			}

			return profile;
		}

		public static void SaveTodos(TodoList todos, string filePath)
		{
			try
			{
				using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
				{
					// Записываем заголовок CSV
					writer.WriteLine("Index;Text;IsDone;LastUpdate");

					for (int i = 0; i < todos.Count; i++)
					{
						TodoItem item = todos.GetItem(i);

						// Экранируем текст для CSV
						string escapedText = EscapeCsv(item.Text);

						string line = $"{i + 1};{escapedText};{item.IsDone.ToString().ToLower()};{item.LastUpdate:yyyy-MM-ddTHH:mm:ss}";
						writer.WriteLine(line);
					}
				}
				Console.WriteLine($"Задачи сохранены: {filePath} (всего {todos.Count} задач)");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при сохранении задач: {ex.Message}");
			}
		}

		public static TodoList LoadTodos(string filePath)
		{
			var todoList = new TodoList();

			if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
			{
				Console.WriteLine("Файл задач не найден или пуст. Будет создан новый список задач.");
				return todoList;
			}

			try
			{
				using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
				{
					// Пропускаем заголовок
					string header = reader.ReadLine();

					if (header == null || !header.StartsWith("Index;"))
					{
						Console.WriteLine("Неверный формат файла задач.");
						return todoList;
					}

					int lineNumber = 1;
					while (!reader.EndOfStream)
					{
						string line = reader.ReadLine();
						lineNumber++;

						if (string.IsNullOrWhiteSpace(line))
							continue;

						try
						{
							string[] parts = ParseCsvLine(line);

							if (parts.Length >= 4)
							{
								// Парсим индекс
								if (!int.TryParse(parts[0], out int index))
								{
									Console.WriteLine($"Ошибка в строке {lineNumber}: неверный формат индекса");
									continue;
								}

								// Обрабатываем текст
								string text = UnescapeCsv(parts[1]);

								// Парсим статус выполнения
								if (!bool.TryParse(parts[2], out bool isDone))
								{
									Console.WriteLine($"Ошибка в строке {lineNumber}: неверный формат статуса");
									continue;
								}

								// Парсим дату последнего обновления
								if (!DateTime.TryParseExact(parts[3], "yyyy-MM-ddTHH:mm:ss",
									CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime lastUpdate))
								{
									lastUpdate = DateTime.Now;
								}

								// Создаем задачу
								TodoItem item = new TodoItem(text)
								{
									IsDone = isDone,
									LastUpdate = lastUpdate
								};

								todoList.Add(item);
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine($"Ошибка при обработке строки {lineNumber}: {ex.Message}");
						}
					}
				}
				Console.WriteLine($"Задачи загружены: {filePath} (всего {todoList.Count} задач)");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при загрузке задач: {ex.Message}");
			}

			return todoList;
		}

		// Экранирование текста для CSV
		private static string EscapeCsv(string text)
		{
			// Если текст содержит переносы строк, точку с запятой или кавычки - обрамляем в кавычки
			if (text.Contains("\"") || text.Contains(";") || text.Contains("\n") || text.Contains("\r"))
			{
				// Экранируем кавычки и обрамляем весь текст в кавычки
				return "\"" + text.Replace("\"", "\"\"") + "\"";
			}
			return text;
		}

		// Восстановление текста из CSV
		private static string UnescapeCsv(string text)
		{
			if (text.StartsWith("\"") && text.EndsWith("\""))
			{
				// Убираем обрамляющие кавычки и восстанавливаем экранированные кавычки
				text = text.Substring(1, text.Length - 2).Replace("\"\"", "\"");
			}
			return text;
		}

		private static string[] ParseCsvLine(string line)
		{
			var parts = new List<string>();
			int currentPosition = 0;
			bool inQuotes = false;
			StringBuilder currentField = new StringBuilder();

			while (currentPosition < line.Length)
			{
				char currentChar = line[currentPosition];

				if (currentChar == '"')
				{
					if (inQuotes && currentPosition + 1 < line.Length && line[currentPosition + 1] == '"')
					{
						// Экранированная кавычка
						currentField.Append('"');
						currentPosition += 2;
						continue;
					}
					else
					{
						// Начало или конец кавычек
						inQuotes = !inQuotes;
					}
				}
				else if (currentChar == ';' && !inQuotes)
				{
					// Конец поля
					parts.Add(currentField.ToString());
					currentField.Clear();
				}
				else
				{
					currentField.Append(currentChar);
				}

				currentPosition++;
			}

			// Добавляем последнее поле
			parts.Add(currentField.ToString());

			return parts.ToArray();
		}
	}
}