using TodoApp;
using TodoApp.Exceptions;
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
	private readonly IClock _clock;
	private string _text;
	private TodoStatus _status;
	private DateTime _lastUpdate;

	// Конструктор с IClock (по умолчанию SystemClock)
	public TodoItem(string text, IClock clock = null)
	{
		_clock = clock ?? new SystemClock();
		_text = text;
		_status = TodoStatus.NotStarted;
		_lastUpdate = _clock.Now;
	}

	// Конструктор с указанием статуса и времени (для загрузки из файла)
	public TodoItem(string text, TodoStatus status, DateTime lastUpdate, IClock clock = null)
	{
		_clock = clock ?? new SystemClock();
		_text = text;
		_status = status;
		_lastUpdate = lastUpdate;
	}

	public bool GetIsDone()
	{
		return _status == TodoStatus.Completed;
	}

	public void MarkDone()
	{
		_status = TodoStatus.Completed;
		_lastUpdate = _clock.Now;
	}

	public void UpdateText(string newText)
	{
		_text = newText;
		_lastUpdate = _clock.Now;
	}

	public void UpdateStatus(TodoStatus newStatus)
	{
		_status = newStatus;
		_lastUpdate = _clock.Now;
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
}