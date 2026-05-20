using System;
using System.Collections.Generic;
using System.Text;

namespace TodoList
{
    /// <summary>
    /// Список задач с внутренним списком.
    /// </summary>
    internal class TodoList
    {
        private readonly List<TodoItem> _items = new List<TodoItem>();

        public int Count => _items.Count;

        // События для уведомления об изменениях
        public event Action<TodoItem>? OnTodoAdded;
        public event Action<TodoItem>? OnTodoDeleted;
        public event Action<TodoItem>? OnTodoUpdated;
        public event Action<TodoItem>? OnStatusChanged;

        /// <summary>
        /// Индексатор для доступа к задачам по индексу.
        /// </summary>
        /// <param name="index">Индекс задачи (0-based)</param>
        public TodoItem this[int index]
        {
            get
            {
                if (index < 0 || index >= _items.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return _items[index];
            }
        }

        public TodoList()
        {
        }

        public void Add(TodoItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            _items.Add(item);
            OnTodoAdded?.Invoke(item);
        }

        public void Delete(int index)
        {
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var item = _items[index];
            _items.RemoveAt(index);
            OnTodoDeleted?.Invoke(item);
        }

        public TodoItem GetItem(int index)
        {
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _items[index];
        }

        public void View(bool showIndex, bool showDone, bool showDate)
        {
            if (_items.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            Console.WriteLine("Задачи:");
            for (int i = 0; i < _items.Count; i++)
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
                    string status = item.Status.ToString();
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

        /// <summary>
        /// Устанавливает статус задачи по индексу.
        /// </summary>
        public void SetStatus(int index, TodoStatus status)
        {
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var item = _items[index];
            item.Status = status;
            OnStatusChanged?.Invoke(item);
        }

        /// <summary>
        /// Обновляет текст задачи по индексу.
        /// </summary>
        public void Update(int index, string newText)
        {
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var item = _items[index];
            item.UpdateText(newText);
            OnTodoUpdated?.Invoke(item);
        }

        /// <summary>
        /// Вставляет задачу по указанному индексу.
        /// </summary>
        public void Insert(int index, TodoItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (index < 0 || index > _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            _items.Insert(index, item);
            OnTodoAdded?.Invoke(item);
        }

        /// <summary>
        /// Позволяет перебирать задачи через foreach.
        /// </summary>
        public System.Collections.Generic.IEnumerable<TodoItem> GetItems()
        {
            foreach (var item in _items)
            {
                yield return item;
            }
        }
    }
}
