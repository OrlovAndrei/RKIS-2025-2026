namespace TodoList;

public class TodoItem
{
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

	public string Text { get; private set; }
	public TodoStatus Status { get; private set; }
	public DateTime LastUpdate { get; private set; }

	public void SetStatus(TodoStatus status)
	{
		Status = status;
		LastUpdate = DateTime.Now;
	}

	public void UpdateText(string newText)
	{
		Text = newText;
		LastUpdate = DateTime.Now;
	}

	public string GetFullInfo()
	{
		return
			$"Текст: {Text}\nСтатус: {Status}\nДата изменения: {LastUpdate:dd.MM.yyyy HH:mm}";
	}
}