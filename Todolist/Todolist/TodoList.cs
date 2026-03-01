namespace TodoList;

public class TodoList
{
    public List<TodoItem> todos = [];

    public void Add(TodoItem item)
    {
        todos.Add(item);
        Console.WriteLine($"Добавлена задача: {todos.Count}) {item.Text}");
    }

    public void Delete(int index)
    {
        todos.RemoveAt(index);
        Console.WriteLine($"Удалена задача: {index + 1}");
    }

    public void SetStatus(int index, TodoStatus status)
    {
        todos[index].SetStatus(status);
        Console.WriteLine($"Задача {todos[index].Text} изменена на {status}");
    }

    public void Update(int index, string newText)
    {
        todos[index].UpdateText(newText);
        Console.WriteLine($"Задача №{index + 1} обновлена: {newText}");
    }

    public void Read(int index)
    {
        Console.WriteLine(todos[index].GetFullInfo(index));
    }

    public void View(bool showIndex, bool showStatus, bool showUpdateDate, bool showAll)
    {
        var indexWidth = 6;
        var textWidth = 36;
        var statusWidth = 14;
        var updateDateWidth = 16;

        List<string> headers = ["Текст задачи".PadRight(textWidth)];
        if (showIndex || showAll) headers.Add("Индекс".PadRight(indexWidth));
        if (showStatus || showAll) headers.Add("Статус".PadRight(statusWidth));
        if (showUpdateDate || showAll) headers.Add("Дата обновления".PadRight(updateDateWidth));

        Console.WriteLine("| " + string.Join(" | ", headers) + " |");
        Console.WriteLine("|-" + string.Join("-|-", headers.Select(it => new string('-', it.Length))) + "-|");

        for (var i = 0; i < todos.Count; i++)
        {
            var text = todos[i].Text.Replace("\n", " ");
            if (text.Length > 30) text = text.Substring(0, 30) + "...";

            var status = todos[i].Status.ToString();
            var date = todos[i].LastUpdate.ToString("yyyy-MM-dd HH:mm");

            List<string> rows = [text.PadRight(textWidth)];
            if (showIndex || showAll) rows.Add((i + 1).ToString().PadRight(indexWidth));
            if (showStatus || showAll) rows.Add(status.PadRight(statusWidth));
            if (showUpdateDate || showAll) rows.Add(date.PadRight(updateDateWidth));

            Console.WriteLine("| " + string.Join(" | ", rows) + " |");
        }
    }
}