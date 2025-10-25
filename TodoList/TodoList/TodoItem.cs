public class TodoItem
{
	private string textt;
	private bool isDonee;
	private DateTime lastUpdatee;
	public TodoItem(string text)
	{
		textt = text;
		isDonee = false;
		lastUpdatee = DateTime.Now;
	}
	public void MarkDone()
	{
		isDonee = true;
		lastUpdatee = DateTime.Now;
	}
	public void UpdateText(string newText)
	{
		textt = newText;
		lastUpdatee = DateTime.Now;
	}
	public string GetShortInfo()
	{
		string shortText = textt.Length <= 30 ? textt : textt.Substring(0, 27) + "...";
		return shortText;
	}
	public string GetFullInfo()
	{
		return $"Текст задачи: \n{textt}\nСтатус: {(isDonee ? "Выполнена" : "Не выполнена")}\nДата последнего изменения: {lastUpdatee:dd.MM.yyyy HH:mm:ss}";
	}
	public string GetText() => textt;
	public bool GetIsDone() => isDonee;
	public DateTime GetLastUpdate() => lastUpdatee;
}
