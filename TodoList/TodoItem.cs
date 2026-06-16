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
		string text = Text.Replace("\r", " ").Replace("\n", " ");
		if (text.Length > 30) text = text.Substring(0, 30) + "...";

		string status = IsDone ? "Выполнена" : "Не выполнена";
		return $"{text,-34}|{status,-16}|{LastUpdate:yyyy-MM-dd HH:mm}|";
	}

	public string GetFullInfo(int index)
	{
		string status = IsDone ? "выполнена" : "не выполнена";
		return $"Полный текст задачи {index}:\n{Text}\nСтатус: {status}\nДата изменения: {LastUpdate:yyyy-MM-dd HH:mm}";
	}
}