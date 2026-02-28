using System;

namespace Todolist
{
	public enum TodoStatus
	{
		NotStarted, // Не начато
		InProgress, // В процессе
		Completed,  // Завершено
		Postponed, // Отложено
		Failed      // Провалено
	}
	public class TodoItem
	{
		public string Text { get; private set; }
		public TodoStatus Status { get; set; }
		public DateTime LastUpdate { get; set; }

		public TodoItem(string text)
		{
			Text = text;
			Status = TodoStatus.NotStarted;
			LastUpdate = DateTime.Now;
		}

		public void MarkDone()
		{
			Status = TodoStatus.Completed;
			LastUpdate = DateTime.Now;
		}

		public void UpdateText(string newText)
		{
			Text = newText;
			LastUpdate = DateTime.Now;
		}

		public string GetFullInfo()
		{
			string statusText = Status switch
			{
				TodoStatus.NotStarted => "Не начато",
				TodoStatus.InProgress => "В процессе",
				TodoStatus.Completed => "Завершено",
				TodoStatus.Postponed => "Отложено",
				TodoStatus.Failed => "Провалено",
				_ => Status.ToString()
			};
			return $"Текст: {Text}\nСтатус: {statusText}\nПоследнее обновление: {LastUpdate:yyyy-MM-dd HH:mm:ss}";
		}
	}
}