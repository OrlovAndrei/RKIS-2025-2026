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
                
            var parts = line.Split(';');

            var text = UnescapeCsv(parts[1]);
            var status = Enum.Parse<TodoStatus>(parts[2]);
            var lastUpdate = DateTime.Parse(parts[3]);

            list.Add(new TodoItem(text, status, lastUpdate));
        }

        return list;
        
        string UnescapeCsv(string text)
            => text.Trim('"').Replace("\\n", "\n").Replace("\"\"", "\"");
    }
}