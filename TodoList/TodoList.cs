using System.Collections;
using System.Collections.Generic;

namespace TodoList
{
    public class TodoList : IEnumerable<TodoItem>
    {
        private List<TodoItem> _tasks;

        public TodoList()
        {
            _tasks = new List<TodoItem>();
        }
        
        public int Count => _tasks.Count;

        public void Add(TodoItem item)
        {
            _tasks.Add(item);
        }

        public void Insert(int index, TodoItem item)
        {
            if (index >= 0 && index <= _tasks.Count)
            {
                _tasks.Insert(index, item);
            }
        }

        public bool Delete(int index)
        {
            if (index < 1 || index > _tasks.Count) return false;
            _tasks.RemoveAt(index - 1);
            return true;
        }

        public TodoItem this[int index]
        {
            get
            {
                if (index < 0 || index >= _tasks.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return _tasks[index];
            }
        }

        public void SetStatus(int index, TodoStatus status)
        {
            if (index < 1 || index > _tasks.Count) return;
            _tasks[index - 1].SetStatus(status);
        }

        public void View(bool showIndex, bool showStatus, bool showDate)
        {
            if (_tasks.Count == 0)
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

            for (int i = 0; i < _tasks.Count; i++)
            {
                string row = "";
                if (showIndex) row += $"{i + 1,-6} | ";
                if (showStatus) row += $"{_tasks[i].Status,-12} | ";
                row += _tasks[i].Text.Replace("\n", " ");
                if (showDate) row += $" | {_tasks[i].LastUpdate:dd.MM.yyyy HH:mm}";
                Console.WriteLine(row);
            }
        }

        public IEnumerator<TodoItem> GetEnumerator()
        {
            foreach (var task in _tasks)
            {
                yield return task;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}