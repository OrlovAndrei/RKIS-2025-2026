namespace TodoList;

internal class Program
{
    public const string dataDirPath = "data";
    public static string profileFilePath = Path.Combine(dataDirPath, "profile.txt");

    private static void Main(string[] args)
    {
        FileManager.EnsureDataDirectory(dataDirPath);
        if (!File.Exists(profileFilePath)) File.WriteAllText(profileFilePath, "Default User 2000");

        Console.WriteLine("Работу выполнили Бурнашов и Хазиев");

        Console.WriteLine($"Добавлен пользователь {CommandParser.profile.GetInfo()}");
        Console.WriteLine("Введите 'help' для списка команд");

        while (true)
        {
            Console.WriteLine("Введите команду: ");
            var input = Console.ReadLine();

            var command = CommandParser.Parse(input);
            command.Execute();
        }
    }
}