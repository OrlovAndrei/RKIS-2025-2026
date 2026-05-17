using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList;

public class TodoItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Text { get; set; } = string.Empty;

    [Required]
    public TodoStatus Status { get; set; }

    public DateTime LastUpdate { get; set; }

    public Guid ProfileId { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public virtual Profile Profile { get; set; } = null!;

    public TodoItem() { }

    public TodoItem(string text, Guid profileId)
    {
        Text = text;
        Status = TodoStatus.NotStarted;
        LastUpdate = DateTime.Now;
        ProfileId = profileId;
    }

    public TodoItem(string text, TodoStatus status, DateTime lastUpdate, Guid profileId)
    {
        Text = text;
        Status = status;
        LastUpdate = lastUpdate;
        ProfileId = profileId;
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

    public string GetFullInfo()
    {
        return $"Текст: {Text}\nСтатус: {Status}\nДата изменения: {LastUpdate:dd.MM.yyyy HH:mm}";
    }
}