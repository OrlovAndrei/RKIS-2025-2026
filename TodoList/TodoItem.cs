using System;

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

    public void SetLastUpdate(DateTime dateTime)
    {
        LastUpdate = dateTime;
    }
    public string GetStatusDisplay()
    {
        return Status switch
        {
            TodoStatus.NotStarted => "Не начато",
            TodoStatus.InProgress => "В процессе",
            TodoStatus.Completed => "Выполнено",
            TodoStatus.Postponed => "Отложено",
            TodoStatus.Failed => "Провалено",
            _ => "Неизвестно"
        };
    }
    public string GetShortInfo()
    {
        string shortText = GetShortText(Text, 30);
        string status = GetStatusDisplay();
        string date = LastUpdate.ToString("dd.MM.yyyy HH:mm");

        return $"{shortText,-30} {status,-10} {date}";
    }

    public string GetFullInfo()
    {
        string status = GetStatusDisplay();
        string date = LastUpdate.ToString("dd.MM.yyyy HH:mm");

        return $"Текст: {Text}\nСтатус: {status}\nДата изменения: {date}";
    }

    private static string GetShortText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        if (text.Length <= maxLength)
            return text;

        return text.Substring(0, maxLength - 3) + "...";
    }
}