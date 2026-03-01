using System;
using System.Collections;
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
            if (index >= 0 && index < _items.Count)
            {
                _items.RemoveAt(index);
            }
        }

        public void View(bool showIndex, bool showStatus, bool showDate)
        {
            string header = "";
            if (showIndex) header += "№       ";
            if (showStatus) header += "Статус           ";
            header += "Текст                          ";
            if (showDate) header += "Дата изменения";

            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            for (int i = 0; i < _items.Count; i++)
            {
                string row = "";

                if (showIndex)
                    row += $"{i + 1}       ".Substring(0, 8);

                if (showStatus)
                {
                    string status = _items[i].Status.ToString();
                    row += $"{status,-16}";
                }

                string taskText = _items[i].Text.Replace("\n", " ");
                if (taskText.Length > 31)
                    taskText = taskText.Substring(0, 27) + "...";
                row += taskText + new string(' ', 31 - taskText.Length);

                if (showDate)
                    row += _items[i].LastUpdate.ToString("dd.MM.yyyy HH:mm");

                Console.WriteLine(row);
            }
        }

        public void SetStatus(int index, TodoStatus status)
        {
            if (index >= 0 && index < _items.Count)
            {
                _items[index].SetStatus(status);
            }
        }

        public int GetCount()
        {
            return _items.Count;
        }

        public TodoItem GetItem(int index)
        {
            if (index >= 0 && index < _items.Count)
            {
                return _items[index];
            }
            return null;
        }

        public TodoItem this[int index]
        {
            get
            {
                if (index >= 0 && index < _items.Count)
                {
                    return _items[index];
                }
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public IEnumerator<TodoItem> GetEnumerator()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}