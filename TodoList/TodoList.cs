using System;

namespace TodoList
{
    public class TodoList
    {
        private TodoItem[] _tasks;
        private int _count;

        public TodoList(int initialCapacity = 10)
        {
            _tasks = new TodoItem[initialCapacity];
            _count = 0;
        }

        public void Add(TodoItem item)
        {
            if (_count == _tasks.Length)
                _tasks = IncreaseArray(_tasks, item);
            else
                _tasks[_count++] = item;
        }

        public bool Delete(int index)
        {
            if (index < 1 || index > _count) return false;
            for (int i = index - 1; i < _count - 1; i++)
                _tasks[i] = _tasks[i + 1];
            _count--;
            return true;
        }

        public TodoItem GetItem(int index)
        {
            return (index >= 1 && index <= _count) ? _tasks[index - 1] : null;
        }

        public void View(bool showIndex, bool showStatus, bool showDate)
        {
            if (_count == 0)
            {
                Console.WriteLine("Нет задач.");
                return;
            }

            string header = "";
            if (showIndex) header += "Индекс | ";
            if (showStatus) header += "Статус | ";
            header += "Задача";
            if (showDate) header += " | Дата";
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            for (int i = 0; i < _count; i++)
            {
                string row = "";
                if (showIndex) row += $"{i + 1,-6} | ";
                if (showStatus) row += $"{(_tasks[i].IsDone ? "[✓]" : "[ ]"),-6} | ";
                row += _tasks[i].Text.Replace("\n", " ");
                if (showDate) row += $" | {_tasks[i].LastUpdate:dd.MM.yyyy HH:mm}";
                Console.WriteLine(row);
            }
        }

        private TodoItem[] IncreaseArray(TodoItem[] items, TodoItem newItem)
        {
            TodoItem[] newArray = new TodoItem[items.Length * 2];
            Array.Copy(items, newArray, _count);
            newArray[_count] = newItem;
            return newArray;
        }
    }
}
