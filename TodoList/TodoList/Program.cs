using System;

class Program
{
    const int INITIAL_SIZE = 10;

    static string[] todos = new string[INITIAL_SIZE];
    static bool[] statuses = new bool[INITIAL_SIZE];
    static DateTime[] dates = new DateTime[INITIAL_SIZE];

    static int count = 0;

    static void Main()
    {
        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            string[] parts = input.Split(' ', 2);

            string command = parts[0];

            switch (command)
            {
                case "add":
                    Add(parts.Length > 1 ? parts[1] : "");
                    break;

                case "view":
                    View();
                    break;

                case "done":
                    Done(GetIndex(parts));
                    break;

                case "delete":
                    Delete(GetIndex(parts));
                    break;

                case "update":
                    Update(parts);
                    break;

                default:
                    Console.WriteLine("Неизвестная команда.");
                    break;
            }
        }
    }

    // ---------- Методы команд ----------

    static void Add(string text)
    {
        EnsureSize();

        todos[count] = text;
        statuses[count] = false;
        dates[count] = DateTime.Now;
        count++;

        Console.WriteLine("Задача добавлена.");
    }

    static void View()
    {
        if (count == 0)
        {
            Console.WriteLine("Список пуст.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            string status = statuses[i] ? "сделано" : "не сделано";
            Console.WriteLine($"{i}: {todos[i]} — {status} — {dates[i]}");
        }
    }

    static void Done(int index)
    {
        if (!IsValidIndex(index)) return;

        statuses[index] = true;
        dates[index] = DateTime.Now;

        Console.WriteLine("Задача отмечена как выполненная.");
    }

    static void Delete(int index)
    {
        if (!IsValidIndex(index)) return;

        for (int i = index; i < count - 1; i++)
        {
            todos[i] = todos[i + 1];
            statuses[i] = statuses[i + 1];
            dates[i] = dates[i + 1];
        }

        count--;
        Console.WriteLine("Задача удалена.");
    }

    static void Update(string[] parts)
    {
        if (parts.Length < 2)
        {
            Console.WriteLine("Используйте: update <индекс> \"новый текст\"");
            return;
        }

        string[] commandParts = parts[1].Split(' ', 2);

        if (commandParts.Length < 2)
        {
            Console.WriteLine("Нужно указать индекс и текст.");
            return;
        }

        int index = int.Parse(commandParts[0]);
        string newText = commandParts[1];

        if (!IsValidIndex(index)) return;

        todos[index] = newText;
        dates[index] = DateTime.Now;

        Console.WriteLine("Задача обновлена.");
    }

    // ---------- Вспомогательные методы ----------

    static void EnsureSize()
    {
        if (count < todos.Length) return;

        int newSize = todos.Length * 2;

        Array.Resize(ref todos, newSize);
        Array.Resize(ref statuses, newSize);
        Array.Resize(ref dates, newSize);
    }

    static int GetIndex(string[] parts)
    {
        return (parts.Length < 2) ? -1 : int.Parse(parts[1]);
    }

    static bool IsValidIndex(int index)
    {
        if (index < 0 || index >= count)
        {
            Console.WriteLine("Неверный индекс!");
            return false;
        }
        return true;
    }
}
