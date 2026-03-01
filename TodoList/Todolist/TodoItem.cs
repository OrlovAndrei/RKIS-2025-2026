using System;

namespace Todolist
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

        public void SetStatus(TodoStatus newStatus)
        {
            Status = newStatus;
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
            string status = GetStatusText(Status);
            string date = LastUpdate.ToString("dd.MM.yy HH:mm");

            return $"{shortText} [{status}] - {date}";
        }

        public string GetFullInfo()
        {
            string status = GetStatusText(Status);
            return $"Текст: {Text}\nСтатус: {status}\nИзменено: {LastUpdate:dd.MM.yyyy HH:mm}";
        }

        private string GetStatusText(TodoStatus status)
        {
            return status switch
            {
                TodoStatus.NotStarted => "Не начато",
                TodoStatus.InProgress => "В процессе",
                TodoStatus.Completed => "Выполнено",
                TodoStatus.Postponed => "Отложено",
                TodoStatus.Failed => "Провалено",
                _ => "Неизвестно"
            };
        }
    }
}
