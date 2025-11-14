namespace TodoList.classes;

public class TodoList
{
	private const int IndexWidth = 6;
	private const int TextWidth = 36;
	private const int StatusWidth = 14;
	private const int DateWidth = 16;

	public readonly List<TodoItem> Items = [];

	public void Add(TodoItem item)
	{
		Items.Add(item);
		Console.WriteLine($"Добавлена задача: {item.Text}");
	}

	public void Delete(int idx)
	{
		Items.RemoveAt(idx);
		Console.WriteLine($"Задача {idx} удалена.");
	}

	public void SetStatus(int idx, TodoStatus status)
	{
		Items[idx].SetStatus(status);
		Console.WriteLine($"Задача {Items[idx].Text} отмечена выполненной");
	}

	public void Update(int idx, string newText)
	{
		Items[idx].UpdateText(newText);
		Console.WriteLine("Задача обновлена");
	}

	public void Read(int idx)
	{
		Console.WriteLine(Items[idx].GetFullInfo(idx));
	}

	public void View(bool showIndex, bool showStatus, bool showUpdateDate)
	{
		List<string> headers = ["Текст задачи".PadRight(TextWidth)];
		if (showIndex) headers.Add("Индекс".PadRight(IndexWidth));
		if (showStatus) headers.Add("Статус".PadRight(StatusWidth));
		if (showUpdateDate) headers.Add("Дата обновления".PadRight(DateWidth));

		Console.WriteLine("+-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-+");
		Console.WriteLine("| " + string.Join(" | ", headers) + " |");
		Console.WriteLine("|-" + string.Join("-+-", headers.Select(it => new string('-', it.Length))) + "-|");

		for (var i = 0; i < Items.Count; i++)
		{
			var text = Items[i].Text.Replace("\n", " ");
			if (text.Length > 30) text = text.Substring(0, 30) + "...";
			
			var date = Items[i].LastUpdate.ToString("yyyy-MM-dd HH:mm");

			List<string> rows = [text.PadRight(TextWidth)];
			if (showIndex) rows.Add(i.ToString().PadRight(IndexWidth));
			if (showStatus) rows.Add(Items[i].Status.ToString().PadRight(StatusWidth));
			if (showUpdateDate) rows.Add(date.PadRight(DateWidth));

			Console.WriteLine("| " + string.Join(" | ", rows) + " |");
		}

		Console.WriteLine("+-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-+");
	}
}