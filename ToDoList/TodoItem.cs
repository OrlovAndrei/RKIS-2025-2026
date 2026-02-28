using System;

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
		public string Text { get; private set; }
		public TodoStatus Status { get; private set; }
		public DateTime LastUpdate { get; private set; }

		public TodoItem(string text)
		{
			this.Text = text;
			this.Status = TodoStatus.NotStarted;
			this.LastUpdate = DateTime.Now;
		}

		public void SetStatus(TodoStatus status)
		{
			this.Status = status;
			this.LastUpdate = DateTime.Now;
		}

		public void UpdateText(string newText)
		{
			this.Text = newText;
			this.LastUpdate = DateTime.Now;
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