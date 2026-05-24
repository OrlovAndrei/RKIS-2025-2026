using System;
using System.Collections;
using System.Collections.Generic;
using Todolist.Models;

namespace Todolist
{
    public class TodoList : IEnumerable<TodoItem>
    {
        private List<TodoItem> items;
        private int capacity;
        private const int DefaultCapacity = 10;
        private const double GrowthFactor = 1.5;

        public TodoList()
        {
            capacity = DefaultCapacity;
            items = new List<TodoItem>(capacity);
        }

        public TodoList(int initialCapacity)
        {
            if (initialCapacity <= 0)
                throw new ArgumentException("Начальная ёмкость должна быть положительной");
            capacity = initialCapacity;
            items = new List<TodoItem>(capacity);
        }

        public int Count => items.Count;
        public int Capacity => capacity;

        public void Add(TodoItem item)
        {
            items.Add(item);
            if (items.Count > capacity)
            {
                capacity = (int)(capacity * GrowthFactor);
                items.Capacity = capacity;
            }
        }

        public bool Remove(int index)
        {
            if (index < 0 || index >= items.Count)
                return false;
            items.RemoveAt(index);
            return true;
        }

        public TodoItem Get(int index)
        {
            if (index < 0 || index >= items.Count)
                throw new IndexOutOfRangeException("Индекс вне диапазона");
            return items[index];
        }

        public void Update(int index, TodoItem newItem)
        {
            if (index < 0 || index >= items.Count)
                throw new IndexOutOfRangeException("Индекс вне диапазона");
            items[index] = newItem;
        }

        public void Clear() => items.Clear();

        public IEnumerator<TodoItem> GetEnumerator() => items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}