using System;

namespace TodoList
{
    public class TodoItem
    {
        public string Text { get; private set; }
        public bool IsDone { get; private set; }
        public DateTime LastUpdate { get; private set; }

        public TodoItem(string text)
        {
            Text = text;
            IsDone = false;
            LastUpdate = DateTime.Now;
        }

        public void MarkDone()
        {
            IsDone = true;
            LastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            Text = newText;
            LastUpdate = DateTime.Now;
        }

        public string GetShortInfo()
        {
            string status = IsDone ? "[✓]" : "[ ]";
            string shortText = Text.Length > 30 ? Text.Substring(0, 27) + "..." : Text;
            return $"{shortText} {status} ({LastUpdate:dd.MM.yyyy HH:mm})";
        }

        public string GetFullInfo()
        {
            string status = IsDone ? "Выполнена" : "Не выполнена";
            return $"Текст: {Text}\nСтатус: {status}\nДата последнего изменения: {LastUpdate:dd.MM.yyyy HH:mm}";
        }
    }
}
