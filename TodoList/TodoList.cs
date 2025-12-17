using System;
using System.Collections.Generic;
using System.Collections;

namespace TodoList
{
	public class TodoList : IEnumerable<TodoItem>
	{
		private List<TodoItem> _tasks;
		private const int ShortTextLength = 30;

		public TodoList()
		{
			_tasks = new List<TodoItem>();
		}

		public void Add(TodoItem item)
		{
			if (item != null)
			{
				_tasks.Add(item);
			}
		}

		public TodoItem this[int index]
		{
			get
			{
				if (index < 1 || index > _tasks.Count)
				{
					return null;
				}
				return _tasks[index - 1];
			}
		}

		public void Delete(int index)
		{
			if (index < 1 || index > _tasks.Count)
			{
				Console.WriteLine($"Ошибка: Неверный индекс. Допустимые значения от 1 до {_tasks.Count}.");
				return;
			}

			_tasks.RemoveAt(index - 1);
			Console.WriteLine($"Задача {index} удалена.");
		}

		public IEnumerator<TodoItem> GetEnumerator()
		{
			foreach (var task in _tasks)
			{
				yield return task;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void View(bool showIndex, bool showStatus, bool showDate)
		{
			if (_tasks.Count == 0)
			{
				Console.WriteLine("Список задач пуст.");
				return;
			}

			int indexWidth = Math.Max(5, _tasks.Count.ToString().Length);
			int taskWidth = ShortTextLength;
			int statusWidth = 10;
			int dateWidth = 19;

			PrintHeader(showIndex, showStatus, showDate, indexWidth, taskWidth, statusWidth, dateWidth);

			for (int i = 0; i < _tasks.Count; i++)
			{
				TodoItem item = _tasks[i];
				string output = "";

				if (showIndex) output += $"{(i + 1),-indexWidth} ";

				string taskText = item.Text ?? string.Empty;
				if (taskText.Length > taskWidth)
				{
					taskText = taskText.Substring(0, taskWidth - 3) + "...";
				}
				output += $"{taskText,-taskWidth} ";

				if (showStatus)
				{
					string statusText = item.GetStatusString();
					output += $"{statusText,-12} ";
				}

				if (showDate)
				{
					output += $"{item.LastUpdate:yyyy-MM-dd HH:mm:ss}";
				}

				Console.WriteLine(output.TrimEnd());
			}
		}

		private void PrintHeader(bool showIndex, bool showStatus, bool showDate, int indexWidth, int taskWidth, int statusWidth, int dateWidth)
		{
			string header = "";
			if (showIndex) header += $"{"Инд",-indexWidth} ";
			header += $"{"Задача",-taskWidth} ";
			if (showStatus) header += $"{"Статус",-statusWidth} ";
			if (showDate) header += $"{"Дата",-dateWidth}";

			Console.WriteLine("Список задач:");
			Console.WriteLine(header.TrimEnd());
			Console.WriteLine(new string('-', header.Length));
		}

		public TodoItem GetItem(int index) => this[index];
	}
}