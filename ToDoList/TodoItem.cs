using System;

namespace TodoList
{
    class TodoItem
    {
        private string text;
        private bool isDone;
        private DateTime lastUpdate;

        public string Text
        {
            get { return text; }
            private set { text = value; }
        }

        public bool IsDone
        {
            get { return isDone; }
            private set { isDone = value; }
        }

        public DateTime LastUpdate
        {
            get { return lastUpdate; }
            private set { lastUpdate = value; }
        }
        public TodoItem(string text)
        {
            this.text = text;
            this.isDone = false;
            this.lastUpdate = DateTime.Now;
        }
        public void MarkDone()
        {
            this.isDone = true;
            this.lastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            this.text = newText;
            this.lastUpdate = DateTime.Now;
        }

        public string GetShortInfo()
        {
            string shortText = text.Length > 30 ? text.Substring(0, 30) + "..." : text;
            string status = isDone ? "Сделано" : "Не сделано";
            string date = lastUpdate.ToString("dd.MM.yyyy HH:mm");
            return $"{shortText} | {status} | {date}";
        }
        public string GetFullInfo()
        {
            string statusText = isDone ? "выполнена" : "не выполнена";
            string dateText = lastUpdate.ToString("dd.MM.yyyy HH:mm");
            return $"Текст: {text}\nСтатус: {statusText}\nДата изменения: {dateText}";
        }
    }
}

