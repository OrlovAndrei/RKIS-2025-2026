
namespace TodoList;
internal class Todoitem
{
	public string Text { get; set; }
	public bool IsDone { get; set; }
	public DateTime LastUpdate { get; set; }

	public Todoitem(string text)
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
		string status = IsDone ? "Done" : "Pending";
		string truncatedText = Text.Length > 30 ? Text.Substring(0, 30) + "..." : Text;
		return $"{truncatedText} ({status}, {LastUpdate.ToShortDateString()})";
	}

	public string GetFullInfo()
	{
		string status = IsDone ? "Выполнено" : "В процессе";
		return $"Задача: {Text}\nСтатус: {status}\nПоследнее обновление: {LastUpdate}";
	}
}
