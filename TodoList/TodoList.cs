namespace TodoList;

public class TodoList
{
	public TodoItem[] items = new TodoItem[2];
	public int taskCount;

	public void Add(TodoItem item)
	{
		if (taskCount == items.Length)
			IncreaseArray();

		items[taskCount] = item;
		taskCount++;
		Console.WriteLine($"Задача добавлена: {item.Text}");
	}

	public void Delete(int idx)
	{
		for (var i = idx - 1; i < taskCount - 1; i++) items[i] = items[i + 1];

		taskCount--;
		Console.WriteLine($"Задача {idx} удалена.");
	}

	public void MarkDone(int idx)
	{
		items[idx - 1].MarkDone();
		Console.WriteLine($"Задача {idx} выполнена.");
	}

	public void Update(int idx, string newText)
	{
		items[idx - 1].UpdateText(newText);
		Console.WriteLine($"Задача {idx} обновлена.");
	}

	public void Read(int idx)
	{
		Console.WriteLine(items[idx - 1].GetFullInfo(idx));
	}

	public void View(bool hasIndex, bool hasStatus, bool hasDate, bool hasAll)
	{
		var header = "|";
		if (hasIndex || hasAll) header += " Индекс".PadRight(8) + " |";
		header += " Задача".PadRight(36) + " |";
		if (hasStatus || hasAll) header += " Статус".PadRight(18) + " |";
		if (hasDate || hasAll) header += " Изменено".PadRight(18) + " |";

		Console.WriteLine(header);
		Console.WriteLine(new string('-', header.Length));

		for (var i = 0; i < taskCount; i++)
		{
			var title = items[i].GetShortInfo();
			var rows = "|";
			if (hasIndex || hasAll) rows += " " + (i + 1).ToString().PadRight(8) + "|";
			rows += " " + title.PadRight(36) + "|";
			if (hasStatus || hasAll) rows += " " + (items[i].IsDone ? "Выполнено" : "Не выполнено").PadRight(18) + "|";
			if (hasDate || hasAll) rows += " " + items[i].LastUpdate.ToString("yyyy-MM-dd HH:mm").PadRight(18) + "|";

			Console.WriteLine(rows);
		}
	}

	private void IncreaseArray()
	{
		var newSize = items.Length * 2;
		Array.Resize(ref items, newSize);
	}
}