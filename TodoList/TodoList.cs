using System.Collections;

namespace TodoList;

public class TodoList : IEnumerable<TodoItem>
{
	public List<TodoItem> items = [];
	public IEnumerator<TodoItem> GetEnumerator() => items.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public void Add(TodoItem item)
	{
		items.Add(item);
	}

	public void Delete(int index)
	{
		items.RemoveAt(index);
	}

	public void View(bool showIndex, bool showStatus, bool showDate)
	{

		var header = "";
		if (showIndex) header += "№".PadRight(6);
		if (showStatus) header += "Статус".PadRight(16);
		if (showDate) header += "Дата".PadRight(20);
		header += "Задача";

		Console.WriteLine(header);
		Console.WriteLine(new string('-', header.Length));

		for (var i = 0; i < items.Count; i++)
		{
			var line = "";
			if (showIndex) line += $"{i + 1}".PadRight(6);
			if (showStatus) line += $"{items[i].Status}".PadRight(16);
			if (showDate) line += $"{items[i].LastUpdate:dd.MM.yyyy HH:mm}".PadRight(20);

			var preview = items[i].Text.Length <= 30 ? items[i].Text : items[i].Text.Substring(0, 27) + "...";
			line += preview;

			Console.WriteLine(line);
		}
	}

	public TodoItem GetItem(int index)
	{
		if (index < 0 || index >= items.Count)
			throw new ArgumentOutOfRangeException("Неверный индекс задачи");
		return items[index];
	}
}