using System;
using System.Text;

namespace TodoList
{
    /// <summary>
    /// Одна задача в списке дел.
    /// </summary>
    internal class TodoItem
    {
        private string _text;
        private bool _isDone;
        private DateTime _lastUpdate;

        public string Text => _text;

        public bool IsDone => _isDone;

        public DateTime LastUpdate => _lastUpdate;

        public TodoItem(string text)
        {
            _text = string.IsNullOrWhiteSpace(text) ? string.Empty : text.Trim();
            _isDone = false;
            _lastUpdate = DateTime.Now;
        }

        public void MarkDone()
        {
            _isDone = true;
            _lastUpdate = DateTime.Now;
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
            string status = _isDone ? "сделано" : "не сделано";
            return $"{shortText,-33} {status,-10} {_lastUpdate:yyyy-MM-dd HH:mm:ss}";
        }

        public string GetFullInfo()
        {
            var status = _isDone ? "сделано" : "не сделано";
            var sb = new StringBuilder();
            sb.AppendLine($"Текст: {_text}");
            sb.AppendLine($"Статус: {status}");
            sb.AppendLine($"Последнее изменение: {_lastUpdate:yyyy-MM-dd HH:mm:ss}");
            return sb.ToString();
        }
    }
}
