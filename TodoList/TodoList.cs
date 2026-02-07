using System.Collections;
using System.Collections.Generic;

namespace TodoList
{
    public class TodoList : IEnumerable<TodoItem>
    {
        private List<TodoItem> _tasks;

        // События
        public event Action<TodoItem>? OnTodoAdded;
        public event Action<TodoItem>? OnTodoDeleted;
        public event Action<TodoItem>? OnTodoUpdated;
        public event Action<TodoItem>? OnStatusChanged;

        public TodoList()
        {
            _tasks = new List<TodoItem>();
        }
        
        public int Count => _tasks.Count;

        public void Add(TodoItem item)
        {
            _tasks.Add(item);
            OnTodoAdded?.Invoke(item);
        }

        public void Insert(int index, TodoItem item)
        {
            if (index >= 0 && index <= _tasks.Count)
            {
                _tasks.Insert(index, item);
                OnTodoAdded?.Invoke(item);
            }
        }

        public bool Delete(int index)
        {
            if (index < 1 || index > _tasks.Count) 
                return false;
            
            var item = _tasks[index - 1];
            _tasks.RemoveAt(index - 1);
            OnTodoDeleted?.Invoke(item);
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
            if (index < 1 || index > _tasks.Count) 
                return;
            
            var item = _tasks[index - 1];
            item.SetStatus(status);
            OnStatusChanged?.Invoke(item);
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

        public void UpdateText(int index, string newText)
        {
            if (index < 1 || index > _tasks.Count) 
                return;
            
            var item = _tasks[index - 1];
            item.UpdateText(newText);
            OnTodoUpdated?.Invoke(item);
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