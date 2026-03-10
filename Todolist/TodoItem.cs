using System;

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

    public TodoItem(string? text)
    {
        this.text = text ?? string.Empty;
        this.status = TodoStatus.NotStarted;
        this.lastUpdate = DateTime.Now;
    }

    public void UpdateText(string? newText)
    {
        text = newText ?? string.Empty;
        lastUpdate = DateTime.Now;
    }

    public string GetShortInfo()
    {
        string truncatedText = text ?? string.Empty;
        if (truncatedText.Length > 30)
        {
            truncatedText = truncatedText.Substring(0, 27) + "...";
        }

        string statusStr = TodoStatusHelper.ToDisplayString(status);
        string dateStr = lastUpdate == default ? "-" : lastUpdate.ToString("yyyy-MM-dd HH:mm");

        return $"{truncatedText.PadRight(30)} | {statusStr,-10} | {dateStr}";
    }

    public string GetFullInfo()
    {
        string statusStr = TodoStatusHelper.ToDisplayString(status);
        string dateStr = lastUpdate == default ? "-" : lastUpdate.ToString("yyyy-MM-dd HH:mm");

        return $"Текст: {text ?? string.Empty}\n" +
               $"Статус: {statusStr}\n" +
               $"Дата обновления: {dateStr}";
    }
}
