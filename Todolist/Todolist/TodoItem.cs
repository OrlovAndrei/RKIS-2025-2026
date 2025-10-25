namespace TodoList;

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

    public string GetShortInfo()
    {
        string text = Text.Replace("\r", " ").Replace("\n", " ");
        if (text.Length > 30) text = text[..30] + "...";

        string status = IsDone ? "выполнена" : "не выполнена";
        return $"{text.PadRight(36)}|{status.PadRight(16)}|{LastUpdate.ToString("yyyy-MM-dd HH:mm").PadRight(16)}|";
    }

    public string GetFullInfo(int index)
    {
        string status = IsDone ? "выполнена" : "не выполнена";
        return $"Индекс:{index}\nДата:{LastUpdate}\nНазвание:{Text}\nСтатус:{status}";
    }
}