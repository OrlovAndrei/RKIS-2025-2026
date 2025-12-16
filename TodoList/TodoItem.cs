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
		return Text.Length <= 30 ? Text : Text.Substring(0, 27) + "...";;
	}
	
	public string GetFullInfo()
	{
		return $"Текст задачи: \n{Text}\n" +
		       $"Статус: {(IsDone ? "Выполнена" : "Не выполнена")}\n" +
		       $"Дата последнего изменения: {LastUpdate:dd.MM.yyyy HH:mm:ss}";
	}
}
