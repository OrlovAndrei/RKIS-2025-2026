using System;

namespace TodoList
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
        public string Text { get; private set; }
        public TodoStatus Status { get; private set; }
        public DateTime LastUpdate { get; private set; }
        public bool IsMultiline { get; private set; }

        public TodoItem(string text, bool isMultiline = false)
        {
            Text = text;
            Status = TodoStatus.NotStarted;
            LastUpdate = DateTime.Now;
            IsMultiline = isMultiline;
        }

        public TodoItem(string text, TodoStatus status, DateTime lastUpdate, bool isMultiline = false)
        {
            Text = text;
            Status = status;
            LastUpdate = lastUpdate;
            IsMultiline = isMultiline;
        }

        public void SetStatus(TodoStatus status)
        {
            Status = status;
            LastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText, bool isMultiline)
        {
            Text = newText;
            IsMultiline = isMultiline;
            LastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            UpdateText(newText, false);
        }

        public string GetShortInfo()
        {
            string displayText = IsMultiline ? "[МНОГОСТРОЧНАЯ] " + GetFirstLine() : Text;
            string singleLineText = displayText.Replace("\n", " | ");
            string shortText = singleLineText.Length > 30 ? singleLineText.Substring(0, 27) + "..." : singleLineText;
            string statusText = GetStatusText(Status);
            return $"{shortText} | {statusText} | {LastUpdate:dd.MM.yyyy HH:mm}";
        }

        public string GetFullInfo()
        {
            string multilineIndicator = IsMultiline ? " (многострочная)" : "";
            return $"Текст{multilineIndicator}:\n{Text}\n\nСтатус: {GetStatusText(Status)}\nДата изменения: {LastUpdate:dd.MM.yyyy HH:mm}";
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

        private string GetFirstLine()
        {
            if (string.IsNullOrEmpty(Text))
                return string.Empty;

            int newLineIndex = Text.IndexOf('\n');
            if (newLineIndex >= 0)
                return Text.Substring(0, Math.Min(newLineIndex, 30)) + "...";
            
            return Text.Length > 30 ? Text.Substring(0, 27) + "..." : Text;
        }
    }
}