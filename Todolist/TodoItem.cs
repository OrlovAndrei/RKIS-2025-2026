using System;

class TodoItem
{
    private string text;
    private bool isDone;
    private DateTime lastUpdate;

    // Свойства
    public string Text
    {
        get { return text; }
        set { text = value; }
    }

    public bool IsDone
    {
        get { return isDone; }
        set { isDone = value; }
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
        this.isDone = false;
        this.lastUpdate = DateTime.Now;
    }

    // Методы
    public void MarkDone()
    {
        isDone = true;
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
        
        string status = isDone ? "сделано" : "не сделано";
        string dateStr = lastUpdate == default ? "-" : lastUpdate.ToString("yyyy-MM-dd HH:mm");
        
        return $"{truncatedText.PadRight(30)} | {status,-10} | {dateStr}";
    }

    public string GetFullInfo()
    {
        string status = isDone ? "выполнена" : "не выполнена";
        string dateStr = lastUpdate == default ? "-" : lastUpdate.ToString("yyyy-MM-dd HH:mm");
        
        return $"Текст: {text}\n" +
               $"Статус: {status}\n" +
               $"Дата последнего изменения: {dateStr}";
    }
}

