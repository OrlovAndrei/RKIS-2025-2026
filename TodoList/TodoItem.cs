using System;

namespace TodoList
{
	public class TodoItem
	{
		public string Text { get; private set; }
		public TodoStatus Status { get; private set; }
		public DateTime LastUpdate { get; private set; }

		public TodoItem(string text)
		{
			Text = text;
			Status = TodoStatus.NotStarted;
			LastUpdate = DateTime.Now;
		}

		public void ChangeStatus(TodoStatus newStatus)
		{
			Status = newStatus;
			LastUpdate = DateTime.Now;
		}

		public void UpdateText(string newText)
		{
			if (!string.IsNullOrEmpty(newText))
			{
				Text = newText;
				LastUpdate = DateTime.Now;
			}
		}

		public string GetFullInfo()
		{
			string statusText = GetStatusString();
			string dateText = LastUpdate.ToString("yyyy-MM-dd HH:mm:ss");
			return $"Задача: {Text}\nСтатус: {statusText}\nПоследнее обновление: {dateText}";
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