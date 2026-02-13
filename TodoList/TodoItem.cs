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

        public void UpdateText(string newText)
        {
            Text = newText;
            LastUpdate = DateTime.Now;
        }

        public void SetStatus(TodoStatus newStatus)
        {
            Status = newStatus;
            LastUpdate = DateTime.Now;
        }

        public string GetShortInfo()
        {
            string status = $"[{Status}]";
            string shortText = Text.Length > 30 ? Text.Substring(0, 27) + "..." : Text;
            return $"{shortText} {status} ({LastUpdate:dd.MM.yyyy HH:mm})";
        }

        public string GetFullInfo()
        {
            return $"Текст: {Text}\nСтатус: {Status}\nДата последнего изменения: {LastUpdate:dd.MM.yyyy HH:mm}";
        }
    }
}