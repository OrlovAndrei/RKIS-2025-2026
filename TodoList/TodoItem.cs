using System;

namespace Todolist
{
    public class TodoItem
    {
        // Приватные поля
        private string text;
        private bool isDone;
        private DateTime lastUpdate;

        // Публичные свойства только для чтения
        public string Text => text;
        public bool IsDone => isDone;
        public DateTime LastUpdate => lastUpdate;

        // Конструктор
        public TodoItem(string text, bool isDone = false, DateTime? lastUpdate = null)
        {
            this.text = text ?? "";
            this.isDone = isDone;
            this.lastUpdate = lastUpdate ?? DateTime.Now;
        }

        // Публичные методы для изменения состояния
        public void MarkDone()
        {
            isDone = true;
            lastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            if (!string.IsNullOrWhiteSpace(newText))
            {
                text = newText;
                lastUpdate = DateTime.Now;
            }
        }

        public string GetShortInfo()
        {
            string shortText = GetShortenedText(text, 30);
            string status = isDone ? "✓ Выполнена" : "□ Не выполнена";
            string date = lastUpdate.ToString("dd.MM.yyyy HH:mm");

            return $"{shortText} | {status} | {date}";
        }

        public string GetFullInfo()
        {
            return $"Текст: {text}\n" +
                   $"Статус: {(isDone ? "Выполнена ✓" : "Не выполнена □")}\n" +
                   $"Последнее изменение: {lastUpdate:dd.MM.yyyy HH:mm}";
        }

        // Приватный вспомогательный метод
        private string GetShortenedText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
        }
    }
}