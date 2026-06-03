using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }

        public Guid ProfileId { get; set; }

        public Profile? Profile { get; set; }

        public TodoStatus Status { get; set; }

        public DateTime LastUpdate { get; set; }

        public TodoItem()
        {
            Text = string.Empty;
            Status = TodoStatus.NotStarted;
            LastUpdate = DateTime.Now;
        }

        public TodoItem(string text)
        {
            Text = text;
            Status = TodoStatus.NotStarted;
            LastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            Text = newText;
            LastUpdate = DateTime.Now;
        }

        public void SetStatus(TodoStatus status)
        {
            Status = status;
            LastUpdate = DateTime.Now;
        }
    }
}
