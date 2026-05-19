using System;
using ToDoList;

namespace TodoList
{
	public enum TodoStatus
	{
		NotStarted, // не начато
		InProgress, // в процессе
		Completed, // выполнено
		Postponed, // отложено
		Failed // провалено
	}


	public class TodoItem
	{
		public string Title { get; set; }

		public bool IsDone { get; set; }

		public readonly IClock _clock;
		public string Text { get; set; }
		public TodoStatus Status { get; set; }
		public DateTime LastUpdate { get; set; }
		public TodoItem()
		{

		}

		public TodoItem(string text, IClock clock = null)
		{
			_clock = clock ?? new SystemClock();
			this.Text = text;
			this.Status = TodoStatus.NotStarted;
			this.LastUpdate = _clock.Now;
		}

		public void SetStatus(TodoStatus status)
		{
			this.Status = status;
			this.LastUpdate = _clock.Now;
		}

		public void UpdateText(string newText)
		{
			this.Text = newText;
			this.LastUpdate = _clock.Now;
		}

		public string GetShortInfo()
		{
			string shortText = Text.Length > 30 ? Text.Substring(0, 30) + "..." : Text;
			string status = GetStatusText();
			string date = LastUpdate.ToString("dd.MM.yyyy HH:mm");
			return $"{shortText} | {status} | {date}";
		}

		public string GetFullInfo()
		{
			string statusText = GetStatusText();
			string dateText = LastUpdate.ToString("dd.MM.yyyy HH:mm");
			return $"Текст: {Text}\nСтатус: {statusText}\nДата изменения: {dateText}";
		}

		public string GetStatusText()
		{
			return Status switch
			{
				TodoStatus.NotStarted => "Не начато",
				TodoStatus.InProgress => "В процессе",
				TodoStatus.Completed => "Выполнено",
				TodoStatus.Postponed => "Отложено",
				TodoStatus.Failed => "Провалено"
			};
		}
	}
}