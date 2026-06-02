using System;

namespace TodoList
{
	public class TodoItem
	{
		public string Text { get; private set; }
		public TodoStatus Status { get; private set; }
		public DateTime LastUpdate { get; private set; }

		public TodoItem(string text, TodoStatus status = TodoStatus.NotStarted, DateTime? lastUpdate = null)
		{
			Text = text;
			Status = status;
			LastUpdate = lastUpdate ?? DateTime.Now;
		}

		public void ChangeStatus(TodoStatus newStatus)
		{
			Status = newStatus;
			LastUpdate = DateTime.Now;
		}

		public string GetStatusString()
		{
			return Status switch
			{
				TodoStatus.NotStarted => "не начато",
				TodoStatus.InProgress => "в процессе",
				TodoStatus.Completed => "выполнено",
				TodoStatus.Postponed => "отложено",
				TodoStatus.Failed => "провалено",
				_ => "неизвестно"
			};
		}
	}
}