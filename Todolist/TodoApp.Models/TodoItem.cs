using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models;

public class TodoItem
{
    public TodoItem()
    {
        Id = Guid.NewGuid();
        Text = string.Empty;
        Status = TodoStatus.NotStarted;
        LastUpdate = DateTime.Now;
    }

    public TodoItem(string text)
        : this()
    {
        Text = text ?? string.Empty;
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Text { get; set; }

    [Required]
    public TodoStatus Status { get; set; }

    public DateTime LastUpdate { get; set; }

    [Required]
    public int SortOrder { get; set; }

    [Required]
    public Guid ProfileId { get; set; }

    public Profile? Profile { get; set; }

    [NotMapped]
    public bool IsCompleted => Status == TodoStatus.Completed;

    public void UpdateText(string newText)
    {
        Text = newText ?? string.Empty;
        LastUpdate = DateTime.Now;
    }
}
