using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TodoList.Models
{
    public enum TodoStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Postponed,
        Failed
    }

    [Table("TodoItems")]
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        public TodoStatus Status { get; set; }

        public DateTime LastUpdate { get; set; }

        public Guid ProfileId { get; set; }

        [ForeignKey(nameof(ProfileId))]
        public Profile Profile { get; set; } = null!;

        public TodoItem() { }

        public TodoItem(string text)
        {
            Text = text;
            Status = TodoStatus.NotStarted;
            LastUpdate = DateTime.Now;
        }

        [JsonConstructor]
        public TodoItem(string text, TodoStatus status, DateTime lastUpdate)
        {
            Text = text;
            Status = status;
            LastUpdate = lastUpdate;
        }

        public void UpdateText(string newText)
        {
            Text = newText;
            LastUpdate = DateTime.Now;
        }

        public void SetStatus(TodoStatus newStatus)
        {
            Status = newStatus;
            LastUpdate = DateTime.Now;
        }

        public string GetShortInfo()
        {
            string status = $"[{Status}]";
            string shortText = Text.Length > 30 ? Text[..27] + "..." : Text;
            return $"{shortText} {status} ({LastUpdate:dd.MM.yyyy HH:mm})";
        }

        public string GetFullInfo()
        {
            return $"Текст: {Text}\nСтатус: {Status}\nДата последнего изменения: {LastUpdate:dd.MM.yyyy HH:mm}";
        }
    }
}