namespace TodoList;

public class TodoItem
{
	public string Text;
	public TodoStatus Status;
	public DateTime LastUpdate;

	public TodoItem(string text)
	{
		Text = text;
		Status = TodoStatus.NotStarted;
		LastUpdate = DateTime.Now;
	}

	public TodoItem(string text, TodoStatus status, DateTime lastUpdate)
	{
		Text = text;
		Status = status;
		LastUpdate = lastUpdate;
	}

	public void SetStatus(TodoStatus newStatus)
	{
		Status = newStatus;
		LastUpdate = DateTime.Now;
	}

	public void UpdateText(string newText)
	{
		Text = newText;
		LastUpdate = DateTime.Now;
	}

	public string GetShortInfo()
	{
		string text = Text.Replace("\r", " ").Replace("\n", " ");
		if (text.Length > 30) text = text[..30] + "...";

		string status = Status.ToString();
		return $"{text,-36}|{status,-16}|{LastUpdate,-16:yyyy-MM-dd HH:mm}|";
	}

	public string GetFullInfo(int index)
	{
		string status = Status.ToString();
		return $"Индекс:{index + 1}\nДата:{LastUpdate}\nНазвание:{Text}\nСтатус:{status}";
	}
}