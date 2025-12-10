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
            if (!string.IsNullOrEmpty(newText))
            {
                Text = newText;
                LastUpdate = DateTime.Now;
            }
        }

        public string GetShortInfo()
        {
            const int maxLen = 30;
            string shortText = Text.Length > maxLen
                               ? Text.Substring(0, maxLen - 3) + "..."
                               : Text;

            string statusText = IsDone ? "сделано" : "не сделано";
            string dateText = LastUpdate.ToString("yyyy-MM-dd HH:mm:ss");

            return $"{shortText} | {statusText} | {dateText}";
        }

        public string GetFullInfo()
        {
            string statusText = IsDone ? "Выполнена" : "Не выполнена";
            string dateText = LastUpdate.ToString("yyyy-MM-dd HH:mm:ss");

            return $"Полный текст задачи:\n\t{Text}\nСтатус: {statusText}\nДата последнего изменения: {dateText}";
        }
    }
}