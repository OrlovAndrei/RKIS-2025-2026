using System;

public enum TodoStatus
{
    NotStarted,  // не начато
    InProgress,  // в процессе
    Completed,   // выполнено
    Postponed,   // отложено
    Failed       // провалено
}

class TodoItem
{
    private string text;
    private TodoStatus status;
    private DateTime lastUpdate;

    // Свойства
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

    // Конструктор
    public TodoItem(string text)
    {
        this.text = text;
        this.status = TodoStatus.NotStarted;
        this.lastUpdate = DateTime.Now;
    }

    // Методы
    public void MarkDone()
    {
        status = TodoStatus.Completed;
        lastUpdate = DateTime.Now;
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
               $"Дата последнего изменения: {dateStr}";
    }

    private string GetStatusString(TodoStatus status)
    {
        return status switch
        {
            TodoStatus.NotStarted => "не начато",
            TodoStatus.InProgress => "в процессе",
            TodoStatus.Completed => "выполнено",
            TodoStatus.Postponed => "отложено",
            TodoStatus.Failed => "провалено",
            _ => "неизвестно"
        };
    }
}

