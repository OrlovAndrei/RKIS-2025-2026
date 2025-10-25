using System;

namespace Todolist
{
	public class TodoList
	{
		private TodoItem[] items;
		private int count;

		public TodoList(int initialCapacity = 2)
		{
			items = new TodoItem[initialCapacity];
			count = 0;
		}
		public void Add(TodoItem item)
		{
			if (count >= items.Length)
			{
				IncreaseArray(item);
			}
			items[count] = item;
			count++;
		}

		public void Delete(int index)
		{
			if (index < 0 || index >= count)
				throw new ArgumentOutOfRangeException(nameof(index), "Неверный индекс");

			for (int i = index; i < count - 1; i++)
			{
				items[i] = items[i + 1];
			}
			items[count - 1] = null;
			count--;
		}

		public void View(bool showIndex, bool showStatus, bool showDate)
		{
			if (count == 0)
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

			for (int i = 0; i < count; i++)
			{
				string line = "";

				if (showIndex) line += $"{i + 1}".PadRight(8);

				if (showStatus)
				{
					string status = items[i].IsDone ? "Сделано" : "Не сделано";
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
			if (index < 0 || index >= count)
				throw new ArgumentOutOfRangeException(nameof(index), "Неверный индекс");

			return items[index];
		}
		public int Count => count;

		private void IncreaseArray(TodoItem item)
		{
			int newSize = items.Length * 2;
			TodoItem[] newArray = new TodoItem[newSize];

			Array.Copy(items, newArray, items.Length);
			items = newArray;

			Console.WriteLine($"Массив расширен до {newSize} элементов");
		}
	}
}