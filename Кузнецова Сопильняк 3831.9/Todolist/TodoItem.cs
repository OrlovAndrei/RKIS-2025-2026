namespace TodoList
{
    public sealed  class TodoItem
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
            string shortText = Text;
            if (shortText == null) shortText = "";
            shortText = shortText.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
            if (shortText.Length > 30)
            {
                shortText = shortText.Substring(0, 27) + "...";
            }
            string status = IsDone ? "Сделано" : "Не сделано";
            string formattedDate = LastUpdate.ToString("dd.MM.yyyy HH:mm");
            return $"{shortText} | {status} | {formattedDate}";
        }

        public string GetFullInfo()
        {
            string status = IsDone ? "выполнена" : "не выполнена";
            string formattedDate = LastUpdate.ToString("dd.MM.yyyy HH:mm");
            return $"Текст: {Text}\nСтатус: {status}\nДата изменения: {formattedDate}";
        }
    }
}