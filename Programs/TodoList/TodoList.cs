using System;
using System.Collections;
using System.Collections.Generic;

namespace Todolist
{
	public class TodoList : IEnumerable<TodoItem>
	{
		private List<TodoItem> items;

		public TodoList(int initialCapacity = 2)
		{
			items = new List<TodoItem> (initialCapacity);
		}
		public void Add(TodoItem item)
		{
			items.Add(item);
		}

		public void Delete(int index)
		{
			if (index < 0 || index >= items.Count)
				throw new ArgumentOutOfRangeException(nameof(index), "Неверный индекс");

			items.RemoveAt(index);
		}

		public void View(bool showIndex, bool showStatus, bool showDate)
		{
			if (items.Count == 0)
			{
				Console.WriteLine("Список пуст");
				return;
			}
			string header = "";
			if (showIndex) header += "Индекс".PadRight(8);
			if (showStatus) header += "Статус".PadRight(12);
			if (showDate) header += "Дата изменения".PadRight(20);
			header += "Задача";

			Console.WriteLine(header);
			Console.WriteLine(new string('-', header.Length));

			for (int i = 0; i < items.Count; i++)
			{
				string line = "";

				if (showIndex) line += $"{i + 1}".PadRight(8);

				if (showStatus)
				{
					string status = items[i].Status switch
					{
						TodoStatus.NotStarted => "Не начато",
						TodoStatus.InProgress => "В процессе",
						TodoStatus.Completed => "Завершено",
						TodoStatus.Postponed => "Отложено",
						TodoStatus.Failed => "Провалено",
						_ => items[i].Status.ToString()
					};
					line += status.PadRight(12);
				}

				if (showDate)
				{
					string date = items[i].LastUpdate.ToString("dd.MM.yyyy HH:mm");
					line += date.PadRight(20);
				}

				string taskText = items[i].Text?.Replace("\n", " ") ?? "";
				if (taskText.Length > 30)
					taskText = taskText.Substring(0, 27) + "...";
				line += taskText;

				Console.WriteLine(line);
			}
		}

		public TodoItem GetItem(int index)
		{
			if (index < 0 || index >= items.Count)
				throw new ArgumentOutOfRangeException(nameof(index), "Неверный индекс");
			return this[index];
		}
		// Метод для установки статуса
		public void SetStatus(int index, TodoStatus status)
		{
			if (index < 0 || index >= items.Count)
				throw new ArgumentOutOfRangeException(nameof(index), "Неверный индекс");

			items[index].Status = status;
			items[index].LastUpdate = DateTime.Now;
		}
		public int Count => items.Count;

		public TodoItem this[int index]
		{
			get
			{
				if (index < 0 || index >= items.Count)
					throw new ArgumentOutOfRangeException(nameof(index), "Неверный индекс");
				return items[index];
			}
			set
			{
				if (index < 0 || index >= items.Count)
					throw new ArgumentOutOfRangeException(nameof(index), "Неверный индекс");
				items[index] = value;
			}
		}
		public IEnumerator<TodoItem> GetEnumerator()
		{
			for (int i = 0; i < items.Count; i++)
			{
				yield return items[i];
			}
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}