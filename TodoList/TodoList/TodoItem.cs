public enum TodoStatus
{
	NotStarted,    // не начато
	InProgress,    // в процессе
	Completed,     // выполнено
	Postponed,     // отложено
	Failed         // провалено
}
public class TodoItem
{
	private string _text;
	private TodoStatus _status;
	private DateTime _lastUpdate;
	public bool GetIsDone()
	{
		return _status == TodoStatus.Completed;
	}
	public void SetStatus(TodoStatus status)
	{
		_status = status;
		_lastUpdate = DateTime.Now;
	}
	public TodoItem(string text)
	{
		_text = text;
		_status = TodoStatus.NotStarted;
		_lastUpdate = DateTime.Now;
	}
	public TodoItem(string text, TodoStatus status, DateTime lastUpdate)
	{
		_text = text;
		_status = status;
		_lastUpdate = lastUpdate;
	}
	public void MarkDone()
	{
		_status = TodoStatus.Completed;
		_lastUpdate = DateTime.Now;
	}
	public void UpdateText(string newText)
	{
		_text = newText;
		_lastUpdate = DateTime.Now;
	}
	public void UpdateStatus(TodoStatus newStatus)
	{
		_status = newStatus;
		_lastUpdate = DateTime.Now;
	}
	public string GetShortInfo()
	{
		string shortText = _text.Length <= 30 ? _text : _text.Substring(0, 27) + "...";
		return shortText;
	}
	public string GetFullInfo()
	{
		return $"Текст задачи: \n{_text}\nСтатус: {GetStatusText()}\nДата последнего изменения: {_lastUpdate:dd.MM.yyyy HH:mm:ss}";
	}
	public string GetText() => _text;
	public TodoStatus GetStatus() => _status;
	public DateTime GetLastUpdate() => _lastUpdate;
	public string GetStatusText()
	{
		return _status switch
		{
			TodoStatus.NotStarted => "Не начато",
			TodoStatus.InProgress => "В процессе",
			TodoStatus.Completed => "Выполнено",
			TodoStatus.Postponed => "Отложено",
			TodoStatus.Failed => "Провалено",
			_ => "Неизвестно"
		};
	}
}