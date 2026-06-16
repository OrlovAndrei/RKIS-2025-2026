namespace TodoList;
public class TodoList
{
	TodoItem[] items = new TodoItem[2];
	int taskCount;

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
		Console.WriteLine(items[idx].GetFullInfo());
	}

	public void View(bool showIndex, bool showStatus, bool showUpdateDate)
	{
		List<string> headers = ["Текст задачи".PadRight(30)];
		if (showIndex) headers.Add("Индекс".PadRight(8));
		if (showStatus) headers.Add("Статус".PadRight(16));
		if (showUpdateDate) headers.Add("Дата обновления".PadRight(16));

		Console.WriteLine("+-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-+");
		Console.WriteLine("| " + string.Join(" | ", headers) + " |");
		Console.WriteLine("|-" + string.Join("-+-", headers.Select(it => new string('-', it.Length))) + "-|");

		for (int i = 0; i < taskCount; i++)
		{
			string text = items[i].GetShortInfo().Replace("\n", " ");

			string status = items[i].IsDone ? "выполнена" : "не выполнена";
			string date = items[i].LastUpdate.ToString("yyyy-MM-dd HH:mm");

			List<string> rows = [text.PadRight(30)];
			if (showIndex) rows.Add(i.ToString().PadRight(8));
			if (showStatus) rows.Add(status.PadRight(16));
			if (showUpdateDate) rows.Add(date.PadRight(16));

			Console.WriteLine("| " + string.Join(" | ", rows) + " |");
		}
		Console.WriteLine("+-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-+");
	}

	private void IncreaseArray()
	{
		var newSize = items.Length * 2;
		Array.Resize(ref items, newSize);
	}
}
