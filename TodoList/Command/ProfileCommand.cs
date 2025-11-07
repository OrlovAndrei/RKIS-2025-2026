using System;

public class ProfileCommand : ICommand
{
    public Profile Profile { get; set; }

    public string ProfileFilePath { get; set; }
    public void Execute()
    {
        Console.WriteLine(Profile.GetInfo());

        if (!string.IsNullOrEmpty(ProfileFilePath))
        {
            FileManager.SaveProfile(Profile, ProfileFilePath);
            Console.WriteLine("Профиль сохранен.");
        }
    }
}