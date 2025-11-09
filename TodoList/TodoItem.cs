namespace TodoApp
{
	public class TodoItem
	{
		private string _text;
		private bool _isDone;
		private DateTime _lastUpdate;
		public DateTime CreationDate { get; set; }
		public string Text
		{
			get => _text;
			private set { _text = value; UpdateTimestamp(); }
		}
		public bool IsDone
		{
			get => _isDone;
			set { _isDone = value; UpdateTimestamp(); }
		}
		public DateTime LastUpdate
		{
			get => _lastUpdate;
			set { _lastUpdate = value; }
		}
		public TodoItem(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentException("Текст задачи не может быть пустым.");

			_text = text;
			_isDone = false;
			CreationDate = DateTime.Now;
			_lastUpdate = DateTime.Now;
		}
		public TodoItem(string text, bool isDone, DateTime creationDate)
		{
			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentException("Текст задачи не может быть пустым.");
			{
				_text = text;
				_isDone = false;
				CreationDate = DateTime.Now;
				LastUpdate = DateTime.Now;
				_lastUpdate = DateTime.Now;
			}
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
		public string GetShortInfo()
		{
			string shortText = _text.Length > 30 ? _text.Substring(0, 30) + "..." : _text;
			string status = _isDone ? "Выполнено" : "Не выполнено";
			return $"{shortText} | {status} | {_lastUpdate:dd.MM.yyyy HH:mm}";
		}
		public string GetFullInfo()
		{
			return $"=========== Полная информация о задаче ===========\n" +
				  $"Текст: {_text}\n" +
				  $"Статус: {(_isDone ? "Выполнено" : "Не выполнено")}\n" +
				  $"Дата изменения: {_lastUpdate:dd.MM.yyyy HH:mm:ss}\n" +
				  $"==================================================";
		}
		public string GetFormattedInfo(int index)
		{
			string creationDate = CreationDate.ToString("yyyy-MM-ddTHH:mm:ss");
			return $"{index};\"{_text}\";{_isDone.ToString().ToLower()};{creationDate}";
		}
	}
}