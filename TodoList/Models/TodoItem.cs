using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoApp.Commands;
namespace TodoApp.Models
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
		[NotMapped]
		public bool IsDone => Status == TodoStatus.Completed;
		public string MarkDone()
		{
			Status = TodoStatus.Completed;
			UpdateTimestamp();
			return $"Задача отмечена выполненной: {Text}";
		}


		public void SetLastUpdate(DateTime newDate)
		{
			LastUpdate = newDate;
		}

		public int GetIndex(List<TodoItem> list)
		{
			return list.IndexOf(this);
		}
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(500)]
		private string _text = string.Empty;

		public string Text
		{
			get => _text;
			set { _text = value; UpdateTimestamp(); }
		}

		public TodoStatus Status { get; set; }

		public DateTime CreationDate { get; set; }

		public DateTime LastUpdate { get; private set; }

		[ForeignKey("Profile")]
		public Guid ProfileId { get; set; }

		public virtual Profile? Profile { get; set; }
		[NotMapped]
		private IClock? _clock;
		public TodoItem(string text, bool isDone, DateTime creationDate, TodoStatus status)
		{
			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentException("Текст задачи не может быть пустым или содержать только пробелы");

			Text = text;
			CreationDate = creationDate;
			LastUpdate = creationDate; 
			Status = status;
		}

		public TodoItem(string text, IClock? clock = null)
		{
			_clock = clock ?? new SystemClock();
			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentException("Текст задачи не может быть пустым.");
			Text = text;
			Status = TodoStatus.NotStarted;
			CreationDate = _clock.Now;
			LastUpdate = _clock.Now;
		}

		public void UpdateText(string newText)
		{
			if (string.IsNullOrWhiteSpace(newText))
			{
				Console.WriteLine("Ошибка: текст задачи не может быть пустым.");
				return;
			}
			Text = newText;
		}

		private void UpdateTimestamp() => LastUpdate = _clock?.Now ?? DateTime.Now;

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

		public string GetCurrentStatusDisplayName() => GetStatusDisplayName(Status);

		public string GetShortInfo()
		{
			string shortText = Text.Length > 30 ? Text.Substring(0, 30) + "..." : Text;
			string status = GetCurrentStatusDisplayName();
			return $"{shortText} | {status} | {LastUpdate:dd.MM.yyyy HH:mm}";
		}

		public string GetFullInfo()
		{
			return $"=========== Полная информация о задаче ===========\n" +
				  $"Текст: {Text}\n" +
				  $"Статус: {GetCurrentStatusDisplayName()}\n" +
				  $"Дата изменения: {LastUpdate:dd.MM.yyyy HH:mm:ss}\n" +
				  $"==================================================";
		}

		public string GetFormattedInfo(int index)
		{
			string statusString = Status.ToString().ToLower();
			return $"{index + 1}.\"{Text}\" {IsDone.ToString().ToLower()} {CreationDate:yyyy-MM-ddTHH:mm:ss} {statusString}";
		}
	}
}
