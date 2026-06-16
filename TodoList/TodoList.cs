namespace TodoList;

public class TodoList
{
	TodoItem[] items = new TodoItem[2];
	private int count;

	public void Add(TodoItem item)
	{
		if (count == items.Length)
			IncreaseArray();

		items[count] = item;
		count++;
		Console.WriteLine($"Задача добавлена: {item.Text}");
	}

	public void Delete(int index)
	{
		for (var i = index; i < count - 1; i++)
		{
			items[i] = items[i + 1];
		}

		count--;
		Console.WriteLine($"Задача {index + 1} удалена.");
	}

	public void MarkDone(int index)
	{
		items[index].MarkDone();
		Console.WriteLine($"Задача {index + 1} выполнена.");
	}

	public void Update(int index, string newText)
	{
		items[index].UpdateText(newText);
		Console.WriteLine($"Задача {index} обновлена.");
	}

	public void Read(int index)
	{
		Console.WriteLine($"Полная информация о задаче {index}");
		Console.WriteLine(items[index].GetFullInfo());
	}

	public void View(bool hasIndex, bool hasStatus, bool hasDate, bool hasAll)
	{
		var header = "|";
		if (hasIndex || hasAll) header += $"{" Индекс",-8} |";
		header += $"{" Задача",-36} |";
		if (hasStatus || hasAll) header += $"{" Статус",-18} |";
		if (hasDate || hasAll) header += $"{" Изменено",-20} |";

		Console.WriteLine(header);
		Console.WriteLine(new string('-', header.Length));

		for (var i = 0; i < count; i++)
		{
			var title = items[i].Text.Replace("\n", " ");
			if (title.Length > 27) title = title.Substring(0, 27) + "...";

			var rows = "|";
			if (hasIndex || hasAll) rows += $" {(i + 1).ToString(),-8}|";
			rows += $" {title,-36}|";
			if (hasStatus || hasAll) rows += $" {(items[i].IsDone ? "Выполнено" : "Не выполнено"),-18}|";
			if (hasDate || hasAll) rows += $" {items[i].LastUpdate,-20:yyyy-MM-dd HH:mm}|";

			Console.WriteLine(rows);
		}
	}

	private void IncreaseArray()
	{
		var newSize = items.Length * 2;
		Array.Resize(ref items, newSize);
	}
}