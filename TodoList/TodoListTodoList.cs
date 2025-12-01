using System;
using System.Text;

namespace TodoList
{
    /// <summary>
    /// Список задач с внутренним динамическим массивом.
    /// </summary>
    internal class TodoList
    {
        private const int InitialCapacity = 2;

        private TodoItem[] _items;
        private int _count;

        public int Count => _count;

        public TodoList()
        {
            _items = new TodoItem[InitialCapacity];
            _count = 0;
        }

        public void Add(TodoItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (_count == _items.Length)
            {
                _items = IncreaseArray(_items, item);
                return;
            }

            _items[_count] = item;
            _count++;
        }

        public void Delete(int index)
        {
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException(nameof(index));

            for (int i = index; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }

            _items[_count - 1] = null!;
            _count--;
        }

        public TodoItem GetItem(int index)
        {
            if (index < 0 || index >= _count)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _items[index];
        }

        public void View(bool showIndex, bool showDone, bool showDate)
        {
            if (_count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            Console.WriteLine("Задачи:");
            for (int i = 0; i < _count; i++)
            {
                var item = _items[i];
                if (item == null) continue;

                var line = new StringBuilder();

                if (showIndex)
                {
                    line.AppendFormat("{0,3}. ", i + 1);
                }

                string textShort = item.Text.Length <= 30
                    ? item.Text
                    : item.Text[..30] + "...";
                line.Append(textShort);

                if (showDone)
                {
                    string status = item.IsDone ? "сделано" : "не сделано";
                    line.Append("  ");
                    line.Append(status);
                }

                if (showDate)
                {
                    line.Append("  ");
                    line.Append(item.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                Console.WriteLine(line.ToString());
            }
        }

        private TodoItem[] IncreaseArray(TodoItem[] items, TodoItem item)
        {
            int currentLength = items.Length;
            int newLength = currentLength == 0 ? InitialCapacity : currentLength * 2;

            var newArray = new TodoItem[newLength];
            for (int i = 0; i < currentLength; i++)
            {
                newArray[i] = items[i];
            }

            newArray[currentLength] = item;
            _count++;

            return newArray;
        }
    }
}
