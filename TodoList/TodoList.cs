using System;
using TodoList.Commands;

namespace TodoList
{
	public class TodoList
	{
		internal List<TodoItem> tasks = new List<TodoItem>();

		public event Action<TodoItem> TaskAdded;
		public event Action<TodoItem> TaskDeleted;
		public event Action<TodoItem> TaskUpdated;
		public event Action<TodoItem> StatusChanged;

		public void AddTask(string line, string[] flags)
		{
			string text;

			if (flags.Contains("multiline"))
			{
				Console.WriteLine("Многострочный ввод (введите !end для завершения):");
				var lines = new List<string>();
				while (true)
				{
					Console.Write("> ");
					string? input = Console.ReadLine();
					if (input == null || input.Trim() == "!end") break;
					if (!string.IsNullOrWhiteSpace(input))
						lines.Add(input);
				}
				text = string.Join("\n", lines);
			}
			else
			{
				if (string.IsNullOrWhiteSpace(line))
				{
					Console.WriteLine("Ошибка: не введён текст задачи");
					return;
				}
				text = line.Trim('"', '\'');
			}

			var newTask = new TodoItem(text);
			tasks.Add(newTask);
			Console.WriteLine("Задача добавлена!");

			TaskAdded?.Invoke(newTask);
		}

		public void MarkTaskDone(string line)
		{
			if (!int.TryParse(line, out int idx))
			{
				Console.WriteLine("Ошибка: укажите номер задачи");
				return;
			}

			idx--;
			if (idx < 0 || idx >= tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			tasks[idx].MarkDone();
			Console.WriteLine("Задача выполнена");

			StatusChanged?.Invoke(tasks[idx]);
		}

		public void DeleteTask(string line)
		{
			if (!int.TryParse(line, out int idx))
			{
				Console.WriteLine("Ошибка: укажите номер задачи");
				return;
			}

			idx--;
			if (idx < 0 || idx >= tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			var deletedTask = tasks[idx];
			tasks.RemoveAt(idx);
			Console.WriteLine("Задача удалена");

			TaskDeleted?.Invoke(deletedTask);
		}

		public void UpdateTask(string line)
		{
			var parts = line.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length < 2 || !int.TryParse(parts[0], out int idx))
			{
				Console.WriteLine("Ошибка: укажите номер задачи и текст");
				return;
			}

			idx--;
			if (idx < 0 || idx >= tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			tasks[idx].UpdateText(parts[1].Trim('"', '\''));
			Console.WriteLine("Задача обновлена");

			TaskUpdated?.Invoke(tasks[idx]);
		}

		public void ViewTasks(string[] flags)
		{
			if (tasks.Count == 0)
			{
				Console.WriteLine("Список задач пуст");
				return;
			}

			bool showIndex = flags.Contains("index") || flags.Contains("all");
			bool showStatus = flags.Contains("status") || flags.Contains("all");
			bool showDate = flags.Contains("update-date") || flags.Contains("all");

			Console.WriteLine("---------------------------------------------------------------");

			string header = "";
			if (showIndex) header += "# ";
			if (showStatus) header += "Status ";
			if (showDate) header += "Last Updated ";
			header += "Task Text";

			Console.WriteLine(header);
			Console.WriteLine("---------------------------------------------------------------");

			for (int i = 0; i < tasks.Count; i++)
			{
				var item = tasks[i];
				string text = item.GetShortInfo();
				string line = "";

				if (showIndex) line += $"{i + 1}. ";
				if (showStatus) line += $"[{item.Status}] ";
				if (showDate) line += $"({item.LastUpdate:yyyy-MM-dd HH:mm}) ";

				Console.WriteLine(line + text);
			}

			Console.WriteLine("---------------------------------------------------------------");
		}

		public void ReadTask(string line)
		{
			if (!int.TryParse(line, out int idx))
			{
				Console.WriteLine("Ошибка: укажите номер задачи");
				return;
			}

			idx--;
			if (idx < 0 || idx >= tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			var item = tasks[idx];
			Console.WriteLine($"\nЗадача {idx + 1}:");
			Console.WriteLine(item.Text);
			Console.WriteLine($"Статус: {item.Status}");
			Console.WriteLine($"Дата изменения: {item.LastUpdate}\n");
		}

		public List<TodoItem> GetAllTasks() => tasks;

		public TodoItem this[int index]
		{
			get
			{
				if (index < 0 || index >= tasks.Count)
					throw new ArgumentOutOfRangeException(nameof(index));
				return tasks[index];
			}
		}

		public System.Collections.IEnumerator GetEnumerator()
		{
			foreach (var task in tasks)
			{
				yield return task;
			}
		}

		public void SetStatus(int index, TodoStatus status)
		{
			if (index < 0 || index >= tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}
			tasks[index].SetStatus(status);
			Console.WriteLine($"Статус задачи {index + 1} изменен на: {status}");

			StatusChanged?.Invoke(tasks[index]);
		}
	}
}