using System;

namespace TodoList
{
    public class TodoList
    {
        private TodoItem[] _items = new TodoItem[2];
        private int _count = 0;

        public int Count => _count;
        public bool IsEmpty() => _count == 0;

        private void IncreaseArray()
        {
            TodoItem[] newArr = new TodoItem[_items.Length * 2];
            for (int i = 0; i < _items.Length; i++)
                newArr[i] = _items[i];
            _items = newArr;
        }

        public void Add(TodoItem item)
        {
            if (_count == _items.Length)
                IncreaseArray();

            _items[_count++] = item;
        }

        public void Delete(int index)
        {
            if (index < 0 || index >= _count)
            {
                Console.WriteLine("Ошибка: неверный индекс");
                return;
            }

            for (int i = index; i < _count - 1; i++)
                _items[i] = _items[i + 1];

            _count--;
        }

        public TodoItem GetItem(int index)
        {
            if (index < 0 || index >= _count)
                throw new IndexOutOfRangeException("Неверный индекс");
            return _items[index];
        }

        public void View(bool showIndex, bool showDone, bool showDate)
        {
            if (_count == 0)
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

            for (int i = 0; i < _count; i++)
            {
                var item = _items[i];
                if (showIndex) Console.Write($"| {i + 1,-5} ");
                string taskText = item.Text.Length > 30 ? item.Text.Substring(0, 30) + "..." : item.Text;
                taskText = taskText.Replace("\n", " ");
                Console.Write($"| {taskText,-35} ");
                if (showDone)
                    Console.Write($"| {(item.IsDone ? "Сделано" : "Не сделано"),-12} ");
                if (showDate)
                    Console.Write($"| {item.LastUpdate:dd.MM.yyyy HH:mm:ss,-20} ");
                Console.WriteLine("|");
            }
        }
    }
}
