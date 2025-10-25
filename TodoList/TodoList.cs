namespace TodoList;

public class TodoList
{
    private TodoItem[] todos = new TodoItem[2];
    private int taskCount;

    public void Add(TodoItem item)
    {
        if (taskCount == todos.Length)
            IncreaseArray();

        todos[taskCount] = item;
        taskCount++;

        Console.WriteLine($"Добавлена задача: {taskCount}) {item.Text}");
    }

    public void Delete(int idx)
    {
        for (var i = idx; i < taskCount - 1; i++)
        {
            todos[i] = todos[i + 1];
        }

        taskCount--;
        Console.WriteLine($"Задача под номером {idx + 1} была удалена.");
    }

    public void MarkDone(int idx)
    {
        todos[idx].MarkDone();
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

        for (int i = 0; i < taskCount; i++)
        {
	        string text = todos[i].Text;
	        if (text.Length > 30) text = text.Substring(0, 30) + "...";

	        string status = todos[i].IsDone ? "выполнена" : "не выполнена";
	        string date = todos[i].LastUpdate.ToString("yyyy-MM-dd HH:mm");

	        string row = "";
	        if (hasAll || hasidx) row += string.Format("{0,-" + idxWidth + "}|", i + 1);
	        row += string.Format("{0,-" + textWidth + "}|", text);
	        if (hasAll || hasStatus) row += string.Format("{0,-" + statusWidth + "}|", status);
	        if (hasAll || hasDate) row += string.Format("{0,-" + dateWidth + "}|", date);

	        Console.WriteLine(row);
        }
    }

    private void IncreaseArray()
    {
        var newSize = todos.Length * 2;
        Array.Resize(ref todos, newSize);
    }
}