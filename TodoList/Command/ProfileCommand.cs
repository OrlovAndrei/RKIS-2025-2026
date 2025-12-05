using System;

public class ProfileCommand : ICommand
{
    public Profile Profile { get; set; }
    public string ProfileFilePath { get; set; }
    public bool ShouldLogout { get; set; }
    public void Execute()
    {
        if (ShouldLogout)
        {
            LogoutProfile();
            return;
        }

        Console.WriteLine(Profile.GetInfo());
        FileManager.SaveProfiles(AppInfo.Profiles, ProfileFilePath);
    }
    private void LogoutProfile()
    {
        if (AppInfo.CurrentProfileId.HasValue)
        {
            string todoFilePath = FileManager.GetUserTodoFilePath(AppInfo.CurrentProfileId.Value, "Data");
            FileManager.SaveTodos(AppInfo.CurrentTodoList, todoFilePath);

            Console.WriteLine($"Выход из профиля: {AppInfo.CurrentProfile?.GetInfo()}");

            AppInfo.CurrentProfileId = null;

            AppInfo.UndoStack.Clear();
            AppInfo.RedoStack.Clear();

            Console.WriteLine("Для продолжения перезапустите программу.");
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Нет активного профиля для выхода.");
        }
    }
    public void Unexecute()
    {
        
    }
}