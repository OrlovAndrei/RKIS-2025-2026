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
	public string GetShortInfo() => Text.Length <= 30 ? Text : Text.Substring(0, 27) + "...";
	public string GetFullInfo() => $"Текст задачи: \n{Text}\n" +
	                               $"Статус: {(IsDone ? "Выполнена" : "Не выполнена")}\n" +
	                               $"Дата изменения: {LastUpdate:dd.MM.yyyy HH:mm:ss}";
}