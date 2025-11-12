using System;

namespace Todolist
{
	public class TodoItem
	{
		public string Text { get; private set; }
		public bool IsDone { get; set; }
		public DateTime LastUpdate { get; set; }

		public TodoItem(string text)
		{
			Text = text;
			IsDone = false;
			LastUpdate = DateTime.Now;
		}

		public void MarkDone()
		{
			IsDone = true;
			LastUpdate = DateTime.Now;
		}

		public void UpdateText(string newText)
		{
			Text = newText;
			LastUpdate = DateTime.Now;
		}

		public string GetFullInfo()
		{
			return $"Текст: {Text}\nСтатус: {(IsDone ? "Выполнено" : "Не выполнено")}\nПоследнее обновление: {LastUpdate:yyyy-MM-dd HH:mm:ss}";
		}
	}
}