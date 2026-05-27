using System;

class TodoList
{
    private TodoItem[] items = new TodoItem[5];
    private int count = 0;

    public void Add(TodoItem item)
    {
        if (count == items.Length)
            IncreaseArray();

        items[count++] = item;
        Console.WriteLine("Задача добавлена");
    }

    public TodoItem? GetItem(int index)
    {
        if (index < 0 || index >= count)
        {
            Console.WriteLine("Неверный индекс");
            return null;
        }
        return items[index];
    }

    public void View(bool showIndex, bool showDone, bool showDate)
    {
        for (int i = 0; i < count; i++)
        {
            if (showIndex) Console.Write($"{i}: ");
            Console.WriteLine(items[i].GetShortInfo());
        }
    }

    private void IncreaseArray()
    {
        TodoItem[] newArr = new TodoItem[items.Length * 2];
        Array.Copy(items, newArr, items.Length);
        items = newArr;
    }
}