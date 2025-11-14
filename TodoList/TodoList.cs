using System;
using System.Collections.Generic;

namespace TodoList
{
    public class TodoList : IEnumerable<TodoItem>
    {
        private List<TodoItem> _items;

        public TodoList()
        {
            _items = new List<TodoItem>();
        }

        public void Add(TodoItem item)
        {
            _items.Add(item);
        }

        public void Delete(int index)
        {
            if (!IsValidIndex(index))
                throw new ArgumentException($"Неверный индекс: {index}");

            _items.RemoveAt(index - 1);
        }

        public void SetStatus(int index, TodoStatus status)
        {
            if (!IsValidIndex(index))
                throw new ArgumentException($"Неверный индекс: {index}");

            _items[index - 1].SetStatus(status);
        }

        public void View(bool showIndex = false, bool showStatus = false, bool showDate = false)
        {
            if (_items.Count == 0)
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

            for (int i = 0; i < _items.Count; i++)
            {
                var rowData = new List<string>();
                
                if (showIndex) rowData.Add((i + 1).ToString());
                if (showStatus) rowData.Add(GetStatusText(_items[i].Status));
                
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

        public TodoItem this[int index]
        {
            get
            {
                if (!IsValidIndex(index))
                    throw new ArgumentOutOfRangeException(nameof(index));
                return _items[index - 1];
            }
        }

        public int Count => _items.Count;

        private bool IsValidIndex(int index)
        {
            return index > 0 && index <= _items.Count;
        }

        private string GetStatusText(TodoStatus status)
        {
            return status switch
            {
                TodoStatus.NotStarted => "Не начато",
                TodoStatus.InProgress => "В процессе",
                TodoStatus.Completed => "Выполнено",
                TodoStatus.Postponed => "Отложено",
                TodoStatus.Failed => "Провалено",
                _ => "Неизвестно"
            };
        }

        public IEnumerator<TodoItem> GetEnumerator()
        {
            foreach (var item in _items)
            {
                yield return item;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}