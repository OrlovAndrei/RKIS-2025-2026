using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TodoList;

public static class FileManager
{
    private static string DataDir => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
    private static string ProfilesPath => Path.Combine(DataDir, "profiles.csv");

    public static void EnsureDataDirectory()
    {
        if (!Directory.Exists(DataDir))
            Directory.CreateDirectory(DataDir);
    }

    public static List<Profile> LoadAllProfiles()
    {
        var profiles = new List<Profile>();
        if (!File.Exists(ProfilesPath))
            return profiles;

        var lines = File.ReadAllLines(ProfilesPath);
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = SplitCsvLine(line);
            if (parts.Length != 6) continue;

            if (Guid.TryParse(parts[0], out var id))
            {
                var profile = new Profile(
                    id,
                    parts[1],
                    parts[2],
                    parts[3],
                    parts[4],
                    int.Parse(parts[5])
                );
                profiles.Add(profile);
            }
        }
        return profiles;
    }

    public static void SaveAllProfiles(List<Profile> profiles)
    {
        var lines = new List<string>();
        foreach (var p in profiles)
        {
            lines.Add($"{p.Id};{EscapeCsv(p.Login)};{EscapeCsv(p.Password)};{EscapeCsv(p.FirstName)};{EscapeCsv(p.LastName)};{p.BirthYear}");
        }
        File.WriteAllLines(ProfilesPath, lines);
    }

    public static void SaveProfile(Profile profile)
    {
        var profiles = LoadAllProfiles();
        var existing = profiles.FirstOrDefault(p => p.Id == profile.Id);
        if (existing != null)
            profiles.Remove(existing);
        profiles.Add(profile);
        SaveAllProfiles(profiles);
    }

    private static string GetTodoFilePath(Guid userId) => Path.Combine(DataDir, $"todos_{userId}.csv");

    public static TodoList LoadTodosForUser(Guid userId)
    {
        var todoList = new TodoList();
        string path = GetTodoFilePath(userId);
        if (!File.Exists(path))
            return todoList;

        var lines = File.ReadAllLines(path);
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = SplitCsvLine(line);
            if (parts.Length != 4) continue;

            string text = UnescapeCsv(parts[1]);
            TodoStatus status = Enum.Parse<TodoStatus>(parts[2]);
            DateTime lastUpdate = DateTime.Parse(parts[3], null, DateTimeStyles.RoundtripKind);
            todoList.Add(new TodoItem(text, status, lastUpdate), silent: true);
        }
        return todoList;
    }

    public static void SaveTodosForUser(Guid userId, TodoList todoList)
    {
        var lines = new List<string>();
        for (int i = 0; i < todoList.Count; i++)
        {
            var item = todoList[i];
            string text = EscapeCsv(item.Text);
            string status = item.Status.ToString();
            string lastUpdate = item.LastUpdate.ToString("o");
            lines.Add($"{i};{text};{status};{lastUpdate}");
        }
        File.WriteAllLines(GetTodoFilePath(userId), lines);
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
                inQuotes = !inQuotes;
            else if (c == ';' && !inQuotes)
            {
                result.Add(current.ToString());
                current.Clear();
            }
            else
                current.Append(c);
        }
        result.Add(current.ToString());
        return result.ToArray();
    }
}