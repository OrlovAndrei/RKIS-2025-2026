using System;

public enum TodoStatus
{
    NotStarted,
    InProgress,
    Completed,
    Postponed,
    Failed
}

class TodoItem
{
    private string text;
    private TodoStatus status;
    private DateTime lastUpdate;

    public string Text
    {
        get { return text; }
        set { text = value; }
    }

    public TodoStatus Status
    {
        get { return status; }
        set { status = value; }
    }

    public DateTime LastUpdate
    {
        get { return lastUpdate; }
        set { lastUpdate = value; }
    }

    public TodoItem(string text)
    {
        this.text = text;
        this.status = TodoStatus.NotStarted;
        this.lastUpdate = DateTime.Now;
    }

    public void UpdateText(string newText)
    {
        text = newText;
        lastUpdate = DateTime.Now;
    }

    public string GetShortInfo()
    {
        string truncatedText = text ?? string.Empty;
        if (truncatedText.Length > 30)
        {
            truncatedText = truncatedText.Substring(0, 27) + "...";
        }
        
        string statusStr = GetStatusString(status);
        string dateStr = lastUpdate == default ? "-" : lastUpdate.ToString("yyyy-MM-dd HH:mm");
        
        return $"{truncatedText.PadRight(30)} | {statusStr,-10} | {dateStr}";
    }

    public string GetFullInfo()
    {
        string statusStr = GetStatusString(status);
        string dateStr = lastUpdate == default ? "-" : lastUpdate.ToString("yyyy-MM-dd HH:mm");
        
        return $"Текст: {text}\n" +
               $"Статус: {statusStr}\n" +
               $"Дата обновления: {dateStr}";
    }

    private string GetStatusString(TodoStatus status)
    {
        return status switch
        {
            TodoStatus.NotStarted => "Не начата",
            TodoStatus.InProgress => "В работе",
            TodoStatus.Completed => "Завершена",
            TodoStatus.Postponed => "Отложена",
            TodoStatus.Failed => "Провалена",
            _ => "Неизвестно"
        };
    }
}

