namespace TodoList;

public class FileManager
{
    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
            Console.WriteLine($"Создана директория: {dirPath}");
        }
    }
    public static void SaveProfile(Profile profile, string filePath)
    {
        try
        {
            string profileData = $"{profile.FirstName}|{profile.LastName}|{profile.BirthYear}";
            File.WriteAllText(filePath, profileData);
            Console.WriteLine("Профиль сохранен");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения профиля: {ex.Message}");
        }
    }
    public static Profile? LoadProfile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл профиля не найден");
                return null;
            }
            string line = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(line))
            {
                string[] parts = line.Split('|');
                if (parts.Length == 3)
                {
                    string firstName = parts[0];
                    string lastName = parts[1];
                    if (int.TryParse(parts[2], out int birthYear))
                    {
                        return new Profile(firstName, lastName, birthYear);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки профиля: {ex.Message}");
        }
        return null;
    }
    
    public static void SaveTodos(TodoList todos, string filePath)
    {
        var lines = new List<string> { "Index;Text;IsDone;LastUpdate" };

        for (int i = 0; i < todos.Count; i++)
        {
            try
            {
                TodoItem item = todos[i];
                string escapedText = EscapeCsvText(item.Text);
                string line = $"{i};\"{escapedText}\";{item.Status};{item.LastUpdate:yyyy-MM-ddTHH:mm:ss}";
                lines.Add(line);
            }
            catch (ArgumentOutOfRangeException)
            { }
        }
        Console.WriteLine("Заметки сохранены");
        File.WriteAllLines(filePath, lines);
    }

    public static TodoList LoadTodos(string filePath)
    {
        var todoList = new TodoList();

        if (!File.Exists(filePath))
            return todoList;

        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] parts = ParseCsvLine(lines[i]);

            if (parts.Length == 4)
            {
                string text = UnescapeCsvText(parts[1]);
                TodoStatus status = (TodoStatus)Enum.Parse(typeof(TodoStatus), parts[2]);
                DateTime lastUpdate = DateTime.Parse(parts[3]);

                todoList.Add(new TodoItem(text, status, lastUpdate));
            }
        }

        return todoList;
    }

    private static string EscapeCsvText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        return text.Replace("\n", "\\n").Replace("\r", "");
    }

    private static string UnescapeCsvText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        return text.Replace("\\n", "\n");
    }

    private static string[] ParseCsvLine(string line)
    {
        var parts = new List<string>();
        bool inQuotes = false;
        string currentPart = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ';' && !inQuotes)
            {
                parts.Add(currentPart);
                currentPart = "";
            }
            else
            {
                currentPart += c;
            }
        }

        parts.Add(currentPart);
        return parts.ToArray();
    }
}