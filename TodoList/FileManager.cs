using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

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
        string[] lines = { profile.FirstName, profile.LastName, profile.BirthYear.ToString() };
        File.WriteAllLines(filePath, lines);
    }

    public static Profile LoadProfile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Profile file not found", filePath);
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length < 3)
            throw new InvalidDataException("Profile file is corrupted");
        return new Profile(lines[0], lines[1], int.Parse(lines[2]));
    }

    public static void SaveTodos(TodoList todoList, string filePath)
    {
        var lines = new List<string>();
        for (int i = 0; i < todoList.Count; i++)
        {
            var item = todoList.GetItem(i);
            string text = EscapeCsv(item.Text);
            string isDone = item.IsDone.ToString();
            string lastUpdate = item.LastUpdate.ToString("o");
            lines.Add($"{i};{text};{isDone};{lastUpdate}");
        }
        File.WriteAllLines(filePath, lines);
    }

    public static TodoList LoadTodos(string filePath)
    {
        var todoList = new TodoList();
        if (!File.Exists(filePath))
            return todoList;

        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = SplitCsvLine(line);
            if (parts.Length != 4) continue;
            
            string text = UnescapeCsv(parts[1]);
            bool isDone = bool.Parse(parts[2]);
            DateTime lastUpdate = DateTime.Parse(parts[3], null, DateTimeStyles.RoundtripKind);
            todoList.Add(new TodoItem(text, isDone, lastUpdate), silent: true);
        }
        return todoList;
    }

    private static string EscapeCsv(string text)
    {
        text = text.Replace("\"", "\"\"");
        text = text.Replace("\n", "\\n");
        return $"\"{text}\"";
    }

    private static string UnescapeCsv(string text)
    {
        if (text.StartsWith("\"") && text.EndsWith("\""))
            text = text.Substring(1, text.Length - 2);
        text = text.Replace("\\n", "\n");
        text = text.Replace("\"\"", "\"");
        return text;
    }

    private static string[] SplitCsvLine(string line)
    {
        var result = new List<string>();
        bool inQuotes = false;
        var current = new StringBuilder();
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ';' && !inQuotes)
            {
                result.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }
        result.Add(current.ToString());
        return result.ToArray();
    }
}