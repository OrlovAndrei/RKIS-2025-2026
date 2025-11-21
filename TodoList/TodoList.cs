using System;
using System.Collections.Generic;
using System.Collections;

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
        if (index < 0 || index >= _items.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
        }
        _items.RemoveAt(index);
    }

    public void View(bool showIndex, bool showStatus, bool showDate)
    {

    }

    public TodoItem GetItem(int index)
    {
        if (index < 0 || index >= _items.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
        }

        return _items[index];
    }

    public int Count => _items.Count;

    public IEnumerator<TodoItem> GetEnumerator()
    {
        foreach (var item in _items)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public TodoItem this[int index]
    {
        get
        {
            if (index < 0 || index >= _items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
            }
            return _items[index];
        }
    }
    private static string GetShortText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        if (text.Length <= maxLength)
            return text;

        return text.Substring(0, maxLength - 3) + "...";
    }

    public void SetStatus(int index, TodoStatus status)
    {
        if (index < 0 || index >= _items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        _items[index].SetStatus(status);
    }
}