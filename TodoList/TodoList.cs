using System;

public class TodoList
{
    private TodoItem[] _items;
    private int _count;

    public TodoList()
    {
        _items = new TodoItem[2];
        _count = 0;
    }

    public void Add(TodoItem item)
    {
        if (_count >= _items.Length)
        {
            IncreaseArray();
        }
        
        _items[_count] = item;
        _count++;
    }

    public void Delete(int index)
    {
        if (index < 0 || index >= _count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
        }

        for (int i = index; i < _count - 1; i++)
        {
            _items[i] = _items[i + 1];
        }
        
        _count--;
        _items[_count] = null;
    }

    public void View(bool showIndex, bool showStatus, bool showDate)
    {
        if (_count == 0)
        {
            Console.WriteLine("Задач нет.");
            return;
        }

        if (!showIndex && !showStatus && !showDate)
        {
            Console.WriteLine("Список задач:");
            for (int i = 0; i < _count; i++)
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
        
        for (int i = 0; i < _count; i++)
        {
            string line = "";
            
            if (showIndex)
                line += $"{i + 1,-4} ";
                
            string shortText = GetShortText(_items[i].Text, 30);
            line += $"{shortText,-30}";
            
            if (showStatus)
            {
                string status = _items[i].IsDone ? "Сделано" : "Не сделано";
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
        if (index < 0 || index >= _count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
        }
        
        return _items[index];
    }

    public int Count => _count;

    private void IncreaseArray()
    {
        int newSize = _items.Length * 2;
        TodoItem[] newArray = new TodoItem[newSize];
        Array.Copy(_items, newArray, _count);
        _items = newArray;
    }

    private static string GetShortText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text))
            return "";
            
        if (text.Length <= maxLength)
            return text;

        return text.Substring(0, maxLength - 3) + "...";
    }
}