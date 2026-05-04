using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models
{
    [Table("TodoItems")]
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public TodoStatus Status { get; set; } = TodoStatus.NotStarted;

        [Required]
        public DateTime LastUpdate { get; set; } = DateTime.Now;

        public int SortOrder { get; set; }

        public Guid ProfileId { get; set; }

        [ForeignKey(nameof(ProfileId))]
        public virtual Profile? Profile { get; set; }

        public TodoItem() { }

        public TodoItem(string text)
        {
            Text = text;
            Status = TodoStatus.NotStarted;
            LastUpdate = DateTime.Now;
        }

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

        public void SetStatus(TodoStatus status, bool updateTime = true)
        {
            Status = status;
            if (updateTime)
                LastUpdate = DateTime.Now;
        }

        public string GetShortInfo()
        {
            string taskText = Text.Replace("\n", " ");
            if (taskText.Length > 34)
                taskText = taskText[..30] + "... ";
            return taskText;
        }

        public string GetFullInfo() =>
            $"Текст: {Text}\nСтатус: {Status}\nДата последнего изменения: {LastUpdate:dd.MM.yyyy HH:mm}";
    }
}