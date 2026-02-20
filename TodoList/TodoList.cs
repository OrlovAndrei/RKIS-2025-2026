using System;
using System.Collections.Generic;

namespace TodoList
{
	public class TodoList
	{
		private List<TodoItem> _tasks;

		public TodoList()
		{
			_tasks = new List<TodoItem>();
		}

		public TodoList(List<TodoItem> tasks)
		{
			_tasks = tasks ?? new List<TodoItem>();
		}

		public event Action<TodoItem> TaskAdded;
		public event Action<TodoItem> TaskDeleted;
		public event Action<TodoItem> TaskUpdated;
		public event Action<TodoItem> StatusChanged;

		public void AddTask(string line, string[] flags)
		{
			string text;

			if (flags != null && flags.Contains("multiline"))
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

				if (string.IsNullOrWhiteSpace(text))
        		{
            		Console.WriteLine("Ошибка: текст задачи не может быть пустым");
            		return;
				}
			}

			var newTask = new TodoItem(text);
			_tasks.Add(newTask);
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
			if (idx < 0 || idx >= _tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			_tasks[idx].MarkDone();
			Console.WriteLine("Задача выполнена");

			StatusChanged?.Invoke(_tasks[idx]);
		}

		public void DeleteTask(string line)
		{
			if (!int.TryParse(line, out int idx))
			{
				Console.WriteLine("Ошибка: укажите номер задачи");
				return;
			}

			idx--;
			if (idx < 0 || idx >= _tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			var deletedTask = _tasks[idx];
			_tasks.RemoveAt(idx);
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
			if (idx < 0 || idx >= _tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			_tasks[idx].UpdateText(parts[1].Trim('"', '\''));
			Console.WriteLine("Задача обновлена");

			TaskUpdated?.Invoke(_tasks[idx]);
		}

		public void ViewTasks(string[] flags)
		{
			if (_tasks.Count == 0)
			{
				Console.WriteLine("Список задач пуст");
				return;
			}

			bool showIndex = flags != null && (flags.Contains("index") || flags.Contains("all"));
			bool showStatus = flags != null && (flags.Contains("status") || flags.Contains("all"));
			bool showDate = flags != null && (flags.Contains("update-date") || flags.Contains("all"));

			Console.WriteLine("---------------------------------------------------------------");

			string header = "";
			if (showIndex) header += "# ";
			if (showStatus) header += "Status ".PadRight(12);
			if (showDate) header += "Last Updated ".PadRight(20);
			header += "Task Text";

			Console.WriteLine(header);
			Console.WriteLine("---------------------------------------------------------------");

			for (int i = 0; i < _tasks.Count; i++)
			{
				var item = _tasks[i];
				string text = item.GetShortInfo();
				string line = "";

				if (showIndex) line += $"{i + 1}. ";
				if (showStatus) line += $"[{item.Status}] ".PadRight(14);
				if (showDate) line += $"({item.LastUpdate:yyyy-MM-dd HH:mm}) ".PadRight(22);

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
			if (idx < 0 || idx >= _tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			var item = _tasks[idx];
			Console.WriteLine($"\nЗадача {idx + 1}:");
			Console.WriteLine(item.Text);
			Console.WriteLine($"Статус: {item.Status}");
			Console.WriteLine($"Дата изменения: {item.LastUpdate}\n");
		}

		public List<TodoItem> GetAllTasks() => _tasks;

		public TodoItem this[int index]
		{
			get
			{
				if (index < 0 || index >= _tasks.Count)
					throw new ArgumentOutOfRangeException(nameof(index));
				return _tasks[index];
			}
		}

		public System.Collections.IEnumerator GetEnumerator()
		{
			foreach (var task in _tasks)
			{
				yield return task;
			}
		}

		public void SetStatus(int index, TodoStatus status)
		{
			if (index < 0 || index >= _tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}
			_tasks[index].SetStatus(status);
			Console.WriteLine($"Статус задачи {index + 1} изменен на: {status}");

			StatusChanged?.Invoke(_tasks[index]);
		}
	}
}