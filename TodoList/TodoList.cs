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
                Console.WriteLine("Список задач пуст");
                return;
            }

            for (int i = 0; i < _count; i++)
            {
                string line = "";
                if (showIndex) line += $"{i + 1}. ";
                line += _items[i].Text;
                if (showDone) line += _items[i].IsDone ? " [✓]" : " [ ]";
                if (showDate) line += $" | {_items[i].LastUpdate:dd.MM.yyyy HH:mm:ss}";
                Console.WriteLine(line);
            }
        }
    }
}
