using System;

class TodoItem
{
    // Свойства
    public string Text { get; set; }
    public bool IsDone { get; set; }
    public DateTime LastUpdate { get; set; }

    // Конструктор
    public TodoItem(string text)
    {
        Text = text;
        IsDone = false;
        LastUpdate = DateTime.Now;
    }

    // Методы
    public void MarkDone()
    {
        IsDone = true;
        LastUpdate = DateTime.Now;
    }

    public void UpdateText(string newText)
    {
        Text = newText;
        LastUpdate = DateTime.Now;
    }

    public string GetShortInfo()
    {
        string truncatedText = Text ?? string.Empty;
        if (truncatedText.Length > 30)
        {
            truncatedText = truncatedText.Substring(0, 27) + "...";
        }
        
        string status = IsDone ? "сделано" : "не сделано";
        string dateStr = LastUpdate == default ? "-" : LastUpdate.ToString("yyyy-MM-dd HH:mm");
        
        return $"{truncatedText.PadRight(30)} | {status,-10} | {dateStr}";
    }

    public string GetFullInfo()
    {
        string status = IsDone ? "выполнена" : "не выполнена";
        string dateStr = LastUpdate == default ? "-" : LastUpdate.ToString("yyyy-MM-dd HH:mm");
        
        return $"Текст: {Text}\n" +
               $"Статус: {status}\n" +
               $"Дата последнего изменения: {dateStr}";
    }
}

