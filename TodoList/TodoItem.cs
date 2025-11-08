using System;
using TodoList.Commands;

namespace TodoList
{
    public class TodoItem
    {
        public string Text { get; private set; }
        public bool IsDone { get; private set; }
        public DateTime LastUpdate { get; private set; }

        public TodoItem(string text)
        {
            Text = text ?? string.Empty;
            IsDone = false;
            LastUpdate = DateTime.Now;
        }

        public TodoItem(string text, bool isDone, DateTime lastUpdate)
        {
            Text = text ?? string.Empty;
            IsDone = isDone;
            LastUpdate = lastUpdate;
        }

        public void MarkDone()
        {
            if (!IsDone)
            {
                IsDone = true;
                LastUpdate = DateTime.Now;
            }
        }

        public void UpdateText(string newText)
        {
            Text = newText ?? string.Empty;
            LastUpdate = DateTime.Now;
        }

        public string GetShortInfo(int maxLen = 30)
        {
            if (maxLen < 4) maxLen = 4;
            string clean = (Text ?? string.Empty).Replace("\r", " ").Replace("\n", " ").Trim();
            if (clean.Length <= maxLen) return clean;
            return clean.Substring(0, maxLen - 3) + "...";
        }
    }
}