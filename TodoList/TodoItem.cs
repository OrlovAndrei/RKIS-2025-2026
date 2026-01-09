namespace TodoList;

public class TodoItem
{
	public string Text { get; private set; }
	public bool IsDone { get; private set; }
	public DateTime LastUpdate { get; private set; }

	public TodoItem(string text)
	{
		Text = text;
		IsDone = false;
		LastUpdate = DateTime.Now;
	}

	public void MarkDone()
	{
		IsDone = true;
		LastUpdate = DateTime.Now;
	}

	public void UpdateText(string newText)
	{
		Text = newText;
		LastUpdate = DateTime.Now;
	}

	public string GetShortInfo()
	{
		var title = Text.Replace("\n", " ");
		if (title.Length > 27) title = title.Substring(0, 27) + "...";

		return title;
	}

	public string GetFullInfo()
	{
		return $"Текст: {Text}\nСтатус: {(IsDone ? "Выполнено" : "Не выполнено")}\nИзменено: {LastUpdate:dd.MM.yyyy HH:mm:ss}";
	}
}