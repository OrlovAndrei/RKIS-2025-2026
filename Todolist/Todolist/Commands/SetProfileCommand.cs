namespace TodoList.Commands;

public class SetProfileCommand : ICommand
{
    public void Execute()
    {
        Console.Write("Введите имя: ");
        var firstName = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        var lastName = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        var yearInput = Console.ReadLine();

        var birthYear = int.Parse(yearInput);

        CommandParser.profile = new Profile(firstName, lastName, birthYear);
        FileManager.SaveProfile(CommandParser.profile, Program.profileFilePath);
    }
}