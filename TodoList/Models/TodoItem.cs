using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models
{
	public class TodoItem
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(500)]
		public string Text { get; set; } = string.Empty;

		public TodoStatus Status { get; set; } = TodoStatus.NotStarted;

		public DateTime CreationDate { get; set; } = DateTime.Now;

		public DateTime LastUpdate { get; set; } = DateTime.Now;

		[ForeignKey("Profile")]
		public Guid ProfileId { get; set; }

		public virtual Profile? Profile { get; set; }

		[NotMapped]
		public bool IsDone => Status == TodoStatus.Completed;

		public void UpdateText(string newText)
		{
			if (string.IsNullOrWhiteSpace(newText))
				throw new ArgumentException("Текст задачи не может быть пустым.");
			Text = newText;
			LastUpdate = DateTime.Now;
		}

		public void SetStatus(TodoStatus newStatus)
		{
			Status = newStatus;
			LastUpdate = DateTime.Now;
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
