namespace TodoList;

public class TodoItem
{
	public string Text;
	public bool IsDone;
	public DateTime LastUpdate;

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
		if (text.Length > 30) text = text[..30] + "...";

		string status = IsDone ? "выполнена" : "не выполнена";
		return $"{text,-36}|{status,-16}|{LastUpdate,-16:yyyy-MM-dd HH:mm}|";
	}

	public string GetFullInfo(int index)
	{
		string status = IsDone ? "выполнена" : "не выполнена";
		return $"Индекс:{index + 1}\nДата:{LastUpdate}\nНазвание:{Text}\nСтатус:{status}";
	}
}