namespace TodoList;
public class TodoList
{
	const int IndexWidth = 6;
	const int textWidth = 36;
	const int statusWidth = 14;
	const int dateWidth = 16;

	public TodoItem[] items = new TodoItem[2];
	public int taskCount = 0;

	public void Add(TodoItem item)
	{
		if (taskCount == items.Length)
			IncreaseArray();

		items[taskCount] = item;
		Console.WriteLine($"Добавлена задача: {taskCount}) {item.Text}");
		taskCount++;
	}

	public void Delete(int idx)
	{
		for (var i = idx; i < taskCount - 1; i++)
		{
			items[i] = items[i + 1];
		}

		taskCount--;
		Console.WriteLine($"Задача {idx} удалена.");
	}

	public void MarkDone(int idx)
	{
		items[idx].MarkDone();
		Console.WriteLine($"Задача {items[idx].Text} отмечена выполненной");
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

	public void View(bool showIndex, bool showStatus, bool showUpdateDate)
	{
		List<string> headers = ["Текст задачи".PadRight(textWidth)];
		if (showIndex) headers.Add("Индекс".PadRight(IndexWidth));
		if (showStatus) headers.Add("Статус".PadRight(statusWidth));
		if (showUpdateDate) headers.Add("Дата обновления".PadRight(dateWidth));

		Console.WriteLine("--" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "--");
		Console.WriteLine("| " + string.Join(" | ", headers) + " |");
		Console.WriteLine("|-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-|");

		for (int i = 0; i < taskCount; i++)
		{
			string text = items[i].Text.Replace("\n", " ");
			if (text.Length > 30) text = text.Substring(0, 30) + "...";

			string status = items[i].IsDone ? "выполнена" : "не выполнена";
			string date = items[i].LastUpdate.ToString("yyyy-MM-dd HH:mm");

			List<string> rows = [text.PadRight(textWidth)];
			if (showIndex) rows.Add((i + 1).ToString().PadRight(IndexWidth));
			if (showStatus) rows.Add(status.PadRight(statusWidth));
			if (showUpdateDate) rows.Add(date.PadRight(dateWidth));

			Console.WriteLine("| " + string.Join(" | ", rows) + " |");
		}
		Console.WriteLine("--" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "--");
	}

	private void IncreaseArray()
	{
		var newSize = items.Length * 2;
		Array.Resize(ref items, newSize);
	}
}
