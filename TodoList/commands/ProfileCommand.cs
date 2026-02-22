using TodoList.Exceptions;

namespace TodoList.commands;

public class ProfileCommand : ICommand
{
    public required string[] parts { get; set; }
    public Profile? OldProfile { get; private set; }
    public Profile? NewProfile { get; private set; }
    private bool isLogoutCommand = false;

    public void Execute()
    {
        if (parts.Length == 1)
        {
            if (AppInfo.CurrentProfile == null)
                throw new ProfileNotFoundException("Нет активного профиля.");
            Console.WriteLine(AppInfo.CurrentProfile.GetInfo());
            return;
        }

        var flags = ParseFlags(string.Join(" ", parts));
        if (flags.Contains("--out") || flags.Contains("-o"))
        {
            Logout();
            return;
        }

        var subCommand = parts[1].ToLower();
        switch (subCommand)
        {
            case "установить":
            case "set":
                Console.WriteLine("В новой системе профиль устанавливается при входе. Используйте 'profile --out' для выхода и создайте новый профиль.");
                break;
            case "показать":
            case "show":
                if (AppInfo.CurrentProfile == null)
                    throw new ProfileNotFoundException("Нет активного профиля.");
                Console.WriteLine(AppInfo.CurrentProfile.GetInfo());
                break;
            default:
                throw new InvalidCommandException($"Неизвестная подкоманда '{subCommand}' для profile.");
        }
    }
    
    private void Logout()
    {
        isLogoutCommand = true;
        Console.WriteLine("Выход из профиля...");
        
        FileManager.SaveProfiles(AppInfo.Profiles);
        
        AppInfo.UndoStack.Clear();
        AppInfo.RedoStack.Clear();
        
        AppInfo.CurrentProfileId = null;
        
        Console.WriteLine("Вы вышли из профиля. Перезапустите программу для входа в другой профиль.");
        Environment.Exit(0);
    }
    
    public void Unexecute()
    {
        if (isLogoutCommand)
            return;
            
        if (OldProfile != null && NewProfile != null)
        {
            Console.WriteLine("Отмена смены профиля невозможна в новой системе.");
        }
    }
    
    private static string[] ParseFlags(string command)
    {
        var parts = command.Split(' ');
        var flags = new List<string>();

        foreach (var part in parts)
            if (part.StartsWith("--"))
                flags.Add(part);
            else if (part.StartsWith("-"))
                for (var i = 1; i < part.Length; i++)
                    flags.Add("-" + part[i]);

        return flags.ToArray();
    }
}