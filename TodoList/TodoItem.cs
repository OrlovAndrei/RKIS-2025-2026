using System;

namespace TodoList
{
    public class TodoItem
    {
        private string _text;
        private TodoStatus _status;
        private DateTime _lastUpdate;

        public string Text => _text;
        public TodoStatus Status => _status;
        public DateTime LastUpdate => _lastUpdate;

        public TodoItem(string text)
        {
            _text = text;
            _status = TodoStatus.NotStarted;
            _lastUpdate = DateTime.Now;
        }

        public TodoItem(string text, TodoStatus status, DateTime lastUpdate)
        {
            _text = text;
            _status = status;
            _lastUpdate = lastUpdate;
        }

        public void SetStatus(TodoStatus status)
        {
            _status = status;
            _lastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            _text = newText;
            _lastUpdate = DateTime.Now;
        }

        public string GetShortInfo()
        {
            string shortText = _text.Length > 30 ? _text.Substring(0, 27) + "..." : _text;
            return $"{shortText,-30} {_status,-15} {_lastUpdate:dd.MM.yyyy HH:mm}";
        }

        public string GetFullInfo()
        {
            return $"Полный текст: {_text}\nСтатус: {_status}\nДата изменения: {_lastUpdate:dd.MM.yyyy HH:mm}";
        }
    }
}