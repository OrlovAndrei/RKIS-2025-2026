using System;

namespace Todolist
{
	public class TodoItem
	{
		private string text;
		private bool isDone;
		private DateTime lastUpdate;
		public string Text
		{
			get => text;
			private set => text = value;
		}
		public bool IsDone
		{
			get => isDone;
			private set => isDone = value;
		}
		public DateTime LastUpdate
		{
			get => lastUpdate;
			private set => lastUpdate = value;
		}
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
		public string GetShortInfo()
		{
			string shortText = Text.Length > 30 ? Text.Substring(0, 27) + "..." : Text;
			string status = IsDone ? "Выполнена" : "Не выполнена";
			string date = LastUpdate.ToString("dd.MM.yyyy HH:mm");

			return $"{shortText,-30} | {status,-12} | {date}";
		}
		public string GetFullInfo()
		{
			string status = IsDone ? "Выполнена" : "Не выполнена";
			string date = LastUpdate.ToString("dd.MM.yyyy HH:mm");

			return $"Задача: {Text}\nСтатус: {status}\nПоследнее изменение: {date}";
		}
	}
}