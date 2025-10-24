using System;

namespace Todolist
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
            string shortText = Text.Length > 30 ? Text.Substring(0, 27) + "..." : Text;
            string status = IsDone ? "V Выполнена" : "X Не выполнена";
            string date = LastUpdate.ToString("dd.MM.yy HH:mm");

            return $"{shortText} [{status}] - {date}";
        }

        public string GetFullInfo()
        {
            string status = IsDone ? "V ВЫПОЛНЕНА" : "X НЕ ВЫПОЛНЕНА";
            return $"Текст: {Text}\nСтатус: {status}\nИзменено: {LastUpdate:dd.MM.yyyy HH:mm}";
        }
    }
}
