using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
public static class FileManager
{
    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }

    public static void SaveProfile(List<Profile> profiles, string filePath)
    {
        var lines = new List<string> { "Id;Login;Password;FirstName;LastName;BirthYear" };

        foreach (var profile in profiles)
        {
            string line = $"{profile.Id};{profile.Login};{profile.Password};{profile.FirstName};{profile.LastName};{profile.BirthYear}";
            lines.Add(line);
        }

        File.WriteAllLines(filePath, lines);
    }
    public static List<Profile> LoadProfile(string filePath)
    {
        var profiles = new List<Profile>();

        if (!File.Exists(filePath))
            return profiles;

        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] parts = lines[i].Split(';');

            if (parts.Length == 6)
            {
                Guid id = Guid.Parse(parts[0]);
                string login = parts[1];
                string password = parts[2];
                string firstName = parts[3];
                string lastName = parts[4];
                int birthYear = int.Parse(parts[5]);

                profiles.Add(new Profile(id, login, password, firstName, lastName, birthYear));
            }
        }
        return null;
    }

    public static void SaveTodos(TodoList todos, string filePath)
    {
        var lines = new List<string> { "Index;Text;Status;LastUpdate" };

        for (int i = 0; i < todos.Count; i++)
        {
            try
            {
                TodoItem item = todos.GetItem(i);
                string escapedText = EscapeCsvText(item.Text);
                string line = $"{i};\"{escapedText}\";{item.Status};{item.LastUpdate:yyyy-MM-ddTHH:mm:ss}";
                lines.Add(line);
            }
            catch (ArgumentOutOfRangeException)
            {
                continue;
            }
        }

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

                TodoItem item = new TodoItem(text);
                item.SetStatus(status);
                item.SetLastUpdate(lastUpdate);

                todoList.Add(item);
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
    public static string GetUserTodoFilePath(Guid userId, string dataDirectory)
    {
        return Path.Combine(dataDirectory, $"todos_{userId}.csv");
    }
}