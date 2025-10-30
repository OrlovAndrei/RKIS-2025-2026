using System;
using System.Collections.Generic;

namespace TodoList
{
    public class TodoList
    {
        private TodoItem[] _items;
        private int _count;

        public TodoList(int capacity = 10)
        {
            _items = new TodoItem[capacity];
            _count = 0;
        }

        public void Add(TodoItem item)
        {
            if (_count >= _items.Length)
            {
                IncreaseArray();
            }
            _items[_count++] = item;
        }

        public void Delete(int index)
        {
            if (!IsValidIndex(index))
                throw new ArgumentException($"Неверный индекс: {index}");

            int taskIndex = index - 1;
            for (int i = taskIndex; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }
            _items[_count - 1] = null;
            _count--;
        }

        public void View(bool showIndex = false, bool showStatus = false, bool showDate = false)
        {
            if (_count == 0)
            {
                Console.WriteLine("Задачи отсутствуют");
                return;
            }

            var headers = new List<string>();
            if (showIndex) headers.Add("№");
            if (showStatus) headers.Add("Статус");
            headers.Add("Задача");
            if (showDate) headers.Add("Дата изменения");

            string header = string.Join(" | ", headers);
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            for (int i = 0; i < _count; i++)
            {
                var rowData = new List<string>();
                
                if (showIndex) rowData.Add((i + 1).ToString());
                if (showStatus) rowData.Add(_items[i].IsDone ? "Сделано" : "Не сделано");
                
                string taskText = _items[i].Text;
                if (taskText.Length > 50)
                    taskText = taskText.Substring(0, 47) + "...";
                rowData.Add(taskText);
                
                if (showDate) rowData.Add(_items[i].LastUpdate.ToString("dd.MM.yyyy HH:mm"));

                Console.WriteLine(string.Join(" | ", rowData));
            }
        }

        public TodoItem GetItem(int index)
        {
            if (!IsValidIndex(index))
                throw new ArgumentException($"Неверный индекс: {index}");
            return _items[index - 1];
        }

        public int Count => _count;

        private void IncreaseArray()
        {
            int newSize = _items.Length * 2;
            TodoItem[] newItems = new TodoItem[newSize];
            Array.Copy(_items, newItems, _count);
            _items = newItems;
            Console.WriteLine($"Массив увеличен до {newSize} элементов");
        }

        private bool IsValidIndex(int index)
        {
            return index > 0 && index <= _count;
        }
    }
}