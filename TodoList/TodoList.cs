using System;
using System.Collections;
using System.Collections.Generic;

namespace TodoList;

public class TodoList : IEnumerable<TodoItem>
{
    public List<TodoItem> items = [];

    // События
    public event Action<TodoItem>? OnTodoAdded;
    public event Action<TodoItem>? OnTodoDeleted;
    public event Action<TodoItem>? OnTodoUpdated;
    public event Action<TodoItem>? OnStatusChanged;

    public IEnumerator<TodoItem> GetEnumerator() => items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(TodoItem item)
    {
        items.Add(item);
        OnTodoAdded?.Invoke(item);
    }

    public void Insert(int index, TodoItem item)
    {
        items.Insert(index, item);
        OnTodoAdded?.Invoke(item);
    }

    public void Delete(int index)
    {
        var item = items[index];
        items.RemoveAt(index);
        OnTodoDeleted?.Invoke(item);
    }

    public void Remove(TodoItem item)
    {
        if (items.Remove(item))
        {
            OnTodoDeleted?.Invoke(item);
        }
    }

    public void NotifyItemUpdated(TodoItem item)
    {
        OnTodoUpdated?.Invoke(item);
    }

    public void NotifyStatusChanged(TodoItem item)
    {
        OnStatusChanged?.Invoke(item);
    }

    public void View(bool showIndex, bool showStatus, bool showDate)
    {
        var header = "";
        if (showIndex) header += "№".PadRight(6);
        if (showStatus) header += "Статус".PadRight(16);
        if (showDate) header += "Дата".PadRight(20);
        header += "Задача";

        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        for (var i = 0; i < items.Count; i++)
        {
            var line = "";
            if (showIndex) line += $"{i + 1}".PadRight(6);
            if (showStatus) line += $"{items[i].Status}".PadRight(16);
            if (showDate) line += $"{items[i].LastUpdate:dd.MM.yyyy HH:mm}".PadRight(20);

            var preview = items[i].Text.Length <= 30 ? items[i].Text : items[i].Text.Substring(0, 27) + "...";
            line += preview;

            Console.WriteLine(line);
        }
    }

    public TodoItem GetItem(int index)
    {
        if (index < 0 || index >= items.Count)
            throw new ArgumentOutOfRangeException("Неверный индекс задачи");
        return items[index];
    }
}