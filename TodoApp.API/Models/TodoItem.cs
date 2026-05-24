using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.API.Models;

public class TodoItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(500)]
    public string Text { get; set; } = string.Empty;

    [Required]
    public TodoStatus Status { get; set; } = TodoStatus.NotStarted;

    [Required]
    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

    public Guid ProfileId { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public Profile Profile { get; set; } = null!;

    public void UpdateText(string newText)
    {
        Text = newText;
        LastUpdate = DateTime.UtcNow;
    }

    public void SetStatus(TodoStatus newStatus)
    {
        Status = newStatus;
        LastUpdate = DateTime.UtcNow;
    }
}