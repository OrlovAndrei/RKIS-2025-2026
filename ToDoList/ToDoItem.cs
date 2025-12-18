namespace MyTodoApp
{
	public class TaskItem
	{
		public string Description { get; private set; }
		public bool IsCompleted { get; private set; }
		public DateTime LastModified { get; private set; }

		public TaskItem(string description)
		{
			Description = description;
			IsCompleted = false;
			LastModified = DateTime.Now;
		}

		public void MarkComplete()
		{
			IsCompleted = true;
			LastModified = DateTime.Now;
		}

		public void UpdateDescription(string newDescription)
		{
			if (!string.IsNullOrEmpty(newDescription))
			{
				Description = newDescription;
				LastModified = DateTime.Now;
			}
		}

		public string GetBriefInfo()
		{
			const int maxLength = 30;
			string shortDesc = Description.Length > maxLength
				? Description.Substring(0, maxLength - 3) + "..."
				: Description;

			string status = IsCompleted ? "Выполнено" : "В процессе";
			string date = LastModified.ToString("yyyy-MM-dd HH:mm:ss");

			return $"{shortDesc} | {status} | {date}";
		}

		public string GetDetailedInfo()
		{
			string status = IsCompleted ? "Задача выполнена" : "Задача не выполнена";
			string date = LastModified.ToString("yyyy-MM-dd HH:mm:ss");

			return $"Задача:\n\t{Description}\nСтатус: {status}\nПоследнее обновление: {date}";
		}
	}
}