namespace TodoList;
public class TodoItem
{
	public string Text { get; private set; }
	public TodoStatus Status { get; private set; }
	public DateTime LastUpdate { get; private set; }

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
		if (text.Length > 30) text = text.Substring(0, 30) + "...";

		string status = Status.ToString();
		return $"{text,-34}|{status,-16}|{LastUpdate:yyyy-MM-dd HH:mm}|";
	}

	public string GetFullInfo(int index)
	{
		string status = Status.ToString() ;
		return $"Полный текст задачи {index}:\n{Text}\nСтатус: {status}\nДата последнего изменения: {LastUpdate:yyyy-MM-dd HH:mm}";
	}
}