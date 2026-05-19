using System;
using System.Collections.Generic;
using System.Collections;

namespace TodoList
{
	public class TodoList : IEnumerable<TodoItem>
	{
		private List<TodoItem> items;
		private int count;

		public TodoList()
		{
			this.items = new List<TodoItem>();
			this.count = 0;
		}

		public TodoList(List<TodoItem> items)
		{
			this.items = items;
			this.count = items.Count;
		}

		public void Add(TodoItem item)
		{
			items.Add(item);
			count++;
		}
		public void Delete(int index)
		{
			if (index < 0 || index >= count)
			{
				Console.WriteLine("Ошибка: неверный индекс задачи");
				return;
			}

			items.RemoveAt(index);
			count--;
			Console.WriteLine($"Задача {index + 1} удалена");
		}
		public void View(bool showIndex = false, bool showDone = false, bool showDate = false)
		{
			if (count == 0)
			{
				Console.WriteLine("Задач нет");
				return;
			}

			Console.WriteLine("Ваши задачи:");
			int indexWidth = 8;
			int statusWidth = 14;
			int dateWidth = 16;
			int taskWidth = 36;

			string header = "|";
			string separator = "|";

			if (showIndex)
			{
				header += " Индекс".PadRight(indexWidth) + " |";
				separator += new string('-', indexWidth + 2) + "|";
			}
			if (showDone)
			{
				header += " Статус".PadRight(statusWidth) + " |";
				separator += new string('-', statusWidth + 2) + "|";
			}
			if (showDate)
			{
				header += " Дата".PadRight(dateWidth) + " |";
				separator += new string('-', dateWidth + 2) + "|";
			}
			header += " Задача".PadRight(taskWidth) + " |";
			separator += new string('-', taskWidth + 2) + "|";
			Console.WriteLine(header);
			Console.WriteLine(separator);

			for (int i = 0; i < count; i++)
			{
				string row = "|";

				if (showIndex)
					row += $" {i + 1}".PadRight(indexWidth) + " |";

				if (showDone)
					row += $" {items[i].GetStatusText()}".PadRight(statusWidth) + " |";

				if (showDate)
					row += $" {items[i].LastUpdate:dd.MM.yyyy HH:mm}".PadRight(dateWidth) + " |";

				string shortTask = items[i].Text.Replace("\n", " ").Replace("\r", "");
				if (shortTask.Length > taskWidth - 2)
					shortTask = shortTask.Substring(0, taskWidth - 3) + "...";
				row += $" {shortTask}".PadRight(taskWidth) + " |";
				Console.WriteLine(row);
			}
		}

		public void Read(int index)
		{
			if (index < 0 || index >= count)
			{
				Console.WriteLine("Ошибка: неверный индекс задачи");
				return;
			}

			Console.WriteLine($"Задача {index + 1}:");
			Console.WriteLine(items[index].GetFullInfo());
		}
		public int Count
		{
			get { return count; }
		}

		public TodoItem GetItem(int index)
		{
			if (index < 0 || index >= count)
			{
				throw new ArgumentException("Неверный индекс задачи");
			}
			return items[index];
		}


		public void AddTodoFromFile(TodoItem todo)
		{
			items.Add(todo);
			count++;
		}

		public void Insert(int index, TodoItem item)
		{
			if (index < 0 || index > count)
			{
				Console.WriteLine("Ошибка: неверный индекс для вставки");
				return;
			}
			items.Insert(index, item);
			count++;
		}

		public IEnumerator<TodoItem> GetEnumerator()
		{
			for (int i = 0; i < count; i++)
			{
				yield return items[i];
			}
		}
		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		public TodoItem this[int index] => items[index];

		public void SetStatus(int index, TodoStatus status)
		{
			if (index < 0 || index >= count)
			{
				Console.WriteLine("Ошибка: неверный индекс задачи");
				return;
			}

			items[index].SetStatus(status);
			Console.WriteLine($"Статус задачи {index + 1} изменен на: {items[index].GetStatusText()}");
		}
	}
}