namespace TodoList;

public class TodoItem
{
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
    public string Text { get; private set; }
    public TodoStatus Status { get; private set; }
    public DateTime LastUpdate { get; private set; }

    public void SetStatus(TodoStatus newStatus)
    {
        Status = newStatus;
        LastUpdate = DateTime.Now;
    }

    public void UpdateText(string newText)
    {
        Text = newText;
        LastUpdate = DateTime.Now;
    }

    public string GetShortInfo()
    {
        var text = Text.Replace("\r", " ").Replace("\n", " ");
        if (text.Length > 30) text = text[..30] + "...";

        var status = Status.ToString();
        return $"{text.PadRight(36)}|{status.PadRight(16)}|{LastUpdate.ToString("yyyy-MM-dd HH:mm").PadRight(16)}|";
    }

    public string GetFullInfo(int index)
    {
        var status = Status.ToString();

        return $"Индекс:{index}\nДата:{LastUpdate}\nНазвание:{Text}\nСтатус:{status}";
    }
}