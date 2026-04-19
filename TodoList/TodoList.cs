namespace TodoList;
public class TodoList
{
	public List<TodoItem> items = [];

	public void Add(TodoItem item)
	{
		items.Add(item);
			}

	public void Delete(int idx)
	{
		items.RemoveAt(idx);
		Console.WriteLine($"Задача {idx} удалена.");
	}

	public void SetStatus(int idx, TodoStatus status)
	{
		items[idx].SetStatus(status);
		Console.WriteLine($"Задача {items[idx].Text} изменена на {status}");
	}

	public void Update(int idx, string newText)
	{
		items[idx].UpdateText(newText);
		Console.WriteLine("Задача обновлена");
	}

	public void Read(int idx)
	{
		Console.WriteLine(items[idx].GetFullInfo(idx));
	}

	public void View(bool showIndex, bool showStatus, bool showDate)
	{
		int indexWidth = 8;
		int textWidth = 34;
		int statusWidth = 16;
		int dateWidth = 16;

		string headerRow = "Текст задачи".PadRight(textWidth) + "|";
		if (showIndex) headerRow += "Индекс".PadRight(indexWidth) + "|";
		if (showStatus) headerRow += "Статус".PadRight(statusWidth) + "|";
		if (showDate) headerRow += "Дата обновления".PadRight(dateWidth) + "|";

		Console.WriteLine(headerRow);
		Console.WriteLine(new string('-', headerRow.Length));

		for (int i = 0; i < items.Count; i++)
		{
			string text = items[i].Text.Replace("\r", " ").Replace("\n", " ");
			if (text.Length > 30) text = text.Substring(0, 30) + "...";

			string status = items[i].Status.ToString();
			string date = items[i].LastUpdate.ToString("yyyy-MM-dd HH:mm");

			string row = text.PadRight(textWidth) + "|";
			if (showIndex) row += i.ToString().PadRight(indexWidth) + "|";
			if (showStatus) row += status.PadRight(statusWidth) + "|";
			if (showDate) row += date.PadRight(dateWidth) + "|";

			Console.WriteLine(row);
		}
	}

}
