using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Todolist.Models;

namespace Todolist.Services
{
    public class TodoListService : IEnumerable<TodoItem>
    {
        private readonly Guid _userId;
        private readonly List<TodoItem> _items;
        private readonly TodoRepository _todoRepository;

        public TodoListService(Guid userId)
        {
            _userId = userId;
            _todoRepository = new TodoRepository();
            _items = _todoRepository.GetAllByProfileId(userId);
        }

        public int GetCount() => _items.Count;

        public TodoItem GetItem(int index) => _items[index];

        public void Add(TodoItem item)
        {
            if (item.ProfileId == Guid.Empty)
                item.ProfileId = _userId;

            _todoRepository.Add(item);
            _items.Add(item);
        }

        public void Delete(int index)
        {
            var item = _items[index];
            _todoRepository.Delete(item.Id);
            _items.RemoveAt(index);
        }

        public void Insert(TodoItem item, int index)
        {
            item.Id = 0;
            _todoRepository.Add(item);
            _items.Insert(index, item);
        }

        public void SetStatus(int index, TodoStatus status, bool updateTime = true)
        {
            var item = _items[index];
            item.SetStatus(status);
            _todoRepository.Update(item);
        }

        public void Refresh()
        {
            _items.Clear();
            _items.AddRange(_todoRepository.GetAllByProfileId(_userId));
        }

        public List<TodoItem> ToList() => _items.ToList();

        public void View(bool showIndex, bool showStatus, bool showDate)
        {
            if (_items.Count == 0)
            {
                Console.WriteLine("Задачи отсутствуют.");
                return;
            }

            for (int i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                string output = "";

                if (showIndex)
                    output += $"[{i + 1}] ";

                string shortText = item.Text.Replace("\n", " ");
                if (shortText.Length > 30)
                    shortText = shortText[..30] + "...";

                output += shortText;

                if (showStatus)
                    output += $" [{item.Status}]";

                if (showDate)
                    output += $" {item.LastUpdate:yyyy-MM-dd}";

                Console.WriteLine(output);
            }
        }

        public IEnumerator<TodoItem> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}