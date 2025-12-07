using System;
using System.Collections;

namespace TodoList
{
    public class TodoList : IEnumerable<TodoItem>
    {
        private List<TodoItem> _items;
        public int Count => _items.Count;

        public TodoList()
        {
            _items = new List<TodoItem>();
        }
        public IEnumerator<TodoItem> GetEnumerator()
        {
            foreach (var item in _items)
            {
                yield return item;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public void Add(TodoItem item)
        {
            _items.Add(item);
        }

        public void Delete(int index)
        {
            if (index < 0 || index >= _items.Count)
            {
                Console.WriteLine("Ошибка: неверный индекс");
                return;
            }

            _items.RemoveAt(index);
        }

        public TodoItem this[int index]
        {
            get
            {
                if (index < 0 || index >= _items.Count)
                    throw new IndexOutOfRangeException("Неверный индекс");
                return _items[index];
            }
        }

        public void View(bool showIndex, bool showDone, bool showDate)
        {
            if (_items.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            int indexWidth = 5, textWidth = 35, statusWidth = 12, dateWidth = 20;

            if (showIndex) Console.Write("| {0,-5} ", "N");
            Console.Write("| {0,-35} ", "Задача");
            if (showDone) Console.Write("| {0,-12} ", "Статус");
            if (showDate) Console.Write("| {0,-20} ", "Дата");
            Console.WriteLine("|");

            if (showIndex) Console.Write($"+{new string('-', indexWidth + 2)}");
            Console.Write($"+{new string('-', textWidth + 2)}");
            if (showDone) Console.Write($"+{new string('-', statusWidth + 2)}");
            if (showDate) Console.Write($"+{new string('-', dateWidth + 2)}");
            Console.WriteLine("+");

            for (int i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                if (showIndex) Console.Write($"| {i + 1,-5} ");
                string taskText = item.Text.Length > 30 ? item.Text.Substring(0, 30) + "..." : item.Text;
                taskText = taskText.Replace("\n", " ");
                Console.Write($"| {taskText,-35} ");
                if (showDone)
                    Console.Write($"| {item.Status,-12} ");
                if (showDate)
                    Console.Write($"| {item.LastUpdate:dd.MM.yyyy HH:mm:ss,-20} ");
                Console.WriteLine("|");
            }
        }
    }
}
