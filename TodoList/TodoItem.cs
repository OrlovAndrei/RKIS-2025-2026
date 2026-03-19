using System;
using System.Text.Json.Serialization;
using TodoList.Interfaces;

namespace TodoList
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
		public string Text { get; private set; }
		public TodoStatus Status { get; private set; }
		public DateTime LastUpdate { get; private set; }

		private readonly IClock _clock;
		public TodoItem(string text) : this(text, new SystemClock()) { }
		public TodoItem(string text, IClock clock)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentNullException(nameof(text), "Текст задачи не может быть пустым.");
			}
			
			_clock = clock;
			Text = text;
			Status = TodoStatus.NotStarted;
			LastUpdate = _clock.Now;
		}

		[JsonConstructor]
		public TodoItem(string text, TodoStatus status, DateTime lastUpdate) 
			: this(text, new SystemClock())
		{
			Text = text ?? string.Empty;
			Status = status;
			LastUpdate = lastUpdate;
		}

		public void MarkDone()
		{
			if (Status != TodoStatus.Completed)
			{
				Status = TodoStatus.Completed;
				LastUpdate = _clock.Now;
			}
		}

		public void SetStatus(TodoStatus newStatus)
		{
			Status = newStatus;
			LastUpdate = _clock.Now;
		}

		public void UpdateText(string newText)
		{
			Text = newText ?? string.Empty;
			LastUpdate = _clock.Now;
		}

		public string GetShortInfo(int maxLen = 30)
		{
			if (maxLen < 4) maxLen = 4;
			string clean = (Text ?? string.Empty).Replace("\r", " ").Replace("\n", " ").Trim();
			if (clean.Length <= maxLen) return clean;
			return clean.Substring(0, maxLen - 3) + "...";
		}
	}
}