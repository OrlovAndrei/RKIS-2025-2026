using System;
using TodoList.Commands;

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

		public TodoItem(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentNullException(nameof(text), "Текст задачи не может быть пустым.");
			}
			
			Text = text ?? string.Empty;
			Status = TodoStatus.NotStarted;
			LastUpdate = DateTime.Now;
		}

		public TodoItem(string text, TodoStatus status, DateTime lastUpdate)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentNullException(nameof(text), "Текст задачи не может быть пустым.");
			}
			
			Text = text ?? string.Empty;
			Status = status;
			LastUpdate = lastUpdate;
		}

		public void MarkDone()
		{
			if (Status != TodoStatus.Completed)
			{
				Status = TodoStatus.Completed;
				LastUpdate = DateTime.Now;
			}
		}

		public void SetStatus(TodoStatus newStatus)
		{
			Status = newStatus;
			LastUpdate = DateTime.Now;
		}

		public void UpdateText(string newText)
		{
			Text = newText ?? string.Empty;
			LastUpdate = DateTime.Now;
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