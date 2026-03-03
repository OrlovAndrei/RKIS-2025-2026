using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TodoList;

public class FileManager
{
    public const string dataDirPath = "data";
    public static string profilesPath = Path.Combine(dataDirPath, "profiles.csv");
    
    public static void EnsureAllData()
    {
        EnsureDataDirectory(dataDirPath);
        if (!File.Exists(profilesPath)) 
            File.WriteAllText(profilesPath, "");
    }
    
    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
    }
    
    public static void SaveProfiles(List<Profile> profiles)
    {
        using var writer = new StreamWriter(profilesPath, false);
        foreach (var profile in profiles)
        {
            writer.WriteLine($"{profile.Id};{profile.Login};{profile.Password};{profile.FirstName};{profile.LastName};{profile.BirthYear}");
        }
    }
    
    public static List<Profile> LoadProfiles()
    {
        var profiles = new List<Profile>();
        
        if (!File.Exists(profilesPath))
            return profiles;
            
        var lines = File.ReadAllLines(profilesPath);
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;
                
            var parts = line.Split(';');
            if (parts.Length != 6)
                continue;
                
            var id = Guid.Parse(parts[0]);
            var login = parts[1];
            var password = parts[2];
            var firstName = parts[3];
            var lastName = parts[4];
            var birthYear = int.Parse(parts[5]);
            
            profiles.Add(new Profile(id, login, password, firstName, lastName, birthYear));
        }
        
        return profiles;
    }
    
    public static string GetTodoFilePath(Guid userId)
    {
        return Path.Combine(dataDirPath, $"todos_{userId}.csv");
    }
    
    public static void SaveTodos(Guid userId, TodoList todoList)
    {
        var todoPath = GetTodoFilePath(userId);
        using var writer = new StreamWriter(todoPath, false);

        for (var i = 0; i < todoList.items.Count; i++)
        {
            var item = todoList.items[i];
            var text = EscapeCsv(item.Text);
            writer.WriteLine($"{i};{text};{item.Status};{item.LastUpdate:O}");
        }
        
        string EscapeCsv(string text)
            => "\"" + text.Replace("\"", "\"\"").Replace("\n", "\\n") + "\"";
    }
    
    public static TodoList LoadTodos(Guid userId)
    {
        var todoPath = GetTodoFilePath(userId);
        var list = new TodoList();

        if (!File.Exists(todoPath))
            return list;

        var lines = File.ReadAllLines(todoPath);
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;
                
            // Ручной парсинг CSV с учётом кавычек
            var fields = ParseCsvLine(line);
            if (fields.Length < 4)
                continue; // игнорируем повреждённые строки

            // fields[0] - индекс (не используется, но можно проверить)
            var text = UnescapeCsv(fields[1]);
            if (!Enum.TryParse<TodoStatus>(fields[2], true, out var status))
                continue; // если статус не распознан, пропускаем
            if (!DateTime.TryParse(fields[3], out var lastUpdate))
                continue;

            list.Add(new TodoItem(text, status, lastUpdate));
        }

        return list;
    }

    // Разделение CSV-строки с поддержкой кавычек
    private static string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    // Экранированная кавычка внутри поля
                    current.Append('"');
                    i++; // пропускаем следующую кавычку
                }
                else
                {
                    inQuotes = !inQuotes;
                }
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
        result.Add(current.ToString()); // последнее поле

        return result.ToArray();
    }
    
    private static string UnescapeCsv(string text)
    {
        if (text.Length >= 2 && text[0] == '"' && text[text.Length - 1] == '"')
            text = text.Substring(1, text.Length - 2);
        return text.Replace("\\n", "\n").Replace("\"\"", "\"");
    }
}