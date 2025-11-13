namespace TodoApp
{
	public enum TodoStatus
	{
    	NotStarted,
    	InProgress, 
    	Completed,
    	Postponed,
    	Failed
	}
	public class TodoItem
	{
		private string _text;
		private TodoStatus _status;
		private DateTime _lastUpdate;
		public DateTime CreationDate { get; set; }
		public string Text
		{
			get => _text;
			private set { _text = value; UpdateTimestamp(); }
		}
		public bool IsDone 
        {
            get => _status == TodoStatus.Completed;
            set 
            { 
                if (value)
                    _status = TodoStatus.Completed;
                else if (_status == TodoStatus.Completed)
                    _status = TodoStatus.NotStarted;
                UpdateTimestamp();
            }
        }
		public TodoStatus Status
        {
            get => _status;
            set { _status = value; UpdateTimestamp(); }
        }
		public DateTime LastUpdate
        {
            get => _lastUpdate;
            private set { _lastUpdate = value; }
        }
        public TodoItem(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Текст задачи не может быть пустым.");
            _text = text;
            _status = TodoStatus.NotStarted;
            CreationDate = DateTime.Now;
            _lastUpdate = DateTime.Now;
        }
        public TodoItem(string text, bool IsDone, DateTime creationDate, TodoStatus status)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Текст задачи не может быть пустым.");
            _text = text;
            _status = status;
            CreationDate = creationDate;
            _lastUpdate = DateTime.Now;
        }
		public string MarkDone()
		{
			IsDone = true;
			return $"Задача отмечена выполненной: {Text}";
		}
		public void UpdateText(string newText)
		{
			if (string.IsNullOrWhiteSpace(newText))
			{
				Console.WriteLine("Ошибка: текст задачи не может быть пустым.");
				return;
			}
			Text = newText;
		}
		private void UpdateTimestamp() => _lastUpdate = DateTime.Now;
		public static string GetStatusDisplayName(TodoStatus status)
        {
            return status switch
            {
                TodoStatus.NotStarted => "Не начата",
                TodoStatus.InProgress => "В процессе", 
                TodoStatus.Completed => "Выполнена",
                TodoStatus.Postponed => "Отложена",
                TodoStatus.Failed => "Провалена",
                _ => "Неизвестно"
            };
        }
		public string GetCurrentStatusDisplayName()
        {
            return GetStatusDisplayName(_status);
        }
        public string GetShortInfo()
        {
            string shortText = _text.Length > 30 ? _text.Substring(0, 30) + "..." : _text;
            string status = GetCurrentStatusDisplayName();
            return $"{shortText} | {status} | {_lastUpdate:dd.MM.yyyy HH:mm}";
        }
		public string GetFullInfo()
        {
            return $"=========== Полная информация о задаче ===========\n" +
                  $"Текст: {_text}\n" +
                  $"Статус: {GetCurrentStatusDisplayName()}\n" +
                  $"Дата изменения: {_lastUpdate:dd.MM.yyyy HH:mm:ss}\n" +
                  $"==================================================";
        }
        public string GetFormattedInfo(int index)
        {
            string statusString = Status.ToString().ToLower();
            return $"{index + 1}.\"{Text}\" {IsDone.ToString().ToLower()} {CreationDate:yyyy-MM-ddTHH:mm:ss} {statusString}";
        }
	}
}