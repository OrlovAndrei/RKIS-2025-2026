using System;
using System.Collections.Generic;
using System.Collections;

namespace TodoList
{
	public class TodoList : IEnumerable<TodoItem>
	{
		private List<TodoItem> _tasks = new List<TodoItem>();
		private const int ShortTextLength = 30;

		public void Add(TodoItem item) => _tasks.Add(item);

		public TodoItem this[int index]
		{
			get => (index >= 1 && index <= _tasks.Count) ? _tasks[index - 1] : null;
		}

		public void SetStatus(int index, TodoStatus status)
		{
			var item = this[index];
			if (item != null)
			{
				item.ChangeStatus(status);
			}
		}

		public void Delete(int index)
		{
			if (index >= 1 && index <= _tasks.Count)
			{
				_tasks.RemoveAt(index - 1);
			}
		}

		public IEnumerator<TodoItem> GetEnumerator() => _tasks.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void View(bool showIndex, bool showStatus, bool showDate)
		{
			if (_tasks.Count == 0) return;

			for (int i = 0; i < _tasks.Count; i++)
			{
				var item = _tasks[i];
				string output = "";
				if (showIndex) output += $"{(i + 1),-5} ";

				string taskText = item.Text.Length > ShortTextLength ? item.Text.Substring(0, 27) + "..." : item.Text;
				output += $"{taskText,-30} ";

				if (showStatus) output += $"{item.GetStatusString(),-15} ";
				if (showDate) output += $"{item.LastUpdate:yyyy-MM-dd HH:mm:ss}";

				Console.WriteLine(output.TrimEnd());
			}
		}
	}
}