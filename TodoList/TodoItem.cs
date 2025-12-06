using System;

namespace TodoList
{
    public class TodoItem
    {
        private string _text;
        private bool _isDone;
        private DateTime _lastUpdate;

        public string Text => _text;
        public bool IsDone => _isDone;
        public DateTime LastUpdate => _lastUpdate;

        public TodoItem(string text)
        {
            _text = text;
            _isDone = false;
            _lastUpdate = DateTime.Now;
        }
        public TodoItem(string text, bool isDone, DateTime lastUpdate)
        {
            _text = text;
            _isDone = isDone;
            _lastUpdate = lastUpdate;
        }

        public void MarkDone()
        {
            _isDone = true;
            _lastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            _text = newText;
            _lastUpdate = DateTime.Now;
        }

        public string GetShortInfo()
        {
            string shortText = _text.Length > 30 ? _text.Substring(0, 30) + "..." : _text;
            string status = _isDone ? "Выполнено" : "Не выполнено";
            return $"{shortText} | {status} | {_lastUpdate:dd.MM.yyyy HH:mm:ss}";
        }

        public string GetFullInfo()
        {
            string status = _isDone ? "Выполнено" : "Не выполнено";
            return $"Текст: {_text}\nСтатус: {status}\nПоследнее изменение: {_lastUpdate:dd.MM.yyyy HH:mm:ss}";
        }
    }
}