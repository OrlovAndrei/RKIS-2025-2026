using System;

namespace Todolist
{
    public class TodoList
    {
        // Приватное поле: массив задач
        private TodoItem[] items;
        private int count;

        // Конструктор
        public TodoList(int initialCapacity = 10)
        {
            items = new TodoItem[initialCapacity];
            count = 0;
        }

        // Свойства для получения информации
        public int Count => count;
        public int CompletedCount => GetCountByStatus(true);
        public int PendingCount => GetCountByStatus(false);

        // Методы

        // Добавить задачу
        public void Add(TodoItem item)
        {
            if (item == null) return;

            if (count >= items.Length)
            {
                IncreaseArray(items, item);
            }
            else
            {
                items[count] = item;
                count++;
            }
        }

        // Удалить задачу
        public void Delete(int index)
        {
            if (!IsValidIndex(index)) return;

            for (int i = index; i < count - 1; i++)
            {
                items[i] = items[i + 1];
            }

            items[count - 1] = null;
            count--;
        }

        // Вывод задач в виде таблицы
        public void View(bool showIndex, bool showDone, bool showDate)
        {
            if (count == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            // Определяем ширину колонок
            int indexWidth = showIndex ? 6 : 0;
            int statusWidth = showDone ? 10 : 0;
            int dateWidth = showDate ? 19 : 0;
            int textWidth = 32;

            // Строим таблицу
            string topBorder = "┌" + (showIndex ? new string('─', indexWidth) + "┬" : "") +
                             new string('─', textWidth) + "┬" +
                             (showDone ? new string('─', statusWidth) + "┬" : "") +
                             (showDate ? new string('─', dateWidth) + "┬" : "");
            Console.WriteLine(topBorder.TrimEnd('┬') + "┐");

            // Заголовок
            string header = "│" + (showIndex ? " №".PadRight(indexWidth - 1) + " │" : "") +
                          " Текст задачи".PadRight(textWidth - 1) + " │" +
                          (showDone ? " Статус".PadRight(statusWidth - 1) + " │" : "") +
                          (showDate ? " Дата изменения".PadRight(dateWidth - 1) + " │" : "");
            Console.WriteLine(header);

            // Разделитель
            string separator = "├" + (showIndex ? new string('─', indexWidth) + "┼" : "") +
                             new string('─', textWidth) + "┼" +
                             (showDone ? new string('─', statusWidth) + "┼" : "") +
                             (showDate ? new string('─', dateWidth) + "┼" : "");
            Console.WriteLine(separator.TrimEnd('┼') + "┤");

            // Данные
            for (int i = 0; i < count; i++)
            {
                string shortText = GetShortenedText(items[i].Text, 30);
                string status = items[i].IsDone ? "Выполнена" : "Не выполнена";
                string date = items[i].LastUpdate.ToString("dd.MM.yyyy HH:mm");

                string row = "│" + (showIndex ? $" {i + 1}".PadRight(indexWidth - 1) + " │" : "") +
                           $" {shortText}".PadRight(textWidth - 1) + " │" +
                           (showDone ? $" {status}".PadRight(statusWidth - 1) + " │" : "") +
                           (showDate ? $" {date}".PadRight(dateWidth - 1) + " │" : "");
                Console.WriteLine(row);
            }

            // Нижняя граница
            string bottomBorder = "└" + (showIndex ? new string('─', indexWidth) + "┴" : "") +
                                new string('─', textWidth) + "┴" +
                                (showDone ? new string('─', statusWidth) + "┴" : "") +
                                (showDate ? new string('─', dateWidth) + "┴" : "");
            Console.WriteLine(bottomBorder.TrimEnd('┴') + "┘");
        }

        // Получить задачу по индексу
        public TodoItem GetItem(int index)
        {
            return IsValidIndex(index) ? items[index] : null;
        }

        // Приватный метод для увеличения массива
        private void IncreaseArray(TodoItem[] oldArray, TodoItem newItem)
        {
            int newCapacity = oldArray.Length * 2;
            TodoItem[] newArray = new TodoItem[newCapacity];

            for (int i = 0; i < count; i++)
            {
                newArray[i] = oldArray[i];
            }

            newArray[count] = newItem;
            count++;

            items = newArray;
            
            Console.WriteLine($"Массив увеличен с {oldArray.Length} до {newCapacity} элементов");
        }

        // Вспомогательные методы
        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < count;
        }

        private int GetCountByStatus(bool isDone)
        {
            int result = 0;
            for (int i = 0; i < count; i++)
            {
                if (items[i].IsDone == isDone) result++;
            }
            return result;
        }

        private string GetShortenedText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
        }
    }
}