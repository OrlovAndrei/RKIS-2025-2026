using System;

namespace TodoApp
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
        
        public TodoItem(string text, TodoStatus status, DateTime lastUpdate)
        {
            Text = text;
            Status = status;
            LastUpdate = lastUpdate;
        }

        public void SetStatus(TodoStatus status)
        {
            Status = status;
            LastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            Text = newText;
            LastUpdate = DateTime.Now;
        }

        public string GetStatusString()
        {
            return Status switch
            {
                TodoStatus.NotStarted => "Не начата",
                TodoStatus.InProgress => "В процессе",
                TodoStatus.Completed => "Выполнена",
                TodoStatus.Postponed => "Отложена",
                TodoStatus.Failed => "Провалена",
                _ => "Неизвестно"
            };
        }

        public string GetShortInfo()
        {
            string shortText = Text.Length > 30 ? Text.Substring(0, 30) + "..." : Text;
            return $"{shortText} | {GetStatusString()} | {LastUpdate:dd.MM.yyyy HH:mm}";
        }

        public string GetFullInfo()
        {
            return $"Задача: {Text}\nСтатус: {GetStatusString()}\nПоследнее изменение: {LastUpdate:dd.MM.yyyy HH:mm}";
        }
    }
}
