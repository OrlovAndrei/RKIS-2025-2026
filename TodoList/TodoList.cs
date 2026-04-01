namespace TodoList;

public class TodoList
{
    public List<TodoItem> todos = [];

    public void Add(TodoItem item)
    {
	    todos.Add(item);
    }

    public void Delete(int idx)
    {
	    todos.RemoveAt(idx);
	    Console.WriteLine($"Задача под номером {idx + 1} была удалена.");
    }
    
    public void SetStatus(int idx, TodoStatus status)
    {
        todos[idx].SetStatus(status);
        Console.WriteLine($"Задача под номером {idx + 1} отмечена выполненной.");
    }

    public void Update(int idx, string newText)
    {
        todos[idx].UpdateText(newText);
        Console.WriteLine($"Задача под номером {idx + 1} была обновлена.");
    }

    public void Read(int idx)
    {
        Console.WriteLine(todos[idx].GetFullInfo(idx));
    }

    public void View(bool hasidx, bool hasStatus, bool hasDate, bool hasAll)
    {
	    int idxWidth = 7;
	    int textWidth = 33;
	    int statusWidth = 12;
	    int dateWidth = 19;

	    string headerRow = "";
	    if (hasAll || hasidx) headerRow += string.Format("{0,-" + idxWidth + "}|", "Индекс");
	    headerRow += string.Format("{0,-" + textWidth + "}|", "Текст задачи");
	    if (hasAll || hasStatus) headerRow += string.Format("{0,-" + statusWidth + "}|", "Статус");
	    if (hasAll || hasDate) headerRow += string.Format("{0,-" + dateWidth + "}|", "Дата обновления");

        Console.WriteLine(headerRow);
        Console.WriteLine(new string('-', headerRow.Length));

        for (int i = 0; i < todos.Count; i++)
        {
	        string text = todos[i].Text.Replace("\n", " ");
	        if (text.Length > 30) text = text.Substring(0, 30) + "...";

	        string status = todos[i].Status.ToString();
	        string date = todos[i].LastUpdate.ToString("yyyy-MM-dd HH:mm");

	        string row = "";
	        if (hasAll || hasidx) row += string.Format("{0,-" + idxWidth + "}|", i + 1);
	        row += string.Format("{0,-" + textWidth + "}|", text);
	        if (hasAll || hasStatus) row += string.Format("{0,-" + statusWidth + "}|", status);
	        if (hasAll || hasDate) row += string.Format("{0,-" + dateWidth + "}|", date);

	        Console.WriteLine(row);
        }
    }
}