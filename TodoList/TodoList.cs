using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TodoList;

public class TodoList : IEnumerable<TodoItem>
{
    private readonly List<TodoItem> _items = new();

    public event Action<TodoItem>? OnTodoAdded;
    public event Action<TodoItem>? OnTodoDeleted;
    public event Action<TodoItem>? OnTodoUpdated;
    public event Action<TodoItem>? OnStatusChanged;

    public int Count => _items.Count;

    public TodoItem this[int index]
    {
        get => _items[index];
        set => _items[index] = value;
    }

    public void Add(TodoItem item, bool silent = false)
    {
        _items.Add(item);
        if (!silent)
        {
            Console.WriteLine($"Задача добавлена: {item.Text}");
            OnTodoAdded?.Invoke(item);
        }
    }

    public void Delete(int idx)
    {
        if (idx < 0 || idx >= _items.Count)
        {
            Console.WriteLine("Ошибка: неверный индекс");
            return;
        }
        var item = _items[idx];
        _items.RemoveAt(idx);
        Console.WriteLine($"Задача {idx + 1} удалена.");
        OnTodoDeleted?.Invoke(item);
    }

    public void SetStatus(int idx, TodoStatus status)
    {
        if (idx < 0 || idx >= _items.Count)
        {
            Console.WriteLine("Ошибка: неверный индекс");
            return;
        }
        var item = _items[idx];
        item.SetStatus(status);
        Console.WriteLine($"Задача {idx + 1} получила статус {status}");
        OnStatusChanged?.Invoke(item);
    }

    public void Update(int idx, string newText)
    {
        if (idx < 0 || idx >= _items.Count)
        {
            Console.WriteLine("Ошибка: неверный индекс");
            return;
        }
        var item = _items[idx];
        item.UpdateText(newText);
        Console.WriteLine($"Задача {idx + 1} обновлена");
        OnTodoUpdated?.Invoke(item);
    }

    public void Read(int idx)
    {
        if (idx < 0 || idx >= _items.Count)
        {
            Console.WriteLine("Ошибка: неверный индекс");
            return;
        }
        Console.WriteLine(_items[idx].GetFullInfo());
    }

    public void View(bool showIndex, bool showStatus, bool showUpdateDate)
    {
        if (_items.Count == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        List<string> headers = new() { "Текст задачи".PadRight(36) };
        if (showIndex) headers.Add("Индекс".PadRight(8));
        if (showStatus) headers.Add("Статус".PadRight(16));
        if (showUpdateDate) headers.Add("Дата обновления".PadRight(16));

        Console.WriteLine("+-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-+");
        Console.WriteLine("| " + string.Join(" | ", headers) + " |");
        Console.WriteLine("|-" + string.Join("-+-", headers.Select(it => new string('-', it.Length))) + "-|");

        for (int i = 0; i < _items.Count; i++)
        {
            string text = _items[i].GetShortInfo().Replace("\n", " ");
            string status = _items[i].Status.ToString();
            string date = _items[i].LastUpdate.ToString("yyyy-MM-dd HH:mm");

            List<string> rows = new() { text.PadRight(36) };
            if (showIndex) rows.Add((i + 1).ToString().PadRight(8));
            if (showStatus) rows.Add(status.PadRight(16));
            if (showUpdateDate) rows.Add(date.PadRight(16));

            Console.WriteLine("| " + string.Join(" | ", rows) + " |");
        }
        Console.WriteLine("+-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-+");
    }

    public IEnumerator<TodoItem> GetEnumerator()
    {
        foreach (var item in _items)
            yield return item;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}