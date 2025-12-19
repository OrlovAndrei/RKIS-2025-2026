namespace TodoList;

public class TodoItem
{
	public TodoItem(string text)
	{
		Text = text;
		IsDone = false;
		LastUpdate = DateTime.Now;
	}
	public TodoItem(string text, bool isDone, DateTime lastUpdate)
	{
		Text = text;
		IsDone = isDone;
		LastUpdate = lastUpdate;
	}

	public string Text { get; private set; }
	public bool IsDone { get; private set; }
	public DateTime LastUpdate { get; private set; }

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
		var text = Text.Replace("\r", " ").Replace("\n", " ");
		if (text.Length > 30) text = text.Substring(0, 30) + "...";

		return text;
	}

	public string GetFullInfo(int index)
	{
		return
			$"Текст: {Text}" +
			$"Статус: {(IsDone ? "Выполнено" : "Не выполнено")}" +
			$"Изменено: {LastUpdate:dd.MM.yyyy HH:mm:ss}";
	}
}