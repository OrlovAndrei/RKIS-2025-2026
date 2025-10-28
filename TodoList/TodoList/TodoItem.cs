public class TodoItem
{
	private string _text;
	private bool _isDone;
	private DateTime _lastUpdate;
	public TodoItem(string text)
	{
		_text = text;
		_isDone = false;
		_lastUpdate = DateTime.Now;
	}
	public void MarkDone()
	{
		_isDone = true;
		_lastUpdate = DateTime.Now;
	}
	public void UpdateText(string newText)
	{
		_text = newText;
		_lastUpdate = DateTime.Now;
	}
	public string GetShortInfo()
	{
		string shortText = _text.Length <= 30 ? _text : _text.Substring(0, 27) + "...";
		return shortText;
	}
	public string GetFullInfo()
	{
		return $"Текст задачи: \n{_text}\nСтатус: {(_isDone ? "Выполнена" : "Не выполнена")}\nДата последнего изменения: {_lastUpdate:dd.MM.yyyy HH:mm:ss}";
	}
	public string GetText() => _text;
	public bool GetIsDone() => _isDone;
	public DateTime GetLastUpdate() => _lastUpdate;
}
