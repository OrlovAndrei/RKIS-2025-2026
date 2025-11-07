using System;
using System.IO;
using System.Collections.Generic;

public static class FileManager
{
    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }

    public static void SaveProfile(Profile profile, string filePath)
    {
        string content = $"{profile.FirstName};{profile.LastName};{profile.BirthYear}";
        File.WriteAllText(filePath, content);
    }

    public static Profile LoadProfile(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        string content = File.ReadAllText(filePath);
        string[] parts = content.Split(';');

        if (parts.Length == 3)
        {
            string firstName = parts[0];
            string lastName = parts[1];
            int birthYear = int.Parse(parts[2]);
            return new Profile(firstName, lastName, birthYear);
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
                TodoItem item = todos.GetItem(i);
                string escapedText = EscapeCsvText(item.Text);
                string line = $"{i};\"{escapedText}\";{item.IsDone};{item.LastUpdate:yyyy-MM-ddTHH:mm:ss}";
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
                bool isDone = bool.Parse(parts[2]);
                DateTime lastUpdate = DateTime.Parse(parts[3]);

                TodoItem item = new TodoItem(text);
                if (isDone)
                {
                    item.MarkDone();
                }
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
}