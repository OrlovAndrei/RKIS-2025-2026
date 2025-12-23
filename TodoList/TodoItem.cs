using System;
using System.Text;

namespace TodoList
{
    public enum TodoStatus
    {
        NotStarted, // не начато
        InProgress, // в процессе
        Completed,  // выполнено
        Postponed,  // отложено
        Failed      // провалено
    }

    /// <summary>
    /// Одна задача в списке дел.
    /// </summary>
    internal class TodoItem
    {
        private string _text;
        private TodoStatus _status;
        private DateTime _lastUpdate;

        public string Text => _text;

        public TodoStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                _lastUpdate = DateTime.Now;
            }
        }

        public DateTime LastUpdate => _lastUpdate;

        public TodoItem(string text)
        {
            _text = string.IsNullOrWhiteSpace(text) ? string.Empty : text.Trim();
            _status = TodoStatus.NotStarted;
            _lastUpdate = DateTime.Now;
        }

        public TodoItem(string text, TodoStatus status, DateTime lastUpdate)
        {
            _text = string.IsNullOrWhiteSpace(text) ? string.Empty : text.Trim();
            _status = status;
            _lastUpdate = lastUpdate;
        }

        public void MarkDone()
        {
            Status = TodoStatus.Completed;
        }

        public void UpdateText(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
                return;

            _text = newText.Trim();
            _lastUpdate = DateTime.Now;
        }

        public string GetShortInfo()
        {
            string shortText = _text.Length <= 30 ? _text : _text[..30] + "...";
            string status = _status.ToString();
            return $"{shortText,-33} {status,-10} {_lastUpdate:yyyy-MM-dd HH:mm:ss}";
        }

        public string GetFullInfo()
        {
            var status = _status.ToString();
            var sb = new StringBuilder();
            sb.AppendLine($"Текст: {_text}");
            sb.AppendLine($"Статус: {status}");
            sb.AppendLine($"Последнее изменение: {_lastUpdate:yyyy-MM-dd HH:mm:ss}");
            return sb.ToString();
        }
    }
}
