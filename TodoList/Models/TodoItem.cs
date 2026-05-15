using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Todos")]
public class TodoItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Text { get; set; } = "";

    public TodoStatus Status { get; set; }

    public DateTime LastUpdate { get; set; }

    public Guid ProfileId { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public Profile Profile { get; set; } = null!;

    [NotMapped]
    private readonly IClock? _clock;

    public TodoItem() { }

    public TodoItem(string text, IClock? clock = null)
    {
        _clock = clock;
        Text = text;
        Status = TodoStatus.NotStarted;
        LastUpdate = GetNow();
    }

    public void SetStatus(TodoStatus status)
    {
        Status = status;
        LastUpdate = GetNow();
    }

    public void UpdateText(string newText)
    {
        Text = newText;
        LastUpdate = GetNow();
    }

    public void SetLastUpdate(DateTime dateTime) => LastUpdate = dateTime;

    private DateTime GetNow() => (_clock ?? new SystemClock()).Now;

    public string GetStatusDisplay() => Status switch
    {
        TodoStatus.NotStarted => "Не начато",
        TodoStatus.InProgress => "В процессе",
        TodoStatus.Completed => "Выполнено",
        TodoStatus.Postponed => "Отложено",
        TodoStatus.Failed => "Провалено",
        _ => "Неизвестно"
    };

    public string GetShortInfo()
    {
        string shortText = GetShortText(Text, 30);
        string status = GetStatusDisplay();
        string date = LastUpdate.ToString("dd.MM.yyyy HH:mm");
        return $"{shortText,-30} {status,-10} {date}";
    }

    public string GetFullInfo() =>
        $"Текст: {Text}\nСтатус: {GetStatusDisplay()}\nДата изменения: {LastUpdate:dd.MM.yyyy HH:mm}";

    private static string GetShortText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text)) return "";
        return text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
    }
}
