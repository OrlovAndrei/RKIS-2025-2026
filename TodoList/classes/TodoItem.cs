namespace TodoList.classes;

public class TodoItem(string text, TodoStatus status, DateTime lastUpdate)
{
	public TodoItem(string text) : this(text, TodoStatus.NotStarted, DateTime.Now) {}

	public string Text { get; private set; } = text;
	public TodoStatus Status { get; private set; } = status;
	public DateTime LastUpdate { get; private set; } = lastUpdate;

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

	public string GetFullInfo(int index) => $"Полный текст задачи {index}:\n{Text}\nСтатус: {status}\nДата последнего изменения: {LastUpdate:yyyy-MM-dd HH:mm}";
}