namespace TodoList;

public static class FileManager
{
    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
    }

    public static void SaveProfile(Profile profile, string filePath)
    {
        var data = $"{profile.FirstName} {profile.LastName} {profile.BirthYear}";
        File.WriteAllText(filePath, data);
    }

    public static Profile LoadProfile(string filePath)
    {
        var parts = File.ReadAllText(filePath).Split(' ');
        return new Profile(parts[0], parts[1], int.Parse(parts[2]));
    }
    
    public static void SaveTodos(TodoList todoList, string filePath)
    {
        List<string> lines = [];

        for (var i = 0; i < todoList.todos.Count; i++)
        {
            var item = todoList.todos[i];
            var text = EscapeCsv(item.Text);
            lines.Add($"{i};{text};{item.Status};{item.LastUpdate:O}");
        }
        File.WriteAllLines(filePath, lines);
    }

    private static string EscapeCsv(string text)
    {
        return "\"" + text.Replace("\"", "\"\"").Replace("\n", "\\n") + "\"";
    }

    public static TodoList LoadTodos(string filePath)
    {
        TodoList list = new();

        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split(';');
            var text = UnescapeCsv(parts[1]);
            var status = Enum.Parse<TodoStatus>(parts[2]);
            var lastUpdate = DateTime.Parse(parts[3]);

            list.Add(new TodoItem(text, status, lastUpdate));
        }

        return list;
    }

    private static string UnescapeCsv(string text)
    {
        return text.Trim('"').Replace("\\n", "\n").Replace("\"\"", "\"");
    }
}