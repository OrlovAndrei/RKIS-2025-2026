using System;
using System.Collections.Generic;

namespace TodoApp
{
    public class TodoList
    {
        // Приватное поле: список задач
        private List<TodoItem> items;

        // Конструктор
        public TodoList()
        {
            items = new List<TodoItem>();
        }

        // Индексатор для доступа к задачам по индексу
        public TodoItem this[int index]
        {
            get
            {
                if (index < 0 || index >= items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
                }
                return items[index];
            }
        }

        // Метод для добавления задачи
        public void Add(TodoItem item)
        {
            items.Add(item);
        }

        // Метод для удаления задачи по индексу
        public void Delete(int index)
        {
            if (index < 0 || index >= items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
            }
            items.RemoveAt(index);
        }

        // Метод для установки статуса задачи
        public void SetStatus(int index, TodoStatus status)
        {
            if (index < 0 || index >= items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
            }
            items[index].SetStatus(status);
        }

        // Метод для получения задачи по индексу
        public TodoItem GetItem(int index)
        {
            if (index < 0 || index >= items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
            }
            return items[index];
        }

        // Метод-итератор с использованием yield return
        public IEnumerable<TodoItem> GetItems()
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }

        // Метод для вывода задач в виде таблицы
        public void View(bool showIndex = true, bool showStatus = true, bool showDate = true)
        {
            if (items.Count == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            // Заголовок таблицы
            Console.WriteLine(new string('-', 80));
            string header = "";
            if (showIndex) header += "№".PadRight(5);
            header += "Задача".PadRight(35);
            if (showStatus) header += "Статус".PadRight(20);
            if (showDate) header += "Дата изменения";
            Console.WriteLine(header);
            Console.WriteLine(new string('-', 80));

            // Вывод задач
            int counter = 1;
            foreach (var item in items)
            {
                string row = "";
                
                if (showIndex)
                {
                    row += $"{counter}".PadRight(5);
                }

                // Обрезаем текст задачи до 30 символов и заменяем переносы строк
                string displayText = item.Text.Replace("\n", " ").Replace("\r", " ");
                string shortText = displayText.Length > 30 ? 
                    displayText.Substring(0, 30) + "..." : 
                    displayText;
                
                row += shortText.PadRight(35);

                if (showStatus)
                {
                    string status = item.GetStatusString();
                    row += status.PadRight(20);
                }

                if (showDate)
                {
                    row += item.LastUpdate.ToString("dd.MM.yyyy HH:mm");
                }

                Console.WriteLine(row);
                counter++;
            }
            Console.WriteLine(new string('-', 80));
        }

        // Свойство для получения количества задач
        public int Count => items.Count;

        // Свойство для проверки, пуст ли список
        public bool IsEmpty => items.Count == 0;
    }
}
