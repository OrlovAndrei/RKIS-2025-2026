using System;

public class TodoItem
{
    public string Text { get; private set; }
    public bool IsDone { get; private set; }
    public DateTime LastUpdate { get; private set; }

    public TodoItem(string text)
    {
        Text = text;
        IsDone = false;
        LastUpdate = DateTime.Now;
    }

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

    public void SetLastUpdate(DateTime dateTime)
    {
        LastUpdate = dateTime;
    }

    public string GetShortInfo()
    {
        string shortText = GetShortText(Text, 30);
        string status = IsDone ? "Сделано" : "Не сделано";
        string date = LastUpdate.ToString("dd.MM.yyyy HH:mm");

        return $"{shortText,-30} {status,-10} {date}";
    }

    public string GetFullInfo()
    {
        string status = IsDone ? "Выполнена" : "Не выполнена";
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