using System;
using System.Collections.Generic;
using System.Collections;

public class TodoList : IEnumerable<TodoItem>
{
    private List<TodoItem> _items = new();

    public event Action<TodoItem>? OnTodoAdded;
    public event Action<TodoItem>? OnTodoDeleted;
    public event Action<TodoItem>? OnTodoUpdated;
    public event Action<TodoItem>? OnStatusChanged;

    public void Add(TodoItem item)
    {
        _items.Add(item);
        OnTodoAdded?.Invoke(item);
    }

    public void Delete(int index)
    {
        if (index < 0 || index >= _items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        var deleted = _items[index];
        _items.RemoveAt(index);
        OnTodoDeleted?.Invoke(deleted);
    }

    public void SetStatus(int index, TodoStatus status)
    {
        if (index < 0 || index >= _items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        _items[index].SetStatus(status);
        OnStatusChanged?.Invoke(_items[index]);
    }

    public void UpdateText(int index, string newText)
    {
        if (index < 0 || index >= _items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        _items[index].UpdateText(newText);
        OnTodoUpdated?.Invoke(_items[index]);
    }

    public void View(bool showIndex, bool showStatus, bool showDate)
    {
        if (_items.Count == 0)
        {
            Console.WriteLine("Задач нет.");
            return;
        }

        if (!showIndex && !showStatus && !showDate)
        {
            Console.WriteLine("Список задач:");
            for (int i = 0; i < _items.Count; i++)
                Console.WriteLine($"{i + 1}. {GetShortText(_items[i].Text, 30)}");
            return;
        }

        Console.WriteLine("Список задач:");
        string header = "";
        if (showIndex) header += "№    ";
        header += "Текст задачи                     ";
        if (showStatus) header += "Статус      ";
        if (showDate) header += "Дата изменения    ";
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        for (int i = 0; i < _items.Count; i++)
        {
            string line = "";
            if (showIndex) line += $"{i + 1,-4} ";
            string cleanText = _items[i].Text.Replace("\n", " ").Replace("\r", "");
            line += $"{GetShortText(cleanText, 30),-30}";
            if (showStatus) line += $" {_items[i].GetStatusDisplay(),-10}";
            if (showDate) line += $" {_items[i].LastUpdate:dd.MM.yyyy HH:mm}";
            Console.WriteLine(line);
        }
    }

    public TodoItem GetItem(int index) => _items[index];
    public int Count => _items.Count;
    public IEnumerator<TodoItem> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public TodoItem this[int index] => _items[index];

    private static string GetShortText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text)) return "";
        return text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
    }
}