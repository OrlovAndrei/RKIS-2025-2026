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

		var status = IsDone ? "выполнена" : "не выполнена";
		return $"{text,-34}|{status,-16}|{LastUpdate:yyyy-MM-dd HH:mm}|";
	}

	public string GetFullInfo(int index)
	{
		var status = IsDone ? "выполнена" : "не выполнена";
		return
			$"Полный текст задачи {index}:\n{Text}\nСтатус: {status}\nДата последнего изменения: {LastUpdate:yyyy-MM-dd HH:mm}";
	}
}