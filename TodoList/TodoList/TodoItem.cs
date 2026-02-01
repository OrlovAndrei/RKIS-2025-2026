using System;

class TodoItem
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
        string shortText = Text.Length > 30 ? Text.Substring(0, 30) + "..." : Text;
        return $"{shortText} | {(IsDone ? "✔" : "✘")} | {LastUpdate}";
    }

    public string GetFullInfo()
    {
        return $"{Text}\nСтатус: {(IsDone ? "выполнена" : "не выполнена")}\nДата: {LastUpdate}";
    }
}