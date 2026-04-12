using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todolist.Models
{
    [Table("Todos")]
    public class TodoItem
    {
        private readonly IClock _clock;

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public TodoStatus Status { get; set; } = TodoStatus.NotStarted;

        [Required]
        public DateTime LastUpdate { get; set; }

        [Required]
        public Guid ProfileId { get; set; }

        [ForeignKey(nameof(ProfileId))]
        public virtual Profile Profile { get; set; } = null!;

        [NotMapped]
        public string ShortText => Text.Length > 30 ? Text.Substring(0, 30) + "..." : Text;

        public TodoItem() : this(null) { }

        public TodoItem(IClock? clock = null)
        {
            _clock = clock ?? new SystemClock();
            Status = TodoStatus.NotStarted;
            LastUpdate = _clock.Now;
        }

        public TodoItem(string text, IClock? clock = null) : this(clock)
        {
            Text = text;
        }

        public void UpdateText(string newText)
        {
            Text = newText;
            LastUpdate = _clock.Now;
        }

        public void SetStatus(TodoStatus newStatus)
        {
            Status = newStatus;
            LastUpdate = _clock.Now;
        }

        public string GetFullInfo()
        {
            return $"Текст: {Text}\nСтатус: {Status}\nПоследнее изменение: {LastUpdate:yyyy-MM-dd HH:mm:ss}";
        }
    }
}