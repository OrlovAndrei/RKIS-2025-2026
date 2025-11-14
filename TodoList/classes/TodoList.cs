namespace TodoList;

public class TodoList
{
	private const int indexWidth = 6;
	private const int textWidth = 36;
	private const int statusWidth = 14;
	private const int dateWidth = 16;

	public readonly List<TodoItem> items = [];

	public void Add(TodoItem item)
	{
		items.Add(item);
		Console.WriteLine($"Добавлена задача: {item.Text}");
	}

	public void Delete(int idx)
	{
		items.RemoveAt(idx);
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
		if (showIndex) headers.Add("Индекс".PadRight(indexWidth));
		if (showStatus) headers.Add("Статус".PadRight(statusWidth));
		if (showUpdateDate) headers.Add("Дата обновления".PadRight(dateWidth));

		Console.WriteLine("+-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-+");
		Console.WriteLine("| " + string.Join(" | ", headers) + " |");
		Console.WriteLine("|-" + string.Join("-+-", headers.Select(it => new string('-', it.Length))) + "-|");

		for (var i = 0; i < items.Count; i++)
		{
			var text = items[i].Text.Replace("\n", " ");
			if (text.Length > 30) text = text.Substring(0, 30) + "...";

			var status = items[i].IsDone ? "выполнена" : "не выполнена";
			var date = items[i].LastUpdate.ToString("yyyy-MM-dd HH:mm");

			List<string> rows = [text.PadRight(textWidth)];
			if (showIndex) rows.Add(i.ToString().PadRight(indexWidth));
			if (showStatus) rows.Add(status.PadRight(statusWidth));
			if (showUpdateDate) rows.Add(date.PadRight(dateWidth));

			Console.WriteLine("| " + string.Join(" | ", rows) + " |");
		}

		Console.WriteLine("+-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-+");
	}
}