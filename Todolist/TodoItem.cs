using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    private Guid id;
    private string text;
    private TodoStatus status;
    private DateTime lastUpdate;
    private int sortOrder;
    private Guid profileId;

    [Key]
    public Guid Id
    {
        get { return id; }
        set { id = value == Guid.Empty ? Guid.NewGuid() : value; }
    }

    [Required]
    [MaxLength(2000)]
    public string Text
    {
        get { return text; }
        set { text = value ?? string.Empty; }
    }

    [Required]
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

    [Required]
    public int SortOrder
    {
        get { return sortOrder; }
        set { sortOrder = value < 0 ? 0 : value; }
    }

    [Required]
    public Guid ProfileId
    {
        get { return profileId; }
        set { profileId = value; }
    }

    public Profile? Profile { get; set; }

    [NotMapped]
    public bool IsCompleted => status == TodoStatus.Completed;

    public TodoItem()
    {
        id = Guid.NewGuid();
        text = string.Empty;
        status = TodoStatus.NotStarted;
        lastUpdate = DateTime.Now;
        sortOrder = 0;
        profileId = Guid.Empty;
    }

    public TodoItem(string? text)
    {
        id = Guid.NewGuid();
        this.text = text ?? string.Empty;
        status = TodoStatus.NotStarted;
        lastUpdate = DateTime.Now;
        sortOrder = 0;
        profileId = Guid.Empty;
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
