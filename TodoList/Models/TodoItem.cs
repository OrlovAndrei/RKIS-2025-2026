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
        private readonly IClock _clock;

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

        public TodoItem(string text, IClock? clock = null)
        {
            _clock = clock ?? new SystemClock();
            Text = text;
            Status = TodoStatus.NotStarted;
            LastUpdate = _clock.Now;
        }

        [JsonConstructor]
        public TodoItem(string text, TodoStatus status, DateTime lastUpdate)
        {
            Text = text;
            Status = status;
            LastUpdate = lastUpdate;
            _clock = new SystemClock(); 
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