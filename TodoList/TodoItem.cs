using System;

namespace Todolist
{
    public class TodoItem
    {
        // Свойства
        public string Text { get; private set; }
        public bool IsDone { get; private set; }
        public DateTime LastUpdate { get; private set; }

        // Конструктор
        public TodoItem(string text, bool isDone = false, DateTime? lastUpdate = null)
        {
            Text = text ?? "";
            IsDone = isDone;
            LastUpdate = lastUpdate ?? DateTime.Now;
        }

        // Методы
        public void MarkDone()
        {
            IsDone = true;
            LastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            if (!string.IsNullOrWhiteSpace(newText))
            {
                Text = newText;
                LastUpdate = DateTime.Now;
            }
        }

        public string GetShortInfo()
        {
            string shortText = GetShortenedText(Text, 30);
            string status = IsDone ? "✓ Выполнена" : "□ Не выполнена";
            string date = LastUpdate.ToString("dd.MM.yyyy HH:mm");

            return $"{shortText} | {status} | {date}";
        }

        public string GetFullInfo()
        {
            return $"Текст: {Text}\n" +
                   $"Статус: {(IsDone ? "Выполнена ✓" : "Не выполнена □")}\n" +
                   $"Последнее изменение: {LastUpdate:dd.MM.yyyy HH:mm}";
        }

        // Вспомогательный метод для сокращения текста
        private string GetShortenedText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
        }
    }
}