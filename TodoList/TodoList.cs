using System;
using System.Collections.Generic;
public class TodoList
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
        if (_items.Count == 0)
        {
            Console.WriteLine("Задач нет.");
            return;
        }

        if (!showIndex && !showStatus && !showDate)
        {
            Console.WriteLine("Список задач:");
            for (int i = 0; i < _items.Count; i++)
            {
                string shortText = GetShortText(_items[i].Text, 30);
                Console.WriteLine($"{i + 1}. {shortText}");
            }
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

            if (showIndex)
                line += $"{i + 1,-4} ";

            string cleanText = _items[i].Text.Replace("\n", " ").Replace("\r", "");
            string shortText = GetShortText(cleanText, 30);
            line += $"{shortText,-30}";

            if (showStatus)
            {
                string status = StatusToString(_items[i].Status);
                line += $" {status,-10}";
            }

            if (showDate)
            {
                string date = _items[i].LastUpdate.ToString("dd.MM.yyyy HH:mm");
                line += $" {date}";
            }

            Console.WriteLine(line);
        }
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
    public System.Collections.Generic.IEnumerator<TodoItem> GetEnumerator()
    {
        foreach (var item in _items)
        {
            yield return item;
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
    private static string StatusToString(TodoStatus status)
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
}