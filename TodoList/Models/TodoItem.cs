using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoApp.Models;

namespace TodoApp.Models
{
	public class TodoItem
	{
		private readonly IClock _clock;

		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(500)]
		public string Text { get; set; } = string.Empty;

		public TodoStatus Status { get; set; } = TodoStatus.NotStarted;

		public DateTime CreationDate { get; set; }

		public DateTime LastUpdate { get; set; }

		[ForeignKey("Profile")]
		public Guid ProfileId { get; set; }

		public virtual Profile? Profile { get; set; }

		[NotMapped]
		public bool IsDone => Status == TodoStatus.Completed;

		public TodoItem()
		{
			_clock = new SystemClock();

			CreationDate = _clock.Now;
			LastUpdate = _clock.Now;
		}

		public TodoItem(
			string text,
			IClock? clock = null)
		{
			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentException("Текст задачи не может быть пустым.");

			_clock = clock ?? new SystemClock();

			Text = text;

			CreationDate = _clock.Now;
			LastUpdate = _clock.Now;
		}

		public TodoItem(
			string text,
			bool isDone,
			DateTime creationDate,
			TodoStatus status,
			IClock? clock = null)
		{
			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentException("Текст задачи не может быть пустым.");

			_clock = clock ?? new SystemClock();

			Text = text;

			Status = isDone
				? TodoStatus.Completed
				: status;

			CreationDate = creationDate;
			LastUpdate = _clock.Now;
		}

		public void UpdateText(string newText)
		{
			if (string.IsNullOrWhiteSpace(newText))
				throw new ArgumentException("Текст задачи не может быть пустым.");

			Text = newText;

			LastUpdate = _clock.Now;
		}

		public void SetStatus(TodoStatus newStatus)
		{
			Status = newStatus;

			LastUpdate = _clock.Now;
		}

		public string MarkDone()
		{
			Status = TodoStatus.Completed;

			LastUpdate = _clock.Now;

			return $"Задача отмечена выполненной: {Text}";
		}

		public void SetLastUpdate(DateTime date)
		{
			LastUpdate = date;
		}

		public string GetCurrentStatusDisplayName()
		{
			return GetStatusDisplayName(Status);
		}

		public string GetShortInfo()
		{
			string shortText = Text.Length > 30
				? Text.Substring(0, 30) + "..."
				: Text;

			return $"{shortText} [{GetCurrentStatusDisplayName()}]";
		}

		public string GetFullInfo()
		{
			return
				$"Текст: {Text}\n" +
				$"Статус: {GetCurrentStatusDisplayName()}\n" +
				$"Дата создания: {CreationDate}\n" +
				$"Дата изменения: {LastUpdate}";
		}

		public string GetFormattedInfo(int index)
		{
			return
				$"{index + 1}. {Text} | " +
				$"{CreationDate:yyyy-MM-ddTHH:mm:ss} | " +
				$"{Status.ToString().ToLower()}";
		}

		public int GetIndex(List<TodoItem> items)
		{
			return items.IndexOf(this);
		}

		public static string GetStatusDisplayName(TodoStatus status)
		{
			return status switch
			{
				TodoStatus.NotStarted => "Не начата",
				TodoStatus.InProgress => "В процессе",
				TodoStatus.Completed => "Выполнена",
				TodoStatus.Postponed => "Отложена",
				TodoStatus.Failed => "Провалена",
				_ => "Неизвестно"
			};
		}
	}
}