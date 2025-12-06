namespace TodoList.commands;

public class ExitCommand : ICommand
{
    public Profile? Profile { get; set; }
    public void Execute()
    {
        if (Profile != null) FileManager.SaveProfile(Profile, Program.ProfileFilePath);
        Console.WriteLine("Выход из программы.");
        Environment.Exit(0);
    }
}