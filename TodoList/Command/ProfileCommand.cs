using System;

public class ProfileCommand : ICommand
{
    public Profile Profile { get; set; }
    public bool ShouldLogout { get; set; }

    public void Execute()
    {
        if (ShouldLogout)
        {
            LogoutProfile();
            return;
        }

        Console.WriteLine(Profile.GetInfo());
    }

    private void LogoutProfile()
    {
        if (AppInfo.CurrentProfileId.HasValue)
        {
            if (AppInfo.CurrentTodoList != null && AppInfo.CurrentProfileId.HasValue)
            {
                Console.WriteLine($"Выход из профиля: {AppInfo.CurrentProfile?.GetInfo()}");
            }

            AppInfo.CurrentProfileId = null;
            AppInfo.UndoStack.Clear();
            AppInfo.RedoStack.Clear();

            Console.WriteLine("\nПрофиль успешно деактивирован.");
        }
        else
        {
            Console.WriteLine("Нет активного профиля для выхода.");
        }
    }
}